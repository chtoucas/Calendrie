﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Horology.Ntp;

using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using Calendrie.Core.Intervals;

using static Calendrie.Core.TemporalConstants;

// No default host like ntp.pool.org; see
// https://www.pool.ntp.org/vendors.html
// https://en.wikipedia.org/wiki/NTP_server_misuse_and_abuse

// Adapted from
// https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/SntpClient.java
// See also
// https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/util/NtpTrustedTime.java
// https://android.googlesource.com/platform/frameworks/base/+/master/core/tests/coretests/src/android/net/sntp/

/// <summary>
/// Provides a stateless client for the Network Time Protocol (NTP).
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class SntpClient
{
    /// <summary>
    /// Represents the default SNTP version.
    /// <para>This field is a constant equal to 4.</para>
    /// </summary>
    public const int DefaultVersion = 4;

    /// <summary>
    /// Represents the default SNTP port.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    public const int DefaultPort = 123;

    /// <summary>
    /// Represents the default amount of time in milliseconds after which a
    /// synchronous call Send will time out.
    /// <para>This field is a constant equal to 500 milliseconds.</para>
    /// </summary>
    public const int DefaultSendTimeout = 500;

    /// <summary>
    /// Represents the default amount of time in milliseconds after which a
    /// synchronous call Receive will time out.
    /// <para>This field is a constant equal to 500 milliseconds.</para>
    /// </summary>
    public const int DefaultReceiveTimeout = 500;

    /// <summary>
    /// Represents the random number generator.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly IRandomNumberGenerator _rng = new DefaultRandomNumberGenerator();

    /// <summary>
    /// Initializes a new instance of the <see cref="SntpClient"/> class.
    /// </summary>
    public SntpClient(string host)
    {
        EndPoint = new DnsEndPoint(host, DefaultPort);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SntpClient"/> class.
    /// </summary>
    public SntpClient(EndPoint endpoint)
    {
        ArgumentNullException.ThrowIfNull(endpoint);

        EndPoint = endpoint;
    }

    public static Segment<int> SupportedVersions { get; } = Segment.Create(3, 4);

    private int _version = DefaultVersion;
    /// <summary>
    /// Gets or initializes the NTP version.
    /// </summary>
    public int Version
    {
        get => _version;
        init
        {
            if (!SupportedVersions.Contains(value))
                throw new ArgumentException(null, nameof(value));
            _version = value;
        }
    }

    /// <summary>
    /// Gets or sets a value that specifies the amount of time in milliseconds
    /// after which a synchronous send call to the undelying socket will time out.
    /// </summary>
    public int SendTimeout { get; set; } = DefaultSendTimeout;

    /// <summary>
    /// Gets or sets a value that specifies the amount of time in milliseconds
    /// after which a synchronous receive call to the undelying socket will time
    /// out.
    /// </summary>
    public int ReceiveTimeout { get; set; } = DefaultReceiveTimeout;

    /// <summary>
    /// Gets or initializes the random number generator.
    /// </summary>
    public IRandomNumberGenerator RandomNumberGenerator
    {
        get => _rng;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            _rng = value;
        }
    }

    /// <summary>
    /// Enables or disables the version check.
    /// <para>There are still old (or bogus?) NTP servers always returning 3,
    /// therefore the default behaviour is to not check the version number.</para>
    /// </summary>
    /// <value>true if this instance checks that the version number found in the
    /// NTP response matches <see cref="Version"/>; otherwise returns false.
    /// </value>
    //
    // An NTP server may always return 3, e.g. "time.windows.com"
    // or "time.nist.gov".
    public bool EnableVersionCheck { get; set; }

    /// <summary>
    /// Gets the network address for the SNTP server.
    /// </summary>
    private EndPoint EndPoint { get; }

    /// <summary>
    /// Gets the first byte of the NTP packet.
    /// </summary>
    private byte FirstByte
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            const int ClientMode = (int)NtpMode.Client - 1; // 3

            // Initialize the first byte to: LI = 0, VN = 3 or 4, Mode = 3.
            // Version 3: 00 011 011 (0x1b)
            // Version 4: 00 100 011 (0x23)
            return (byte)((Version << 3) | ClientMode);
        }
    }

    /// <summary>
    /// Queries the SNTP server.
    /// </summary>
    [Pure]
    public NtpResponse QueryTime()
    {
        Span<byte> buf = stackalloc byte[NtpPacket.BinarySize];
        buf[0] = FirstByte;

        // A system clock is not monotonic (unattended synchronization).
        // Do NOT write
        // > var responseTime = DateTime.UtcNow;
        // otherwise we could end up with responseTime < requestTime!
        var stopwatch = Stopwatch.StartNew();

        using var sock = new Socket(SocketType.Dgram, ProtocolType.Udp)
        {
            SendTimeout = SendTimeout,
            ReceiveTimeout = ReceiveTimeout,
        };

        sock.Connect(EndPoint);

        // Start time on this side of the network.
        var requestTime = DateTime.UtcNow;
        long startTicks = stopwatch.ElapsedTicks;
        // Randomize the timestamp, then write the result into the buffer.
        var requestTimestamp =
            Timestamp64.FromDateTime(requestTime)
                .RandomizeSubMilliseconds(RandomNumberGenerator);
        requestTimestamp.WriteTo(buf, NtpPacket.TransmitTimestampOffset);

        _ = sock.Send(buf);
        _ = sock.Receive(buf);

        long endTicks = stopwatch.ElapsedTicks;

        // Elapsed ticks during the query on this side of the network.
        long elapsedTicks = endTicks - startTicks;
        // End time on this side of the network.
        var responseTime = requestTime.AddMilliseconds(elapsedTicks / TicksPerMillisecond);
        var responseTimestamp = Timestamp64.FromDateTime(responseTime);

        return ReadResponse(buf, requestTimestamp, responseTimestamp);
    }

    /// <summary>
    /// Queries the SNTP server asynchronously.
    /// </summary>
    [Pure]
    public async Task<NtpResponse> QueryTimeAsync()
    {
        byte[] bytes = new byte[NtpPacket.BinarySize];
        bytes[0] = FirstByte;

        var stopwatch = Stopwatch.StartNew();

        using var sock = new Socket(SocketType.Dgram, ProtocolType.Udp);

        await sock.ConnectAsync(EndPoint).ConfigureAwait(false);

        var requestTime = DateTime.UtcNow;
        long startTicks = stopwatch.ElapsedTicks;

        var requestTimestamp =
            Timestamp64.FromDateTime(requestTime)
                .RandomizeSubMilliseconds(RandomNumberGenerator);
        requestTimestamp.WriteTo(bytes, NtpPacket.TransmitTimestampOffset);

        var buf = new ArraySegment<byte>(bytes);
        _ = await sock.SendAsync(buf, SocketFlags.None).ConfigureAwait(false);
        _ = await sock.ReceiveAsync(buf, SocketFlags.None).ConfigureAwait(false);

        long endTicks = stopwatch.ElapsedTicks;

        long elapsedTicks = endTicks - startTicks;
        var responseTime = requestTime.AddMilliseconds(elapsedTicks / TicksPerMillisecond);
        var responseTimestamp = Timestamp64.FromDateTime(responseTime);

        return ReadResponse(buf, requestTimestamp, responseTimestamp);
    }

    /// <summary>
    /// Reads an <see cref="NtpResponse"/> value from the beginning of a
    /// read-only span of bytes.
    /// </summary>
    [Pure]
    private NtpResponse ReadResponse(
        ReadOnlySpan<byte> buf,
        Timestamp64 requestTimestamp,
        Timestamp64 responseTimestamp)
    {
        var pkt = NtpPacket.ReadFrom(buf);
        CheckPacket(in pkt);
        // The only fields not yet checked are those that the server is expected
        // to copy verbatim from the request.
        if (EnableVersionCheck && pkt.Version != Version)
        {
            NtpException.Throw(FormattableString.Invariant(
                $"Version missmatch: expected {Version}, received {pkt.Version}."));
        }
        if (pkt.OriginateTimestamp != requestTimestamp)
            NtpException.Throw("Originate Timestamp does not match the Request Timestamp.");

        var si = new NtpServerInfo
        {
            LeapIndicator = pkt.LeapIndicator,
            Version = pkt.Version,
            StratumLevel = pkt.StratumLevel,
            PollExponent = pkt.PollExponent,
            PrecisionExponent = pkt.PrecisionExponent,
            RoundTripTime = pkt.RootDelay,
            Dispersion = pkt.RootDispersion,
            ReferenceId = pkt.ReferenceId,
            ReferenceTimestamp = pkt.ReferenceTimestamp,
        };

        var ti = new NtpTimeInfo
        {
            RequestTimestamp = requestTimestamp,
            ReceiveTimestamp = pkt.ReceiveTimestamp,
            TransmitTimestamp = pkt.TransmitTimestamp,
            ResponseTimestamp = responseTimestamp
        };

        return new NtpResponse(si, ti);
    }

    /// <summary>
    /// Checks the specified packet according to RFC 4330, section 5 (client
    /// operations).
    /// </summary>
    //
    // Simple check (unicast mode):
    // - Mode == NtpMode.Server (4)
    // - StratumLevel <= MaxStratumLevel (15)
    // - TransmitTimestamp != Timestamp64.Zero
    // Other things we could check:
    // - IP addresses
    private static void CheckPacket(in NtpPacket pkt)
    {
        // Legit stratums: primary or secondary reference.
        const byte
            MinStratumLevel = 1,
            MaxStratumLevel = 15;

        // It should not be possible to end up here with an invalid LI.
        Debug.Assert(pkt.LeapIndicator != LeapIndicator.Unknown);
        if (pkt.LeapIndicator == LeapIndicator.Unsynchronized)
        {
            NtpException.Throw(FormattableString.Invariant(
                $"The server clock is not synchronised."));
        }
        if (pkt.Mode != NtpMode.Server)
        {
            NtpException.Throw(FormattableString.Invariant(
                $"Invalid mode: received \"{pkt.Mode}\" but it should be \"Server\"."));
        }
        if (pkt.StratumLevel < MinStratumLevel || pkt.StratumLevel > MaxStratumLevel)
        {
            NtpException.Throw(FormattableString.Invariant(
                $"The server is either unavailable or unsynchronised: StratumLevel = {pkt.StratumLevel}."));
        }
        // The server clock should be monotonically increasing.
        if (pkt.ReceiveTimestamp < pkt.ReferenceTimestamp)
        {
            NtpException.Throw(FormattableString.Invariant(
                $"The server clock should be monotonically increasing: Receive Timestamp ({pkt.ReceiveTimestamp}) < Reference Timestamp ({pkt.ReferenceTimestamp})."));
        }
        if (pkt.TransmitTimestamp < pkt.ReceiveTimestamp)
        {
            NtpException.Throw(FormattableString.Invariant(
                $"The server clock should be monotonically increasing: Transmit Timestamp ({pkt.TransmitTimestamp}) < Receive Timestamp ({pkt.ReceiveTimestamp})."));
        }
        // TransmitTimestamp >= ReceiveTimestamp >= ReferenceTimestamp
        // therefore if ReferenceTimestamp != 0 i.e. > 0, then the other
        // timestamps are non-zero too.
        if (pkt.ReferenceTimestamp == Timestamp64.Zero)
            NtpException.Throw("Reference Timestamp = 0.");
    }
}
