using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.CardPiles;

namespace Wizard.WizardCode.Commands;

public static class CastCmd
{
    public static async Task<CardModel?> CastTopOfSpellPile(
        PlayerChoiceContext choiceContext, Player player, bool forceExhaust = false)
    {
        var spellPile = SpellCardPile.SpellPileType.GetPile(player);
        var card = spellPile.Cards.FirstOrDefault();
        if (card == null || player.Creature.IsDead) return null;

        await CardPileCmd.Add(card, PileType.Play);
        card.ExhaustOnNextPlay = forceExhaust;
        await CardCmd.AutoPlay(choiceContext, card, null);
        return card;
    }

    public static async Task CastMultiple(
        PlayerChoiceContext choiceContext, Player player, int count, bool forceExhaust = false)
    {
        for (int i = 0; i < count; i++)
            if (await CastTopOfSpellPile(choiceContext, player, forceExhaust) == null) break;
    }
}