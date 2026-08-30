using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Relics;

public class Archmage_s_Book() : WizardRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(WizardKeywords.Etch)
    ];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;

        CardSelectorPrefs prefs = new(SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromCombatPile(
                null,
                PileType.Draw.GetPile(Owner),
                Owner,
                prefs))
            .FirstOrDefault();

        if (card == null)
            return;

        Flash();
        if (await EtchCmd.Etch(card))
        {
            await CardPileCmd.Draw(
                new ThrowingPlayerChoiceContext(),
                DynamicVars.Cards.BaseValue,
                Owner);
        }
    }
}
