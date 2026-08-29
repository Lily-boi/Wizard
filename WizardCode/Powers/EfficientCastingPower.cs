using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Wizard.WizardCode.Commands;

namespace Wizard.WizardCode.Powers;

public sealed class EfficientCastingPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        var power = this;
        if (!participants.Contains(power.Owner)) return;
        power.Flash();
        await CastCmd.CastMultiple(choiceContext, power.Owner.Player, power.Amount);
    }
}