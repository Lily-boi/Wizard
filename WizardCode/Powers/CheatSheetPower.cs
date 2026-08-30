using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Commands;

namespace Wizard.WizardCode.Powers;

public sealed class CheatSheetPower :
    WizardPower,
    IAfterWizardCardEtchedPower
{
    private int _lastTurnNumber = -1;
    private bool _usedThisTurn;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public async Task AfterWizardCardEtched(CardModel card)
    {
        if (card.Owner != Owner.Player)
            return;

        int turnNumber = Owner.Player.PlayerCombatState.TurnNumber;
        if (_lastTurnNumber != turnNumber)
        {
            _lastTurnNumber = turnNumber;
            _usedThisTurn = false;
        }

        if (_usedThisTurn)
            return;

        // Mark this first so Etching the bonus copy cannot recursively copy itself.
        _usedThisTurn = true;
        Flash();
        await EtchCmd.EtchCopy(card, forceExhaust: true);
    }
}
