// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.DaysInMonthDistributionTestSuite

open Calendrie.Core.Schemas
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core
open Calendrie.Testing.Faux

open Xunit

let private test = IDaysInMonthsFacts.Test

[<Fact>]
let Coptic12Tests () =
    test(new Coptic12Schema(), Coptic12DataSet.CommonYear, Coptic12DataSet.LeapYear)

[<Fact>]
let Coptic13Tests () =
    test(new Coptic13Schema(), Coptic13DataSet.CommonYear, Coptic13DataSet.LeapYear)

[<Fact>]
let Egyptian12Tests () =
    test(new Egyptian12Schema(), Egyptian12DataSet.SampleYear, Egyptian12DataSet.SampleYear)

[<Fact>]
let Egyptian13Tests () =
    test(new Egyptian13Schema(), Egyptian13DataSet.SampleYear, Egyptian13DataSet.SampleYear)

[<Fact>]
let FrenchRepublican12Tests () =
    test(new FrenchRepublican12Schema(), FrenchRepublican12DataSet.CommonYear, FrenchRepublican12DataSet.LeapYear)

[<Fact>]
let FrenchRepublican13Tests () =
    test(new FrenchRepublican13Schema(), FrenchRepublican13DataSet.CommonYear, FrenchRepublican13DataSet.LeapYear)

[<Fact>]
let GregorianTests () =
    test(new GregorianSchema(), GregorianDataSet.CommonYear, GregorianDataSet.LeapYear)

[<Fact>]
let InternationalFixedTests () =
    test(new InternationalFixedSchema(), InternationalFixedDataSet.CommonYear, InternationalFixedDataSet.LeapYear)

[<Fact>]
let JulianTests () =
    test(new JulianSchema(), JulianDataSet.CommonYear, JulianDataSet.LeapYear)

[<Fact>]
let FauxLunisolarTests () =
    test(new FauxLunisolarSchema(), FauxLunisolarDataSet.CommonYear, FauxLunisolarDataSet.LeapYear)

[<Fact>]
let PaxTests () =
    test(new PaxSchema(), PaxDataSet.CommonYear, PaxDataSet.LeapYear)

[<Fact>]
let Persian2820Tests () =
    test(new Persian2820Schema(), Persian2820DataSet.CommonYear, Persian2820DataSet.LeapYear)

[<Fact>]
let PositivistTests () =
    test(new PositivistSchema(), PositivistDataSet.CommonYear, PositivistDataSet.LeapYear)

[<Fact>]
let TabularIslamicTests () =
    test(new TabularIslamicSchema(), TabularIslamicDataSet.CommonYear, TabularIslamicDataSet.LeapYear)

[<Fact>]
let TropicaliaTests () =
    test(new TropicaliaSchema(), TropicaliaDataSet.CommonYear, TropicaliaDataSet.LeapYear)

[<Fact>]
let Tropicalia3031Tests () =
    test(new Tropicalia3031Schema(), Tropicalia3031DataSet.CommonYear, Tropicalia3031DataSet.LeapYear)

[<Fact>]
let Tropicalia3130Tests () =
    test(new Tropicalia3130Schema(), Tropicalia3130DataSet.CommonYear, Tropicalia3130DataSet.LeapYear)

[<Fact>]
let WorldTests () =
    test(new WorldSchema(), WorldDataSet.CommonYear, WorldDataSet.LeapYear)
