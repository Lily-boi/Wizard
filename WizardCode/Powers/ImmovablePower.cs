using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Wizard.WizardCode.Powers;

public sealed class ImmovablePower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterBlockGained(
        Creature creature,
        decimal amount,
        ValueProp props,
        CardModel? cardSource)
    {
        if (creature != Owner || amount <= 0M)
            return;

        Creature[] enemies = Owner.CombatState.HittableEnemies.ToArray();
        if (enemies.Length == 0)
            return;

        Creature? target = Owner.Player.RunState.Rng.CombatTargets.NextItem(enemies);
        if (target == null)
            return;

        Flash();
        await PowerCmd.Apply<WeakPower>(
            new ThrowingPlayerChoiceContext(),
            target,
            Amount,
            Owner,
            null);
    }
}
