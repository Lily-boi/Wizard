using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Commands;

namespace Wizard.WizardCode.Powers;

public sealed class LivingGrimoirePower :
    WizardPower,
    IAfterWizardCardEtchedPower
{
    private int _lastTurnNumber = -1;
    private int _drawsThisTurn;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task AfterWizardCardEtched(CardModel card)
    {
        if (card.Owner != Owner.Player)
            return;

        int turnNumber = Owner.Player.PlayerCombatState.TurnNumber;
        if (_lastTurnNumber != turnNumber)
        {
            _lastTurnNumber = turnNumber;
            _drawsThisTurn = 0;
        }

        if (_drawsThisTurn >= Amount)
            return;

        _drawsThisTurn++;
        Flash();
        await CardPileCmd.Draw(
            new ThrowingPlayerChoiceContext(),
            1M,
            Owner.Player);
    }
}
