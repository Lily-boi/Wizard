using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System.Linq;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Cards;

namespace Wizard.WizardCode.Commands;

public static class EtchCmd
{
    public static async Task<bool> Etch(CardModel card)
    {
        if (card is IComplexCard) return false;
        await CardPileCmd.Add(card, SpellCardPile.SpellPileType, CardPilePosition.Random);
        return true;
    }

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

    public static async Task<CardModel?> EtchTopOfDrawPile(PlayerChoiceContext choiceContext, Player owner)
    {
        await CardPileCmd.ShuffleIfNecessary(choiceContext, owner);
        var card = PileType.Draw.GetPile(owner).Cards.FirstOrDefault();
        if (card == null) return null;
        await Etch(card);
        return card;
    }

    public static async Task<IReadOnlyList<CardModel>> EtchChosenFromHand(
        PlayerChoiceContext choiceContext, Player owner, AbstractModel source, int max)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(EtchSelectorPrefs.ChosenFromHandPrompt, minCount: 0, maxCount: max);

        var chosen = await CardSelectCmd.FromHand(choiceContext, owner, prefs,
            c => c is not IComplexCard, source);

        var etched = new List<CardModel>();
        foreach (CardModel card in chosen)
        {
            if (await Etch(card))
                etched.Add(card);
        }
        return etched;
    }

    // "Etch 1 card from your Discard pile" — player choice, per Graveblast/Dredge's
    // CardSelectCmd.FromCombatPile pattern. Reuses the same prompt as hand selection,
    // since the loc text ("Choose a card to Etch") isn't hand-specific.
    public static async Task<CardModel?> EtchChosenFromDiscard(PlayerChoiceContext choiceContext, Player owner)
    {
        var prefs = new CardSelectorPrefs(EtchSelectorPrefs.ChosenFromHandPrompt, 1);
        var chosen = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(owner), owner, prefs);
        var card = chosen.FirstOrDefault();
        if (card == null) return null;
        return await Etch(card) ? card : null;
    }

    // Kept for any future "Etch a card from your Discard pile at random" design —
    // not currently used by any built card.
    public static async Task<CardModel?> EtchRandomFromDiscard(Player owner)
    {
        var pile = PileType.Discard.GetPile(owner);
        var eligible = pile.Cards.Where(c => c is not IComplexCard).ToList();
        if (eligible.Count == 0) return null;
        var card = owner.RunState.Rng.CombatCardSelection.NextItem(eligible);
        await Etch(card);
        return card;
    }

    public static async Task<CardModel> EtchCopy(CardModel source)
    {
        var copy = source.CreateClone();
        await CardPileCmd.AddGeneratedCardToCombat(copy, SpellCardPile.SpellPileType, source.Owner, CardPilePosition.Random);
        return copy;
    }

    public static async Task<CardModel> EtchNewCopy<T>(Player owner) where T : CardModel
    {
        var newCard = owner.Creature.CombatState.CreateCard<T>(owner);
        await CardPileCmd.AddGeneratedCardToCombat(newCard, SpellCardPile.SpellPileType, owner, CardPilePosition.Random);
        return newCard;
    }
}