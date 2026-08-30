using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;

namespace Wizard.WizardCode.Cards;

public class Forbidden_Rites() : WizardCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        new DynamicVar[]
        {
            new CardsVar(5),
            new HpLossVar(5)
        };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Forbidden_Rites card = this;
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, card.Owner.Creature, card.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, (CardModel) card, play);
        await CastCmd.CastMultiple(choiceContext, card.Owner, card.DynamicVars.Cards.IntValue);
    }

    protected override void OnUpgrade() => this.DynamicVars.HpLoss.UpgradeValueBy(-2);
}