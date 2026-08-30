using MegaCrit.Sts2.Core.Models;

namespace Wizard.WizardCode.Commands;

/// <summary>
/// Implemented by Wizard powers that react after a card is successfully Etched.
/// EtchCmd is the single dispatcher so the mechanic also works for generated spells.
/// </summary>
public interface IAfterWizardCardEtchedPower
{
    Task AfterWizardCardEtched(CardModel card);
}

public static class WizardPowerHooks
{
    public static async Task AfterCardEtched(CardModel card)
    {
        IAfterWizardCardEtchedPower[] listeners = card.Owner.Creature.Powers
            .OfType<IAfterWizardCardEtchedPower>()
            .ToArray();

        foreach (IAfterWizardCardEtchedPower listener in listeners)
            await listener.AfterWizardCardEtched(card);
    }
}
