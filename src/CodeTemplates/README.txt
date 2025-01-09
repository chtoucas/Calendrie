
Calendrie.TextTemplating.xx.x.dll
Voir le projet Calendrie.TextTemplating

Initialisation d'un nouveau calendrier
--------------------------------------

On suppose que ce calendrier utilise la plage d'année standard (1 à 9999) et
une epoch différente de DayZero.NewStyle.

1. Décharger le projet

2. Créer le modèle T4, p.ex. Préfixe.tt avec des valeurs factices pour
   "epochDaysSinceZeroValue" et "maxDaysSinceEpochValue" :

<#= new CalendarTemplate(this, "MonSchema", "MonEpoch", "0", "0").Execute() #>

3. Rajouter le modèle T4 au projet :

<Compile Update="Systems\Préfixe.g.cs">
    <DesignTime>True</DesignTime>
    <AutoGen>True</AutoGen>
    <DependentUpon>Préfixe.tt</DependentUpon>
</Compile>

4. Créer un fichier de tests :

module Prelude =
    [<Fact>]
    let ``Value of PréfixeCalendar.Epoch.DaysZinceZero`` () =
        PréfixeCalendar.Instance.Epoch.DaysSinceZero === 0

    [<Fact>]
    let ``default(PréfixeDate) is PréfixeCalendar.Epoch`` () =
        Unchecked.defaultof<PréfixeDate>.DayNumber === PréfixeCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of PréfixeCalendar.MinDaysSinceEpoch`` () =
        PréfixeCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of PréfixeCalendar.MaxDaysSinceEpoch`` () =
        PréfixeCalendar.Instance.MaxDaysSinceEpoch === 0
#endif

Le premier test ainsi que le quatrième vont échouer mais ils vous donneront les
bonnes valeurs à utiliser dans le modèle T4, p.ex. 397 et 3_652_060. Mettre
alors à jour le modèle comme suit :

<#= new CalendarTemplate(this, "MonSchema", "MonEpoch", "397", "3_652_060").Execute() #>

Avertissements :

On suppose que le "scope" de ScopeClass (voir plus bas) définit deux constantes
MinYear et MaxYear.

Le constructeur suppose que le calendrier est régulier, auquel cas ne pas oublier
de préciser la valeur de la propriété T4 MonthsInYear. On suppose bien entendu
que le schéma sous-jacent définit une constante MonthsInYear ayant la même valeur (!).

Pour les calendriers non-réguliers, utiliser l'option "regular: false" du
constructeur T4. Il conviendra de préciser la valeur de la propriété T4
MaxMonthsSinceEpochValue. Un calendrier non-régulier devrait définir son propre
"pré-validateur".

Options disponibles
-------------------

- DisplayName : permet d'utiliser un préfixe différent du nom du modèle.
  Valeur par défaut = nom du modèle.

- ScopeClass : pour utiliser un "scope" différent de StandarScope. Attention :
  les modèles T4 sont prévus pour des "scope" pour lesquels MinYear = 1,
  garantissant ainsi que MinDaysSinceEpoch = 0. On suppose aussi que le "scope"
  définit deux constantes MinYear (= 1) et MaxYear ; nécessaire pour que
  l'arithmétique fonctionne.

- EnableIsSupplementary : quand cette propriété retourne "false", la propriété
  IsSupplementary est rendue comme suit :
  > bool IDateable.IsSupplementary => false;
  Valeur par défaut = true

Pour les calendriers produits en dehors des deux projets principaux, les deux
options suivantes peuvent se révéler être utiles.

- EnableNullable : quand cette propriété retourne "true", rajoute la ligne
  suivante
  > #nullable enable
  Valeur par défaut = false

- ImportCalendrieSystemsNamespace : quand cette propriété retourne "true", on
  importe les espaces de noms "System" nécessaires et suffisants.
  Valeur par défaut = false

Remarques
---------

Pour les calendriers dont l'epoch est DayZero.NewStyle, on utilisera plutôt le
modèle "ZeroCalendarTemplate" :

<#= new ZeroCalendarTemplate(this, "MonSchema", "3_652_060").Execute() #>

Les modèles T4 disponibles ne sont pas adaptés aux calendriers "proleptiques".
La plupart des méthodes doivent être désactivées. Voir Gregorian.tt ou Julian.tt.
