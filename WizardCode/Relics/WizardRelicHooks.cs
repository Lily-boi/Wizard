using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Wizard.WizardCode.Relics;

public interface IAfterWizardCardCastRelic
{
    Task AfterWizardCardCast(PlayerChoiceContext choiceContext, CardModel card);
}

public interface IAfterWizardCardEtchedRelic
{
    Task AfterWizardCardEtched(CardModel card);
}

public interface IAfterWizardSpellCreatedRelic
{
    Task AfterWizardSpellCreated(CardModel card);
}

public static class WizardRelicHooks
{
    public static async Task AfterCardCast(
        PlayerChoiceContext choiceContext,
        CardModel card)
    {
        foreach (IAfterWizardCardCastRelic relic in
                 card.Owner.Relics.OfType<IAfterWizardCardCastRelic>().ToArray())
        {
            await relic.AfterWizardCardCast(choiceContext, card);
        }
    }

    public static async Task AfterCardEtched(CardModel card)
    {
        foreach (IAfterWizardCardEtchedRelic relic in
                 card.Owner.Relics.OfType<IAfterWizardCardEtchedRelic>().ToArray())
        {
            await relic.AfterWizardCardEtched(card);
        }
    }

    public static async Task AfterSpellCreated(CardModel card)
    {
        foreach (IAfterWizardSpellCreatedRelic relic in
                 card.Owner.Relics.OfType<IAfterWizardSpellCreatedRelic>().ToArray())
        {
            await relic.AfterWizardSpellCreated(card);
        }
    }
}
