Calendar reforms
----------------

It is very unlikely for a calendar reform to be adopted (widely, not just by
some enthusiasts) any time soon. Anyway, the Gregorian calendar being the _de
facto_ universal calendar, a reform should not part ways with its fundamental
properties.

### Checklist

MUST
- Solar and arithmetical.
- Twelve months (not thirteen) and a week of seven days.
  * Religion: these two cycles often have holy meanings.
  * Business: 13 months means that we would have to change the way we compute
    salaries, interests, etc. Also, 13 months can not be grouped in quarters.
  * Science: 13 is prime number which would make things unnecesary complicated.
    This is not just a problem in pure science, for instance it would also affect
    navigation systems.
  * History: the continuity of the week cycle (no blank-days) is very important
    for chronologists.
- Keep the month subdivision. Weeks are too short.
- For a reform to be accepted, a reform must not disrupt too much people habits.

SHOULD
- Anniversaries. A calendar reform should stay as close as possible to the
  Gregorian numbering system of months and days.
- Human-friendliness, not too monotonous but not too complex
  * 12 months of identical length would be very disturbing, we lose the sense
    of passage of time.
  * Many perennial calendars are actually pretty boring.
  * Easy to remember leap rules.
  * The distribution of days in months should be simple (mnemonic tools).
- Computer-friendliness.
  * `IsLeapYear()`.
    The "Farey rule" offers a very good approximation for the length of the
    tropical year and is very computer-friendly? See `TropicalSchema`.
  * `CountDaysInYearBeforeMonth()` and `CountDaysInMonth()`.
    Something along the lines of the Tabular Islamic schema but adapted to match
    the length of the tropical year?
  * `GetYear()` and `GetMonthDayParts()`.
    The intercalary day should be positioned at the end of the year.

COULD
- Better approximation for the (current) length of the tropical year.
- Be perennial. If we exclude blank-days, we are left with only one option:
  leap-week calendars. Disavantages: complex leap rules, no general agreement on
  the meaning of "first day of the week", more "leapers".

### Failed reforms in the past

- French Republican: a decimal calendar with 10-day weeks.

Lessons:
- A calendar with weeks longer than 7 days is doomed to fail.

### Proposals

- Bonavian
- Hanke–Henry Permanent (HPPC)
- International Fixed
- Pax
- Positivist
- World
- Symmetry010
- Symmetry454

Month patterns
--------------

Solar calendars (12 months).

Pattern               | Jan. | Feb.    | Mar. | Apr. | May  | June      | July | Aug. | Sep. | Oct. | Nov. | Dec.
--------------------- | ---- | ------- | ---- | ---- | ---- | --------- | ---- | ---- | ---- | ---- | ---- | -----------
Coptic: 30            | 30   | 30      | 30   | 30   | 30   | 30        | 30   | 30   | 30   | 30   | 30   | 30+5 (30+6)
Julian, Gregorian     | 31   | 28 (29) | 31   | 30   | 31   | 30        | 31   | 31   | 30   | 31   | 30   | 31
World: 31-30-30       | 31   | 30      | 30   | 31   | 30   | 30 (31)   | 31   | 30   | 30   | 31   | 30   | 30+1
Positivist: 28        | 28   | 28      | 28   | 28   | 28   | 28 (28+1) | 28   | 28   | 28   | 28   | 28   | 28+1
Fixed: 28             | 28   | 28      | 28   | 28   | 28   | 28        | 28   | 28   | 28   | 28   | 28   | 28+1 (28+2)
Symmetry010: 30-31-30 | 30   | 31      | 30   | 30   | 31   | 30        | 30   | 31   | 30   | 30   | 31   | 30 (37)
Symmetry454: 28-35-28 | 28   | 35      | 28   | 28   | 35   | 28        | 28   | 35   | 28   | 28   | 35   | 28 (35)

_Simple_ alternatives to the Gregorian layout with the leap day at the end of December.
- 365 = 7 * 31 + 4 * 30 + 28; eg pattern A
- 365 = 6 * 31 + 5 * 30 + 29; eg patterns B, 31-30
- 365 = 5 * 31 + 6 * 30 + 30; eg pattern 30-31
- 365 = 4 * 31 + 7 * 30 + 31; eg patterns 31-30-30, 30-31-30
- 365 = 3 * 31 + 8 * 30 + 32; eg pattern 30-30-31

Pattern  | Jan. | Feb. | Mar. | Apr. | May  | June | July | Aug. | Sep. | Oct. | Nov. | Dec. | Lost days | Months changed
-------- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | --------- | --------------
A        | 31   | 28/29| 31   | 30   | 31   | 30   | 31   | 31   | 30   | 31   | 30   | 31   | 0         | 0
B        | 31   | 30   | 31   | 30   | 31   | 30   | 31   | 31   | 30   | 31   | 30   |**29**| 2 (1)     | 2
31-30    | 31   | 30   | 31   | 30   | 31   | 30   | 31   |**30**| 31   |**30**| 31   |**29**| 4 (3)     | 6
30-31    |**30**| 31   |**30**| 31   |**30**| 31   |**30**| 31   | 30   | 31   | 30   |**30**| 5 (4)     | 8
31-30-30 | 31   | 30   |**30**| 31   |**30**| 30   | 31   |**30**| 30   | 31   | 30   | 31   | 3         | 5 (6)
30-31-30 |**30**| 31   |**30**| 30   | 31   | 30   |**30**| 31   | 30   |**30**| 31   | 31   | 4         | 6 (7)
30-30-31 |**30**| 30   | 31   | 30   |**30**| 31   |**30**|**30**| 31   |**30**| 30   | 32   | 5         | 9

With the pattern B, we are very conservative, still we simplify some computations.
- only two months are affected.
- two days (one on leap years) disappear.
- leapers for the Gregorian calendar are no longer leapers, maybe they won't like
  it though.
- fast-track validation, when `d < 30` we don't have to compute the number of
  days in the month, only 17 exceptional days (18 on leap years) --- in the
  Gregorian case, we have 29 (30) exceptional days.
- the four quarters: 92, 91, 92, 90 (91).

With the pattern 31-30
- six months are affected.
- simple rule:
  * odd month  = odd number of days in month (31)
  * even month = even number of days in month (30)
  * December = 29 days (30 days on leap years)
- three days (two on leap years) disappear, among them the December 31st...
  In France, Aug., Oct. and Dec. are (were?) low on births, so it might affect 
  less people than other patterns
  [INED](https://www.ined.fr/fichier/s_rubrique/19823/popf.1_re.gnier_2010.fr.pdf)
- leapers for the Gregorian calendar are no longer leapers, maybe they won't like
  it though.
- fast-track validation, when `d < 30` we don't have to compute the number of
  days in the month, only 17 exceptional days (18 on leap years).
- the four quarters: 92, 91, 92, 90 (91).
- losing one day during the summer might be refused by the industry of tourism.

References
----------

- Paul Couderc, "Le Calendrier", PUF.
- http://myweb.ecu.edu/mccartyr/calendar-reform.html
- http://myweb.ecu.edu/mccartyr/weekdate.html
- https://www.hermetic.ch/cal_stud/palmen/lweek1.htm
