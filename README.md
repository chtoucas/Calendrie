
An experimental calendar library with a focus on calendrical calculations and
extensibility.

_This project is heavily inspired by NodaTime, JodaTime and the BCL._

Compared to NodaTime and the BCL, Calendrie is missing many important features
(parsing, formatting, time-related types, etc.), therefore there is very little
reason to use this library instead of them; it's really just a hobby project.
Nevertheless, there are features in this project not found elsewhere.

Current status:
- Alpha: unstable API
- Well tested
- Results regarding performance are very promising
- Targets .NET 8.0+

Features
--------

- _Dedicated date, month and year types **per** calendar_
- Optional custom math for the date and month types
- User-defined calendars
- Most calendrical calculations are obtained using algorithms based on the
  discrete geometry of calendars

### Pre-defined Calendars

#### Proleptic calendars

These calendars support dates in the range from 999.999 B.C.E. to 999.999 C.E.
of years.
- Gregorian calendar
- Julian calendar

#### Retropolated calendars

These calendars support dates in the range from 1 to 9999 of years.
- Armenian calendar (*)
- Civil calendar, same as the Gregorian calendar above but with a different range
  of supported years
- Coptic calendar (*)
- Egyptian calendar (*)
- Ethiopic calendar (*)
- French Republican calendar (*), an arithmetisation of the astronomical calendar
- International Fixed calendar
- Pax calendar
- Persian calendar, an arithmetisation of the astronomical calendar
- Positivist calendar
- Tabular Islamic calendar, an arithmetisation of the astronomical calendar
- Tropicalia calendar (**)
- World calendar aka Universal calendar
- Zoroastrian calendar (*)

(*) Calendar available in two forms: 12 months or 12 months plus a virtual
thirteenth month.

(**) The Tropicalia calendar is like the Gregorian calendar but with a much
better rule to identify leap years.
