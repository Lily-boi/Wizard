using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Wizard.WizardCode.CardPiles;

public class SpellCardPile() : CustomPile(SpellPileType)
{
    [CustomEnum] public static PileType SpellPileType;
    
    public override bool CardShouldBeVisible(CardModel card) => true;
    public override bool NeedsCustomTransitionVisual => true;
    public static Vector2 ButtonPosition = new Vector2(23, 785);

    public override Vector2 GetTargetPosition(CardModel model, Vector2 size)
    {
        var buttonSize = new Vector2(80, 80);
        return ButtonPosition + buttonSize / 2f;
    }
}
public struct EtchSelectorPrefs
{
    public static LocString ChosenFromHandPrompt => new LocString("card_selection", "TO_ETCH");
}