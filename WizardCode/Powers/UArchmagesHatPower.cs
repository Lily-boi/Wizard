using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Cards;

public sealed class UArchmagesHatPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        var power = this;
        if (!participants.Contains(power.Owner)) return;

        power.Flash();
        for (int i = 0; i < power.Amount; i++)
        {
            var hand = PileType.Hand.GetPile(power.Owner.Player).Cards
                .Where(c => !c.EnergyCost.CostsX && c.EnergyCost.GetWithModifiers(CostModifiers.Local) > 0)
                .ToList();
            if (hand.Count == 0) return;
            power.Owner.Player.RunState.Rng.CombatCardSelection.NextItem(hand)?.EnergyCost.AddUntilPlayed(-2);
        }
    }
}
