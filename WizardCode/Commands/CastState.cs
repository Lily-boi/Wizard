using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Wizard.WizardCode.Commands;

// Turn-scoped bookkeeping for the Cast mechanic. Not derived from any decompiled
// source — this is new infrastructure, since nothing in the base game needs to track
// "how many times has this specific mechanic fired this turn" the way Combo/Momentum/
// Scorch do. Reset every turn from MagesBook's existing BeforeSideTurnStart hook,
// since that's guaranteed to run for every Wizard, every turn.
public static class CastState
{
    private static readonly Dictionary<Player, int> _castsThisTurn = new();
    private static readonly HashSet<Player> _castBlocked = new();

    public static int GetCastCount(Player player) =>
        _castsThisTurn.TryGetValue(player, out var c) ? c : 0;

    public static void RecordCast(Player player) =>
        _castsThisTurn[player] = GetCastCount(player) + 1;

    public static bool IsCastBlocked(Player player) => _castBlocked.Contains(player);

    public static void BlockCastingThisTurn(Player player) => _castBlocked.Add(player);

    public static void ResetForNewTurn(Player player)
    {
        _castsThisTurn[player] = 0;
        _castBlocked.Remove(player);
    }
}