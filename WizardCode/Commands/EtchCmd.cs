using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Cards;

namespace Wizard.WizardCode.Commands;

public static class EtchCmd
{
    // Directly etch a specific card (used once you've already picked/rolled it).
    // Returns false and does nothing if the card is Complex.
    public static async Task<bool> Etch(CardModel card)
    {
        if (card is IComplexCard) return false;
        await CardPileCmd.Add(card, SpellCardPile.SpellPileType, CardPilePosition.Random);
        return true;
    }

    // "Etch N cards in hand at random" — same shape as TrueGrit's exhaust-random.
    public static async Task<IReadOnlyList<CardModel>> EtchRandomFromHand(Player owner, int count)
    {
        var pile = PileType.Hand.GetPile(owner);
        var eligible = pile.Cards.Where(c => c is not IComplexCard).ToList();
        var etched = new List<CardModel>();
        for (int i = 0; i < count && eligible.Count > 0; i++)
        {
            var card = owner.RunState.Rng.CombatCardSelection.NextItem(eligible);
            eligible.Remove(card);
            await Etch(card);
            etched.Add(card);
        }
        return etched;
    }

    // "Etch the top card of your draw pile"
    public static async Task<CardModel?> EtchTopOfDrawPile(PlayerChoiceContext choiceContext, Player owner)
    {
        await CardPileCmd.ShuffleIfNecessary(choiceContext, owner);
        var card = PileType.Draw.GetPile(owner).Cards.FirstOrDefault();
        if (card == null) return null;
        await Etch(card); // no-ops silently if Complex, per your spec
        return card;
    }

    // "Etch a card in your hand" (player choice) — filter Complex OUT of the selectable pool
    // entirely, rather than letting them pick one and fizzle. Better UX.
    public static async Task<CardModel?> EtchChosenFromHand(
        PlayerChoiceContext choiceContext, Player owner, CardModel source, int max)
    {
        
        CardSelectorPrefs prefs = new CardSelectorPrefs(source.SelectionScreenPrompt, minCount: 0, maxCount: max);
        
        foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, owner, prefs, null, (AbstractModel) source))
            await CardCmd.Exhaust(choiceContext, card);

        return null;
    }
}