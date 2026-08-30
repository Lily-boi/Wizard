using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;

namespace Wizard.WizardCode.Powers;

public sealed class InvokeFireboltPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner))
            return;

        Flash();
        for (int i = 0; i < Amount; i++)
        {
            await EtchCmd.EtchNewCopy<Firebolt>(
                Owner.Player,
                forceExhaust: true);
        }
    }
}
