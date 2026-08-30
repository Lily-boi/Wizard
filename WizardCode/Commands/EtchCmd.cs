using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Relics;

namespace Wizard.WizardCode.Commands;

public static class EtchCmd
{
    public static async Task<bool> Etch(CardModel card)
    {
        if (card is IComplexCard)
            return false;

        await CardPileCmd.Add(
            card,
            SpellCardPile.SpellPileType,
            CardPilePosition.Random);

        await WizardRelicHooks.AfterCardEtched(card);
        await WizardPowerHooks.AfterCardEtched(card);
        return true;
    }

    public static async Task<IReadOnlyList<CardModel>> EtchRandomFromHand(
        Player owner,
        int count)
    {
        CardPile pile = PileType.Hand.GetPile(owner);
        List<CardModel> eligible = pile.Cards
            .Where(card => card is not IComplexCard)
            .ToList();
        List<CardModel> etched = new();

        for (int i = 0; i < count && eligible.Count > 0; i++)
        {
            CardModel card = owner.RunState.Rng.CombatCardSelection.NextItem(eligible);
            eligible.Remove(card);

            if (await Etch(card))
                etched.Add(card);
        }

        return etched;
    }

    public static async Task<CardModel?> EtchTopOfDrawPile(
        PlayerChoiceContext choiceContext,
        Player owner)
    {
        await CardPileCmd.ShuffleIfNecessary(choiceContext, owner);
        CardModel? card = PileType.Draw.GetPile(owner).Cards.FirstOrDefault();
        if (card == null)
            return null;

        return await Etch(card) ? card : null;
    }

    public static async Task<IReadOnlyList<CardModel>> EtchChosenFromHand(
        PlayerChoiceContext choiceContext,
        Player owner,
        AbstractModel source,
        int max)
    {
        CardSelectorPrefs prefs = new(
            EtchSelectorPrefs.ChosenFromHandPrompt,
            minCount: 0,
            maxCount: max);

        IEnumerable<CardModel> chosen = await CardSelectCmd.FromHand(
            choiceContext,
            owner,
            prefs,
            card => card is not IComplexCard,
            source);

        List<CardModel> etched = new();
        foreach (CardModel card in chosen)
        {
            if (await Etch(card))
                etched.Add(card);
        }

        return etched;
    }

    public static async Task<CardModel?> EtchChosenFromDiscard(
        PlayerChoiceContext choiceContext,
        Player owner)
    {
        CardSelectorPrefs prefs = new(EtchSelectorPrefs.ChosenFromHandPrompt, 1);
        IEnumerable<CardModel> chosen = await CardSelectCmd.FromCombatPile(
            choiceContext,
            PileType.Discard.GetPile(owner),
            owner,
            prefs);

        CardModel? card = chosen.FirstOrDefault();
        return card != null && await Etch(card) ? card : null;
    }

    public static async Task<CardModel?> EtchRandomFromDiscard(Player owner)
    {
        CardPile pile = PileType.Discard.GetPile(owner);
        List<CardModel> eligible = pile.Cards
            .Where(card => card is not IComplexCard)
            .ToList();

        if (eligible.Count == 0)
            return null;

        CardModel card = owner.RunState.Rng.CombatCardSelection.NextItem(eligible);
        return await Etch(card) ? card : null;
    }

    public static async Task<CardModel> EtchCopy(
        CardModel source,
        bool forceExhaust = false)
    {
        CardModel copy = source.CreateClone();
        if (forceExhaust)
            CardCmd.ApplyKeyword(copy, CardKeyword.Exhaust);

        await AddGeneratedCard(copy);
        return copy;
    }

    public static async Task<CardModel> EtchNewCopy<T>(
        Player owner,
        bool forceExhaust = false,
        bool upgrade = false)
        where T : CardModel
    {
        CardModel newCard = owner.Creature.CombatState.CreateCard<T>(owner);
        if (forceExhaust)
            CardCmd.ApplyKeyword(newCard, CardKeyword.Exhaust);
        if (upgrade)
            CardCmd.Upgrade(newCard);

        await AddGeneratedCard(newCard);
        return newCard;
    }

    private static async Task AddGeneratedCard(CardModel card)
    {
        await CardPileCmd.AddGeneratedCardToCombat(
            card,
            SpellCardPile.SpellPileType,
            card.Owner,
            CardPilePosition.Random);

        await WizardRelicHooks.AfterCardEtched(card);
        await WizardPowerHooks.AfterCardEtched(card);
        await WizardRelicHooks.AfterSpellCreated(card);
    }

    // Carbon Paper uses this path for its bonus copy. It deliberately fires Etch
    // listeners but not creation listeners, preventing recursive duplication.
    public static async Task<CardModel> EtchBonusGeneratedCopy(CardModel source)
    {
        CardModel copy = source.CreateClone();

        await CardPileCmd.AddGeneratedCardToCombat(
            copy,
            SpellCardPile.SpellPileType,
            source.Owner,
            CardPilePosition.Random);

        await WizardRelicHooks.AfterCardEtched(copy);
        await WizardPowerHooks.AfterCardEtched(copy);
        return copy;
    }
}
