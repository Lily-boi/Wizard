using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;


public class Force_Field() : WizardCard(0, CardType.Skill,
    CardRarity.Status, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new BlockVar(4m, ValueProp.Move) };
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] { WizardKeywords.Bountiful, CardKeyword.Exhaust };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Force_Field card = this;
        await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block, play);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(2m);
}