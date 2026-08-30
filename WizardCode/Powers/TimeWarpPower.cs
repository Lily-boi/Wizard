using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Wizard.WizardCode.Powers;

public sealed class TimeWarpPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool ShouldTakeExtraTurn(Player player) =>
        Amount > 0 && player == Owner.Player;

    public override async Task AfterTakingExtraTurn(Player player)
    {
        if (player != Owner.Player)
            return;

        await PowerCmd.Decrement(this);
    }
}
