using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Commands;

namespace Wizard.WizardCode.Powers;

public sealed class MagicalResiduePower :
    WizardPower,
    IAfterWizardCardEtchedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task AfterWizardCardEtched(CardModel card)
    {
        if (card.Owner != Owner.Player || Amount <= 0)
            return;

        Flash();
        await CardPileCmd.Draw(
            new ThrowingPlayerChoiceContext(),
            Amount,
            Owner.Player);
    }
}
