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

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        var power = this;
        if (card.Type != CardType.Status || card.Owner != power.Owner.Player || card.IsClone) return;
        power.Flash();
        await EtchCmd.Etch(card);
        if (power.Amount >= 2)
            await EtchCmd.EtchCopy(card);
    }
}