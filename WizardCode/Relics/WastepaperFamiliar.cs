using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Relics;

public class WastepaperFamiliar() : WizardRelic, IAfterWizardCardCastRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(4M, ValueProp.Unpowered),
        new CardsVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromKeyword(WizardKeywords.Cast)
    ];

    public async Task AfterWizardCardCast(
        PlayerChoiceContext choiceContext,
        CardModel card)
    {
        if (card.Owner != Owner || card is not Fizzle)
            return;

        Flash();
        await CreatureCmd.GainBlock(
            Owner.Creature,
            DynamicVars.Block,
            cardPlay: null);
        await CardPileCmd.Draw(
            choiceContext,
            DynamicVars.Cards.BaseValue,
            Owner);
    }
}
