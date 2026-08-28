using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Wizard.WizardCode.Cards;

public class Fizzle() : WizardCard(1, CardType.Status, CardRarity.Status, TargetType.None)
{
    public override int MaxUpgradeLevel => 0;

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] { CardKeyword.Exhaust };
}