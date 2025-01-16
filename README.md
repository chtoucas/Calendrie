﻿
An experimental calendar library with a focus on calendrical calculations and
extensibility.

_This project is heavily inspired by NodaTime and JodaTime._

Compared to NodaTime and the BCL, Calendrie is missing many important features
(parsing, formatting, time-related classes, etc.), therefore there
is very little reason to use this library instead of them; it's really just a
hobby project.

Current status:
- Alpha: unstable API
- Well tested
- Results regarding performance are rather promising
- Targets .NET 8.0+

Features
--------

- Dedicated date, month and year types _per_ calendar
- Custom math
- User-defined calendars

Supported Calendars
-------------------

### Proleptic calendars

- Gregorian
- Julian

### Retropolated calendars

- Armenian (*)
- Civil
- Coptic (*)
- Egyptian (*)
- Ethiopic (*)
- French Republican (*)
- International Fixed
- Pax (**)
- Persian
- Tabular Islamic
- Tropicalia
- World aka Universal
- Zoroastrian (*)

(*) Calendar available in two forms: 12 months or 12 months plus a virtual
thirteenth month.
(**) Tropicalia is like the Gregorian calendar but with a much better rule to
identify leap years.
