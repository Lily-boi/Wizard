using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Wizard.WizardCode.CardPiles;

public class SpellCardPile() : CustomPile(SpellPileType)
{
    [CustomEnum] public static PileType SpellPileType;

    // Not visible like exhaust — see note below about whether you want this true instead.
    public override bool CardShouldBeVisible(CardModel card) => true;
    public override bool NeedsCustomTransitionVisual => true;

    public override Vector2 GetTargetPosition(CardModel model, Vector2 size)
    {
        return new Vector2(75, 765); // placeholder position
    }
}