
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

### Supported Calendars

#### Arithmetical calendars
- Armenian (*)
- Civil
- Coptic (*)
- Ethiopic (*)
- Gregorian (proleptic)
- Julian (proleptic)
- Zoroastrian (*)

(*) Calendar available in two forms: 12 months or 12 months plus a virtual
thirteenth month.

#### Arithmetisation of astronomical calendars
- Tabular Islamic

#### Perennial blank-day calendars
- World aka Universal
