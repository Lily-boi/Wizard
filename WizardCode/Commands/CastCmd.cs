using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Relics;

namespace Wizard.WizardCode.Commands;

public static class CastCmd
{
    public static async Task<CardModel?> CastTopOfSpellPile(
        PlayerChoiceContext choiceContext,
        Player player,
        bool forceExhaust = false)
    {
        if (CastState.IsCastBlocked(player) || player.Creature.IsDead)
            return null;

        CardModel? card = SpellCardPile.SpellPileType
            .GetPile(player)
            .Cards
            .FirstOrDefault();

        return card == null
            ? null
            : await CastCard(choiceContext, card, forceExhaust);
    }

    public static async Task<CardModel?> CastFromSpellPile(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool forceExhaust = false)
    {
        if (CastState.IsCastBlocked(card.Owner) || card.Owner.Creature.IsDead)
            return null;

        CardPile spellPile = SpellCardPile.SpellPileType.GetPile(card.Owner);
        if (!spellPile.Cards.Contains(card))
            return null;

        return await CastCard(choiceContext, card, forceExhaust);
    }

    private static async Task<CardModel> CastCard(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool forceExhaust)
    {
        await CardPileCmd.Add(card, PileType.Play);

        // Never clear an ExhaustOnNextPlay applied by another effect.
        if (forceExhaust)
            card.ExhaustOnNextPlay = true;

        CastState.BeginCast(card);
        if (card is WizardCard wc)
            wc.WasCast = true;

        try
        {
            await CardCmd.AutoPlay(choiceContext, card, null);
            CastState.RecordCast(card.Owner);
            await WizardRelicHooks.AfterCardCast(choiceContext, card);
            return card;
        }
        finally
        {
            if (card is WizardCard wizardCard)
                wizardCard.WasCast = false;

            CastState.EndCast(card);
        }
    }

    public static async Task CastMultiple(
        PlayerChoiceContext choiceContext,
        Player player,
        int count,
        bool forceExhaust = false)
    {
        for (int i = 0; i < count; i++)
        {
            if (await CastTopOfSpellPile(choiceContext, player, forceExhaust) == null)
                break;
        }
    }

    public static AttackCommand FromSpellPile(
        this AttackCommand attack,
        CardModel card,
        CardPlay? cardPlay)
    {
        attack.FromCard(card, cardPlay);
        attack.WithNoAttackerAnim();
        return attack;
    }
}
