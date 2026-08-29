using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;

namespace Wizard.WizardCode.Powers;


public sealed class FortifyPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var power = this;
        if (cardPlay.Card.Owner != power.Owner.Player) return;
        if (cardPlay.Card is not WizardCard wc || !wc.WasCast) return;
        await CreatureCmd.GainBlock(power.Owner, (decimal)power.Amount, ValueProp.Unpowered, null);
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        var power = this;
        if (!participants.Contains(power.Owner)) return;
        await PowerCmd.Remove(power);
    }
}