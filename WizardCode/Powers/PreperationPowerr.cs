using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Wizard.WizardCode.Powers;

public sealed class PreparationPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var power = this;
        if (cardPlay.Card.Owner != power.Owner.Player || cardPlay.Card.Type != CardType.Power) return;
        power.Flash();
        await CreatureCmd.GainBlock(power.Owner, (decimal)power.Amount, ValueProp.Unpowered, null);
    }
}