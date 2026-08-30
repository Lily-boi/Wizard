using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace Wizard.WizardCode.Commands;

public static class CastState
{
    private sealed class PlayerTurnState
    {
        public object? CombatState { get; set; }
        public int TurnNumber { get; set; } = -1;
        public int CastsThisTurn { get; set; }
        public bool IsCastingBlocked { get; set; }
    }

    private static readonly ConditionalWeakTable<Player, PlayerTurnState> States = new();
    private static readonly HashSet<CardModel> CardsBeingCast = new();

    private static PlayerTurnState GetCurrentState(Player player)
    {
        PlayerTurnState state = States.GetValue(
            player,
            static _ => new PlayerTurnState());
        object? combatState = player.Creature.CombatState;
        int turnNumber = player.PlayerCombatState?.TurnNumber ?? -1;

        if (!ReferenceEquals(state.CombatState, combatState) ||
            state.TurnNumber != turnNumber)
        {
            state.CombatState = combatState;
            state.TurnNumber = turnNumber;
            state.CastsThisTurn = 0;
            state.IsCastingBlocked = false;
        }

        return state;
    }

    public static int GetCastCount(Player player) =>
        GetCurrentState(player).CastsThisTurn;

    public static void RecordCast(Player player) =>
        GetCurrentState(player).CastsThisTurn++;

    public static bool IsCastBlocked(Player player) =>
        GetCurrentState(player).IsCastingBlocked;

    public static void BlockCastingThisTurn(Player player) =>
        GetCurrentState(player).IsCastingBlocked = true;

    public static void ResetForNewTurn(Player player)
    {
        PlayerTurnState state = GetCurrentState(player);
        state.CastsThisTurn = 0;
        state.IsCastingBlocked = false;
    }

    public static bool IsBeingCast(CardModel card) => CardsBeingCast.Contains(card);

    internal static void BeginCast(CardModel card) => CardsBeingCast.Add(card);

    internal static void EndCast(CardModel card) => CardsBeingCast.Remove(card);
}
