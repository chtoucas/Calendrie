// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.CalendricalSchemaTestSuite

open System

open Calendrie
open Calendrie.Core
open Calendrie.Core.Intervals
open Calendrie.Core.Schemas
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core
open Calendrie.Testing.Faux

open Xunit

// TODO(fact): Pax (unfinished).

[<Sealed>]
type CivilTests() =
    inherit CalendricalSchemaFacts<CivilSchema, StandardGregorianDataSet>(new CivilSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<GregorianPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYears_Prop() =
        let range = CalendricalSchema.DefaultSupportedYears.WithMin(1)
        x.SchemaUT.SupportedYears === range

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<CivilSchema>()

[<Sealed>]
type Coptic12Tests() =
    inherit CalendricalSchemaFacts<Coptic12Schema, Coptic12DataSet>(new Coptic12Schema())

    static member EpagomenalDayInfoData with get() = Coptic12Tests.DataSet.EpagomenalDayInfoData

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.EndingAt(Int32.MaxValue - 1)

    [<Fact>]
    member x.``CountDaysInMonth() overflows when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        for m in 1 .. x.MaxMonth do
            (fun () -> sch.CountDaysInMonth(Int32.MaxValue, m)) |> overflows

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<Coptic12Schema>()

    //
    // Featurettes
    //

    [<Theory; MemberData(nameof(Coptic12Tests.DateInfoData))>]
    member x.``IsEpagomenalDay()`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        // Act
        let isEpagomenal, epanum = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        info.IsSupplementary === isEpagomenal
        if isEpagomenal then
            epanum > 0 |> ok
        else
            epanum === 0

    [<Theory; MemberData(nameof(Coptic12Tests.EpagomenalDayInfoData))>]
    member x.``IsEpagomenalDay() check out param`` (info: YemodaAnd<int>) =
        let y, m, d, epanum = info.Deconstruct()
        // Act
        let isEpagomenal, epagomenalNumber = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        isEpagomenal |> ok
        epagomenalNumber === epanum

[<Sealed>]
type Coptic13Tests() =
    inherit CalendricalSchemaFacts<Coptic13Schema, Coptic13DataSet>(new Coptic13Schema())

    static member EpagomenalDayInfoData with get() = Coptic13Tests.DataSet.EpagomenalDayInfoData

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.EndingAt(Int32.MaxValue - 1)

    [<Fact>]
    member x.``CountDaysInMonth() overflows when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        for m in 1 .. x.MaxMonth do
            (fun () -> sch.CountDaysInMonth(Int32.MaxValue, m)) |> overflows

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<Coptic13Schema>()

    //
    // Featurettes
    //

    [<Theory; MemberData(nameof(Coptic13Tests.DateInfoData))>]
    member x.``IsEpagomenalDay()`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        // Act
        let isEpagomenal, epanum = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        info.IsSupplementary === isEpagomenal
        if isEpagomenal then
            epanum > 0 |> ok
        else
            epanum === 0

    [<Theory; MemberData(nameof(Coptic13Tests.EpagomenalDayInfoData))>]
    member x.``IsEpagomenalDay() check out param`` (info: YemodaAnd<int>) =
        let y, m, d, epanum = info.Deconstruct()
        // Act
        let isEpagomenal, epagomenalNumber = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        isEpagomenal |> ok
        epagomenalNumber === epanum

[<Sealed>]
type Egyptian12Tests() =
    inherit CalendricalSchemaFacts<Egyptian12Schema, Egyptian12DataSet>(new Egyptian12Schema())

    static member EpagomenalDayInfoData with get() = Egyptian12Tests.DataSet.EpagomenalDayInfoData

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<Egyptian12Schema>()

    //
    // Featurettes
    //

    [<Theory; MemberData(nameof(Egyptian12Tests.DateInfoData))>]
    member x.``IsEpagomenalDay()`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        // Act
        let isEpagomenal, epanum = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        info.IsSupplementary === isEpagomenal
        if isEpagomenal then
            epanum > 0 |> ok
        else
            epanum === 0

    [<Theory; MemberData(nameof(Egyptian12Tests.EpagomenalDayInfoData))>]
    member x.``IsEpagomenalDay() check out param`` (info: YemodaAnd<int>) =
        let y, m, d, epanum = info.Deconstruct()
        // Act
        let isEpagomenal, epagomenalNumber = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        isEpagomenal |> ok
        epagomenalNumber === epanum

[<Sealed>]
type Egyptian13Tests() =
    inherit CalendricalSchemaFacts<Egyptian13Schema, Egyptian13DataSet>(new Egyptian13Schema())

    static member EpagomenalDayInfoData with get() = Egyptian13Tests.DataSet.EpagomenalDayInfoData

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<Egyptian13Schema>()

    //
    // Featurettes
    //

    [<Theory; MemberData(nameof(Egyptian13Tests.DateInfoData))>]
    member x.``IsEpagomenalDay()`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        // Act
        let isEpagomenal, epanum = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        info.IsSupplementary === isEpagomenal
        if isEpagomenal then
            epanum > 0 |> ok
        else
            epanum === 0

    [<Theory; MemberData(nameof(Egyptian13Tests.EpagomenalDayInfoData))>]
    member x.``IsEpagomenalDay() check out param`` (info: YemodaAnd<int>) =
        let y, m, d, epanum = info.Deconstruct()
        // Act
        let isEpagomenal, epagomenalNumber = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        isEpagomenal |> ok
        epagomenalNumber === epanum

[<Sealed>]
type FrenchRepublican12Tests() =
    inherit CalendricalSchemaFacts<FrenchRepublican12Schema, FrenchRepublican12DataSet>(new FrenchRepublican12Schema())

    static member EpagomenalDayInfoData with get() = FrenchRepublican12Tests.DataSet.EpagomenalDayInfoData

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<FrenchRepublican12Schema>()

    //
    // Featurettes
    //

    [<Theory; MemberData(nameof(FrenchRepublican12Tests.DateInfoData))>]
    member x.``IsEpagomenalDay()`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        // Act
        let isEpagomenal, epanum = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        info.IsSupplementary === isEpagomenal
        if isEpagomenal then
            epanum > 0 |> ok
        else
            epanum === 0

    [<Theory; MemberData(nameof(FrenchRepublican12Tests.EpagomenalDayInfoData))>]
    member x.``IsEpagomenalDay() check out param`` (info: YemodaAnd<int>) =
        let y, m, d, epanum = info.Deconstruct()
        // Act
        let isEpagomenal, epagomenalNumber = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        isEpagomenal |> ok
        epagomenalNumber === epanum

[<Sealed>]
type FrenchRepublican13Tests() =
    inherit CalendricalSchemaFacts<FrenchRepublican13Schema, FrenchRepublican13DataSet>(new FrenchRepublican13Schema())

    static member EpagomenalDayInfoData with get() = FrenchRepublican13Tests.DataSet.EpagomenalDayInfoData

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<FrenchRepublican13Schema>()

    //
    // Featurettes
    //

    [<Theory; MemberData(nameof(FrenchRepublican13Tests.DateInfoData))>]
    member x.``IsEpagomenalDay()`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        // Act
        let isEpagomenal, epanum = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        info.IsSupplementary === isEpagomenal
        if isEpagomenal then
            epanum > 0 |> ok
        else
            epanum === 0

    [<Theory; MemberData(nameof(FrenchRepublican13Tests.EpagomenalDayInfoData))>]
    member x.``IsEpagomenalDay() check out param`` (info: YemodaAnd<int>) =
        let y, m, d, epanum = info.Deconstruct()
        // Act
        let isEpagomenal, epagomenalNumber = x.SchemaUT.IsEpagomenalDay(y, m, d)
        // Assert
        isEpagomenal |> ok
        epagomenalNumber === epanum

[<Sealed>]
type GregorianTests() =
    inherit CalendricalSchemaFacts<GregorianSchema, GregorianDataSet>(new GregorianSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<GregorianPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<GregorianSchema>()

[<Sealed>]
type InternationalFixedTests() =
    inherit CalendricalSchemaFacts<InternationalFixedSchema, InternationalFixedDataSet>(new InternationalFixedSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar13
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<InternationalFixedSchema>()

    //
    // Featurettes
    //

    [<Theory; MemberData(nameof(FrenchRepublican13Tests.DateInfoData))>]
    member x.``IsBlankDay()`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        // Act & Assert
        x.SchemaUT.IsBlankDay(y, m, d) === info.IsSupplementary

[<Sealed>]
type JulianTests() =
    inherit CalendricalSchemaFacts<JulianSchema, JulianDataSet>(new JulianSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<JulianPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<JulianSchema>()

[<Sealed>]
type FauxLunisolarTests() =
    inherit CalendricalSchemaFacts<FauxLunisolarSchema, FauxLunisolarDataSet>(new FauxLunisolarSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Lunisolar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Months
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Lunisolar
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunisolarPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (false, 0)

[<Sealed>]
type PaxTests() =
    inherit ICalendricalSchemaFacts<PaxSchema, PaxDataSet>(new PaxSchema())

    static member MoreYearInfoData with get() = PaxDataSet.MoreYearInfoData
    static member MoreMonthInfoData with get() = PaxDataSet.MoreMonthInfoData
    static member WeekInfoData with get() = PaxDataSet.WeekInfoData
    static member DaysSinceEpochYewedaInfoData with get() = PaxDataSet.DaysSinceEpochYewedaInfoData

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Other
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Weeks
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PaxPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (false, 0)

    override x.SupportedYears_Prop() = ()
        //let range = CalendricalSchema.DefaultSupportedYears.WithMin(1)
        //x.SchemaUT.SupportedYears === range

    [<Fact>]
    member x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<PaxSchema>()

    //
    // Featurettes
    //

    [<Theory; MemberData(nameof(PaxTests.MoreMonthInfoData))>]
    member x.``IsPaxMonth()`` (info: YemoAnd<bool, bool>) =
        let (y, m, isPaxMonthOfYear, _) = info.Deconstruct()
        // Act & Assert
        x.SchemaUT.IsPaxMonth(y, m) === isPaxMonthOfYear

    [<Theory; MemberData(nameof(PaxTests.MoreMonthInfoData))>]
    member x.``IsLastMonthOfYear()`` (info: YemoAnd<bool, bool>) =
        let (y, m, _, isLastMonthOfYear) = info.Deconstruct()
        // Act & Assert
        x.SchemaUT.IsLastMonthOfYear(y, m) === isLastMonthOfYear

    //
    // ILeapWeekSchema
    //

    [<Fact>]
    member x.``Property FirstDayOfWeek``() =
        x.SchemaUT.FirstDayOfWeek === DayOfWeek.Sunday

    [<Theory; MemberData(nameof(PaxTests.WeekInfoData))>]
    member x.``IsIntercalaryWeek()`` (info: WeekInfo) =
        let (y, woy) = info.Yewe.Deconstruct()
        // Act & Assert
        x.SchemaUT.IsIntercalaryWeek(y, woy) === info.IsIntercalary

    [<Theory; MemberData(nameof(PaxTests.MoreYearInfoData))>]
    member x.``CountWeeksInYear()`` (info: YearAnd<int>) =
        let (y, weeksInYear) = info.Deconstruct()
        // Act & Assert
        x.SchemaUT.CountWeeksInYear(y) === weeksInYear

    [<Theory; MemberData(nameof(PaxTests.DaysSinceEpochYewedaInfoData))>]
    member x.``CountDaysSinceEpoch()`` (info: DaysSinceEpochYewedaInfo) =
        let (daysSinceEpoch, y, woy, dow) = info.Deconstruct()
        // Act & Assert
        x.SchemaUT.CountDaysSinceEpoch(y, woy, dow) === daysSinceEpoch

[<Sealed>]
type Persian2820Tests() =
    inherit CalendricalSchemaFacts<Persian2820Schema, Persian2820DataSet>(new Persian2820Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.StartingAt(Int32.MinValue + Persian2820Schema.YearZero)

    [<Fact>]
    member x.``CountDaysInMonth() may overflow when y = Int32.MinValue``() =
        let sch = x.SchemaUT

        // No overflow if y >= Int32.MinValue + Persian2820Schema.Year0.
        // It is a bit more complicated, it depends on the value of month.

        // No underflow if m < 12.
        [for m in 1 .. 11 -> sch.CountDaysInMonth(Int32.MinValue, m)] |> ignore

        for y0 in 0 .. (Persian2820Schema.YearZero - 1) do
            for m in 12 .. x.MaxMonth do
                (fun () -> sch.CountDaysInMonth(Int32.MinValue + y0, m)) |> overflows

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<Persian2820Schema>()

[<Sealed>]
type PositivistTests() =
    inherit CalendricalSchemaFacts<PositivistSchema, PositivistDataSet>(new PositivistSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar13
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<PositivistSchema>()

    //
    // Featurettes
    //

    [<Theory; MemberData(nameof(PositivistTests.DateInfoData))>]
    member x.``IsBlankDay()`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        // Act & Assert
        x.SchemaUT.IsBlankDay(y, m, d) === info.IsSupplementary

[<Sealed>]
type TabularIslamicTests() =
    inherit CalendricalSchemaFacts<TabularIslamicSchema, TabularIslamicDataSet>(new TabularIslamicSchema()) with

    let isnot12 m = m <> 12

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Lunar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Lunar
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunarPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYears_Prop() =
        x.SchemaUT.SupportedYears === Range.Create(-199_999, 200_000)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.Create(-199_999, 200_000)

    [<Fact>]
    member x.``CountDaysInMonth() may overflow when y = Int32.MinValue``() =
        let sch = x.SchemaUT
        let countDaysInMonth m = sch.CountDaysInMonth(Int32.MinValue, m)
        let maxMonth = x.MaxMonth

        (fun () -> countDaysInMonth 12) |> overflows
        // No overflow if m != 12.
        [1 .. maxMonth] |> List.filter isnot12 |> List.map countDaysInMonth

    [<Fact>]
    member x.``CountDaysInMonth() may overflow when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        let countDaysInMonth m = sch.CountDaysInMonth(Int32.MaxValue, m)
        let maxMonth = x.MaxMonth

        (fun () -> countDaysInMonth 12) |> overflows
        // No overflow if m != 12.
        [1 .. maxMonth] |> List.filter isnot12 |> List.map countDaysInMonth

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<TabularIslamicSchema>()

[<Sealed>]
type TropicaliaTests() =
    inherit CalendricalSchemaFacts<TropicaliaSchema, TropicaliaDataSet>(new TropicaliaSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<TropicaliaSchema>()

[<Sealed>]
type Tropicalia3031Tests() =
    inherit CalendricalSchemaFacts<Tropicalia3031Schema, Tropicalia3031DataSet>(new Tropicalia3031Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<Tropicalia3031Schema>()

[<Sealed>]
type Tropicalia3130Tests() =
    inherit CalendricalSchemaFacts<Tropicalia3130Schema, Tropicalia3130DataSet>(new Tropicalia3130Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<Tropicalia3130Schema>()

[<Sealed>]
type WorldTests() =
    inherit CalendricalSchemaFacts<WorldSchema, WorldDataSet>(new WorldSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    [<Fact>]
    static member CreateInstance() = SchemaActivatorFacts.Test<WorldSchema>()

    //
    // Featurettes
    //

    [<Theory; MemberData(nameof(WorldTests.DateInfoData))>]
    member x.``IsBlankDay()`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        // Act & Assert
        x.SchemaUT.IsBlankDay(y, m, d) === info.IsSupplementary
