using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;

namespace Wizard.WizardCode.Cards;


public class Whirlpool() : WizardCard(1, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    public override bool GainsBlock => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        new DynamicVar[]
        {
            new BlockVar(12M, ValueProp.Move),
            new PowerVar<VigorPower>(8)            
        };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Whirlpool card = this;
        await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block.BaseValue, ValueProp.Move, play);
        VigorPower vigorPower = await PowerCmd.Apply<VigorPower>(choiceContext, card.Owner.Creature, (Decimal) card.DynamicVars["VigorPower"].IntValue, card.Owner.Creature, (CardModel) card);

    }

    protected override void OnUpgrade() => this.DynamicVars["VigorPower"].UpgradeValueBy(3M);
}