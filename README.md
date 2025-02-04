
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

We say that a calendar is "retropolated" if it's extended backward to dates
before its official introduction, supporting all dates from its epoch, the origin
of the underlying era.

A proleptic calendar is extended even further, supporting dates not only before
its first adoption but also before its epoch.

#### Proleptic calendars

These calendars support dates in the range from 999.999 B.C.E. to 999.999 C.E.
of years.
- Gregorian calendar
- Julian calendar

#### Retropolated calendars

These calendars support dates in the range from 1 to 9999 of years.
- Armenian
- Civil, same as the Gregorian calendar above but with a different range
  of supported years
- Coptic
- Egyptian
- Ethiopic
- French Republican, an arithmetisation of the astronomical calendar
- International Fixed
- Pax
- Persian, an arithmetisation of the astronomical calendar
- Positivist
- Tabular Islamic, an arithmetisation of the astronomical calendar
- Tropicalia
- World aka Universal
- Zoroastrian

