
An experimental calendar library with a focus on calendrical calculations and
extensibility.

_This project is heavily inspired by NodaTime and JodaTime._

Compared to NodaTime and the BCL, Calendrie is missing many important features
(parsing, formatting, time-related classes, etc.), therefore there
is very little reason to use this library instead of them; it's really just a
hobby project.

Current status
--------------

- Alpha: unstable API
- Well tested
- Results regarding performance are rather promising
- Targets .NET 8.0+

Features
--------

- Dedicated date, month and year types _per_ calendar
- Custom math
- User-defined calendars

### Supported Calendars

#### Proleptic calendars

These calendars support dates in the range from 999.999 B.C.E. to 999.999 C.E.
of years.
- Gregorian calendar
- Julian calendar

#### Retropolated calendars

These calendars support dates in the range from 1 to 9999 of years.
- Armenian calendar (*)
- Civil calendar
- Coptic calendar (*)
- Egyptian calendar (*)
- Ethiopic calendar (*)
- French republican calendar (*)
- International fixed calendar
- Pax calendar
- Persian calendar
- Tabular islamic calendar
- Tropicalia calendar (**)
- World calendar aka universal calendar
- Zoroastrian calendar (*)

(*) Calendar available in two forms: 12 months or 12 months plus a virtual
thirteenth month.

(**) The tropicalia calendar is like the gregorian calendar but with a much
better rule to identify leap years.
