// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

// TODO(code): value names, règles et roundoff (voir Developer Notes)
// Quelle valeur prendre pour roundoff (pas toujours cohérent, voir les
// calendriers non-réguliers). Incohérences aussi entre AddYears() et AddMonths().
// DateMath & co: XML doc. Explain comparison with DateDifference
// Add a warning about the data (CountYearsBetweenData & co) which only
// offer symmetrical results; see DefaultDateMathFacts and DefaultMonthMathFacts.

#region Developer Notes

// Ajouter des années
// ------------------
//
// Règle : quand une addition naïve donnerait un résultat invalide, on
// sélectionne le dernier jour valide précédent.
// Cette règle est simple à retenir mais ne donne pas nécessairement un résultat
// très naturel, tout particulièrement quand le calendrier n'est pas régulier
// (Pax) ou dans les variantes à 13 mois des calendriers réguliers.
//
// ## Calendrier grégorien régulier
// Seul le jour intercalaire d'une année bissextile pose problème.
// 29/02 +/- 1 année = 28/02 ; roundoff = 1
//
// ## Calendrier copte régulier (12 mois)
// Seul le jour intercalaire d'une année bissextile pose problème.
// 36/12 +/- 1 année = 35/12 ; roundoff = 1
//
// ## Calendrier copte régulier (13 mois)
// Seul le jour intercalaire d'une année bissextile pose problème.
// 06/13 +/- 1 année = 05/13 ; roundoff = 1
//
// ## Calendrier pax __très__ régulier (13 mois)
// Année bissextile vers année ordinaire :
// 01/13 +/- 1 année = 01/13
// 07/13 +/- 1 année = 07/13
// 01/14 +/- 1 année = 28/13 ; roundoff = 1     -20 ? 08/13 (0) - 08/13 (exact)
// 21/14 +/- 1 année = 28/13 ; roundoff = 21      0 ? 28/13 (0) - 28/13 (exact)
// 22/14 +/- 1 année = 28/13 ; roundoff = 22      1 ? 28/13 (1) - 01/14 (exact)
// 28/14 +/- 1 année = 28/13 ; roundoff = 28      7 ? 28/13 (7) - 07/14 (exact)
//
// Année ordinaire vers année bissextile :
// 01/13 +/- 1 année = 01/13
// 07/13 +/- 1 année = 07/13
// 08/13 +/- 1 année = 07/13 ; roundoff = 1
// 28/13 +/- 1 année = 07/13 ; roundoff = 21
//
// Ajouter des mois
// ----------------
//
// Exemples (calendriers grégorien et julien) quand le résultat d'une opération
// arithmétique n'est pas exact (roundoff > 0).
//
// AdditionRule.Truncate : dernier jour du mois cible.
// * 31/5/2016 + 1 mois = 30/6/2016 (le 31/6/2016 n'existe pas)
// * 31/5/2016 - 1 mois = 30/4/2016 (le 31/4/2016 n'existe pas)
// * 29/2/2016 + 1 an   = 28/2/2017 (le 29/2/2017 n'existe pas)
// * 29/2/2016 - 1 an   = 28/2/2015 (le 29/2/2015 n'existe pas)
// Exemples avec un décalage supérieur à 1 jour :
// * 31/5/2015 + 9 mois = 28/2/2016 (le 31/2/2016 n'existe pas)
// * 30/5/2015 + 9 mois = 28/2/2016 (le 30/2/2016 n'existe pas)
// * 31/5/2015 - 3 mois = 28/2/2015 (le 31/2/2016 n'existe pas)
// * 30/5/2015 - 3 mois = 28/2/2015 (le 30/2/2016 n'existe pas)
//
// AdditionRule.Overspill : premier jour du mois suivant.
// * 31/5/2016 + 1 mois = 1/7/2016
// * 31/5/2016 - 1 mois = 1/5/2016
// * 29/2/2016 + 1 an   = 1/3/2017
// * 29/2/2016 - 1 an   = 1/3/2015
// Exemples avec un décalage supérieur à 1 jour :
// * 31/5/2015 + 9 mois = 1/3/2016
// * 30/5/2015 + 9 mois = 1/3/2016
// * 31/5/2015 - 3 mois = 1/3/2015
// * 30/5/2015 - 3 mois = 1/3/2015
//
// AdditionRule.Exact : on décale d'autant de jours que nécessaire pour supprimer
// l'arrondi.
// * 31/5/2016 + 1 mois = 1/7/2016
// * 31/5/2016 - 1 mois = 1/5/2016
// * 29/2/2016 + 1 an   = 1/3/2017
// * 29/2/2016 - 1 an   = 1/3/2015
// Exemples avec un décalage supérieur à 1 jour :
// * 31/5/2015 + 9 mois = 3/3/2016 (décalage = +3)
// * 30/5/2015 + 9 mois = 2/3/2016 (décalage = +2)
// * 31/5/2015 - 3 mois = 3/3/2015 (décalage = +3)
// * 30/5/2015 - 3 mois = 2/3/2015 (décalage = +2)
//
// Notes
// -----
//
// Initialement, on proposait aussi une valeur Overflow, mais je pense que c'est
// une mauvaise idée car cela reviendrait à utiliser une exception pour une
// situation non-exceptionnelle.

#endregion

/// <summary>
/// Specifies the strategy to resolve ambiguities when adding a number of months
/// or years to a date, or a number of years to a month.
/// </summary>
public enum AdditionRule
{
    /// <summary>
    /// When the result is not a valid day (resp. month), return the end of the
    /// target month (resp. year).
    /// <para>This is the <i>default</i> strategy.</para>
    /// </summary>
    Truncate = 0,

    /// <summary>
    /// When the result is not a valid day (resp. month), return the start of
    /// the next month (resp. year).
    /// </summary>
    Overspill,

    /// <summary>
    /// When the result is not a valid day (resp. month), return the day (resp.
    /// month) obtained by adding "roundoff" days (resp. months) to the end of
    /// the target month (resp. year).
    /// </summary>
    Exact,
}
