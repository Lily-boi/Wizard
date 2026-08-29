using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Cards;

namespace Wizard.WizardCode.Commands;

public static class CastCmd
{
    public static async Task<CardModel?> CastTopOfSpellPile(
        PlayerChoiceContext choiceContext, Player player, bool forceExhaust = false)
    {
        if (CastState.IsCastBlocked(player)) return null;

        var spellPile = SpellCardPile.SpellPileType.GetPile(player);
        var card = spellPile.Cards.FirstOrDefault();
        if (card == null || player.Creature.IsDead) return null;

        await CardPileCmd.Add(card, PileType.Play);
        card.ExhaustOnNextPlay = forceExhaust;

        if (card is WizardCard wc) wc.WasCast = true;
        try { await CardCmd.AutoPlay(choiceContext, card, null); }
        finally { if (card is WizardCard wc2) wc2.WasCast = false; }

        CastState.RecordCast(player);
        return card;
    }

    public static async Task<CardModel?> CastFromSpellPile(
        PlayerChoiceContext choiceContext, CardModel card, bool forceExhaust = false)
    {
        if (CastState.IsCastBlocked(card.Owner)) return null;
        if (card.Owner.Creature.IsDead) return null;

        var spellPile = SpellCardPile.SpellPileType.GetPile(card.Owner);
        if (!spellPile.Cards.Contains(card)) return null;

        await CardPileCmd.Add(card, PileType.Play);
        card.ExhaustOnNextPlay = forceExhaust;

        if (card is WizardCard wc) wc.WasCast = true;
        try { await CardCmd.AutoPlay(choiceContext, card, null); }
        finally { if (card is WizardCard wc2) wc2.WasCast = false; }

        CastState.RecordCast(card.Owner);
        return card;
    }

    public static async Task CastMultiple(
        PlayerChoiceContext choiceContext, Player player, int count, bool forceExhaust = false)
    {
        for (int i = 0; i < count; i++)
            if (await CastTopOfSpellPile(choiceContext, player, forceExhaust) == null) break;
    }

    public static AttackCommand FromSpellPile(this AttackCommand attack, CardModel card, CardPlay? cardPlay)
    {
        attack.FromCard(card, cardPlay);
        attack.WithNoAttackerAnim();
        return attack;
    }
}