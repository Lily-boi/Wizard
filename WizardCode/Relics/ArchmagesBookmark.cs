using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Relics;

public class ArchmagesBookmark() : WizardRelic, IAfterWizardCardCastRelic
{
    private object? _lastCombatState;
    private int _lastTurn = -1;

    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Cost", 2M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(WizardKeywords.Cast),
        HoverTipFactory.FromKeyword(WizardKeywords.Etch)
    ];

    public async Task AfterWizardCardCast(
        PlayerChoiceContext choiceContext,
        CardModel card)
    {
        if (card.Owner != Owner ||
            card.EnergyCost.CostsX ||
            card.EnergyCost.Canonical < DynamicVars["Cost"].IntValue ||
            !TryActivateThisTurn())
        {
            return;
        }

        Flash();
        await EtchCmd.EtchCopy(card, forceExhaust: true);
    }

    private bool TryActivateThisTurn()
    {
        object? combatState = Owner.Creature.CombatState;
        int turn = Owner.PlayerCombatState.TurnNumber;

        if (ReferenceEquals(_lastCombatState, combatState) && _lastTurn == turn)
            return false;

        _lastCombatState = combatState;
        _lastTurn = turn;
        return true;
    }
}
