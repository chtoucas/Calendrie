﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

#if ENABLE_SERIALIZATION

/// <summary>
/// Provides a method to serialize an object.
/// </summary>
/// <typeparam name="TBinary">The type of binary data.</typeparam>
internal interface ISerializable<out TBinary>
{
    /// <summary>
    /// Serializes the current instance to a binary value that subsequently can
    /// be used to recreate the object.
    /// </summary>
    [Pure] TBinary ToBinary();
}

/// <summary>
/// Defines a serializable type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="TBinary">The type of binary data.</typeparam>
internal interface ISerializable<TSelf, TBinary> :
    ISerializable<TBinary>
    where TSelf : ISerializable<TSelf, TBinary>
{
    /// <summary>
    /// Deserializes a binary value and recreates an original serialized
    /// <typeparamref name="TSelf"/> object.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Invalid input.</exception>
    [Pure] static abstract TSelf FromBinary(TBinary data);
}

#endif
