using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.CardPiles;

namespace Wizard.WizardCode.Powers;

public sealed class FireBodyPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;

        int spellCount = SpellCardPile.SpellPileType
            .GetPile(Owner.Player)
            .Cards.Count;
        decimal damage = spellCount * Amount;
        if (damage <= 0M)
            return;

        Flash();
        await CreatureCmd.Damage(
            choiceContext,
            Owner.CombatState.HittableEnemies,
            damage,
            ValueProp.Unpowered,
            Owner);
    }
}
