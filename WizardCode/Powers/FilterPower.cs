using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Commands;

namespace Wizard.WizardCode.Powers;

public sealed class FilterPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardGeneratedForCombat(
        CardModel card,
        Player? creator)
    {
        if (card.Type != CardType.Status ||
            card.Owner != Owner.Player ||
            card.IsClone)
        {
            return;
        }

        Flash();
        await EtchCmd.Etch(card);

        // Filter+ applies 2 stacks. The old <= check duplicated at 1 stack.
        if (Amount >= 2)
            await EtchCmd.EtchCopy(card);
    }
}
