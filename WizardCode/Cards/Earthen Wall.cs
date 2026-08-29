using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Wizard.WizardCode.Cards;

public class Earthen_Wall() : WizardCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[]
        {
            new BlockVar(7M, ValueProp.Move),
            new DynamicVar("CastBlock", 5M)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Earthen_Wall card = this;
        decimal amount = card.DynamicVars.Block.BaseValue + (card.WasCast ? card.DynamicVars["CastBlock"].BaseValue : 0M);
        await CreatureCmd.GainBlock(card.Owner.Creature, amount, ValueProp.Move, cardPlay);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Block.UpgradeValueBy(2M);
        this.DynamicVars["CastBlock"].UpgradeValueBy(2M);
    }
}