﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Bounded;

using Calendrie.Systems;
using Calendrie.Testing.Data.Unbounded;

/// <summary>
/// Defines test data for a calendar with years within the range [1..9999] and provides a base for
/// derived classes.
/// </summary>
/// <typeparam name="TDataSet">The type that represents the original calendar dataset.</typeparam>
public class StandardCalendarDataSet<TDataSet> : MinMaxYearCalendarDataSet<TDataSet>
    where TDataSet : UnboundedCalendarDataSet
{
    public StandardCalendarDataSet(TDataSet inner)
        : base(inner, StandardScope.MinYear, StandardScope.MaxYear) { }
}

/// <summary>
/// Provides test data for the Armenian calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardArmenian12DataSet :
    StandardCalendarDataSet<UnboundedArmenian12DataSet>, IEpagomenalDataSet, ISingleton<StandardArmenian12DataSet>
{
    private StandardArmenian12DataSet() : base(UnboundedArmenian12DataSet.Instance) { }

    public static StandardArmenian12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardArmenian12DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Unbounded.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Armenian13 calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardArmenian13DataSet :
    StandardCalendarDataSet<UnboundedArmenian13DataSet>, IEpagomenalDataSet, ISingleton<StandardArmenian13DataSet>
{
    private StandardArmenian13DataSet() : base(UnboundedArmenian13DataSet.Instance) { }

    public static StandardArmenian13DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardArmenian13DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Unbounded.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Coptic calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardCoptic12DataSet :
    StandardCalendarDataSet<UnboundedCoptic12DataSet>, IEpagomenalDataSet, ISingleton<StandardCoptic12DataSet>
{
    private StandardCoptic12DataSet() : base(UnboundedCoptic12DataSet.Instance) { }

    public static StandardCoptic12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardCoptic12DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Unbounded.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Coptic13 calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardCoptic13DataSet :
    StandardCalendarDataSet<UnboundedCoptic13DataSet>, IEpagomenalDataSet, ISingleton<StandardCoptic13DataSet>
{
    private StandardCoptic13DataSet() : base(UnboundedCoptic13DataSet.Instance) { }

    public static StandardCoptic13DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardCoptic13DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Unbounded.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Ethiopic calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardEthiopic12DataSet :
    StandardCalendarDataSet<UnboundedEthiopic12DataSet>, IEpagomenalDataSet, ISingleton<StandardEthiopic12DataSet>
{
    private StandardEthiopic12DataSet() : base(UnboundedEthiopic12DataSet.Instance) { }

    public static StandardEthiopic12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardEthiopic12DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Unbounded.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Ethiopic13 calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardEthiopic13DataSet :
    StandardCalendarDataSet<UnboundedEthiopic13DataSet>, IEpagomenalDataSet, ISingleton<StandardEthiopic13DataSet>
{
    private StandardEthiopic13DataSet() : base(UnboundedEthiopic13DataSet.Instance) { }

    public static StandardEthiopic13DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardEthiopic13DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Unbounded.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Gregorian calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardGregorianDataSet :
    StandardCalendarDataSet<UnboundedGregorianDataSet>,
    IDayOfWeekDataSet,
    ISingleton<StandardGregorianDataSet>
{
    private StandardGregorianDataSet() : base(UnboundedGregorianDataSet.Instance) { }

    public static StandardGregorianDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardGregorianDataSet Instance = new();
        static Singleton() { }
    }

    // IDayOfWeekDataSet
    public DataGroup<YemodaAnd<DayOfWeek>> DayOfWeekData => Unbounded.DayOfWeekData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Before_Data => Unbounded.DayOfWeek_Before_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrBefore_Data => Unbounded.DayOfWeek_OnOrBefore_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Nearest_Data => Unbounded.DayOfWeek_Nearest_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrAfter_Data => Unbounded.DayOfWeek_OnOrAfter_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_After_Data => Unbounded.DayOfWeek_After_Data.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Positivist calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardPositivistDataSet :
    StandardCalendarDataSet<UnboundedPositivistDataSet>, ISingleton<StandardPositivistDataSet>
{
    private StandardPositivistDataSet() : base(UnboundedPositivistDataSet.Instance) { }

    public static StandardPositivistDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardPositivistDataSet Instance = new();
        static Singleton() { }
    }
}

/// <summary>
/// Provides test data for the Tabular Islamic calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardTabularIslamicDataSet :
    StandardCalendarDataSet<UnboundedTabularIslamicDataSet>, ISingleton<StandardTabularIslamicDataSet>
{
    private StandardTabularIslamicDataSet() : base(UnboundedTabularIslamicDataSet.Instance) { }

    public static StandardTabularIslamicDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardTabularIslamicDataSet Instance = new();
        static Singleton() { }
    }
}

/// <summary>
/// Provides test data for the Zoroastrian calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardZoroastrian12DataSet :
    StandardCalendarDataSet<UnboundedZoroastrian12DataSet>, IEpagomenalDataSet, ISingleton<StandardZoroastrian12DataSet>
{
    private StandardZoroastrian12DataSet() : base(UnboundedZoroastrian12DataSet.Instance) { }

    public static StandardZoroastrian12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardZoroastrian12DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Unbounded.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Zoroastrian13 calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardZoroastrian13DataSet :
    StandardCalendarDataSet<UnboundedZoroastrian13DataSet>, IEpagomenalDataSet, ISingleton<StandardZoroastrian13DataSet>
{
    private StandardZoroastrian13DataSet() : base(UnboundedZoroastrian13DataSet.Instance) { }

    public static StandardZoroastrian13DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardZoroastrian13DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Unbounded.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the World calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardWorldDataSet :
    StandardCalendarDataSet<UnboundedWorldDataSet>, ISingleton<StandardWorldDataSet>
{
    private StandardWorldDataSet() : base(UnboundedWorldDataSet.Instance) { }

    public static StandardWorldDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardWorldDataSet Instance = new();
        static Singleton() { }
    }
}
