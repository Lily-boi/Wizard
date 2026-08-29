using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;

namespace Wizard.WizardCode.Powers;



public sealed class EtchForceFieldNextTurnPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        EtchForceFieldNextTurnPower power = this;
        if (!participants.Contains(power.Owner)) return;

        power.Flash();

        await EtchCmd.EtchNewCopy<Force_Field>(power.Owner.Player);

        await PowerCmd.Remove(power);
    }
}