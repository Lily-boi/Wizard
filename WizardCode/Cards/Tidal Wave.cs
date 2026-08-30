using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;


public class Tidal_Wave() : WizardCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool GainsBlock => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[]
        {
            HoverTipFactory.FromKeyword(WizardKeywords.Etch),
            HoverTipFactory.FromKeyword(WizardKeywords.Bountiful)
        };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        WizardKeywords.Bountiful
    };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new BlockVar(6M, ValueProp.Move) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Tidal_Wave card = this;
        await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block.BaseValue, ValueProp.Move, cardPlay);
        await EtchCmd.Etch(card);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}