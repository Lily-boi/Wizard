using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Wizard.WizardCode.CardPiles;

public class SpellCardPile() : CustomPile(SpellPileType)
{
    [CustomEnum] public static PileType SpellPileType;
    
    public override bool CardShouldBeVisible(CardModel card) => true;
    public override bool NeedsCustomTransitionVisual => true;
    public static Vector2 ButtonPosition = new Vector2(15f, 785);

    public override Vector2 GetTargetPosition(CardModel model, Vector2 size)
    {
        return ButtonPosition;
    }
    
    
}