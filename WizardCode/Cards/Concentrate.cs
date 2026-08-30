using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public class Concentrate() : WizardCard(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{    
    public override bool GainsBlock => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch), HoverTipFactory.FromKeyword(WizardKeywords.Cast) };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new BlockVar(7M, ValueProp.Move) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Concentrate card = this;
        await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block, cardPlay);
        await EtchCmd.EtchChosenFromHand(choiceContext, card.Owner, card, 1);
        await CastCmd.CastTopOfSpellPile(choiceContext, card.Owner);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(2M);
}