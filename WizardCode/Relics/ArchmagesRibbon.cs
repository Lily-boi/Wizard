using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Relics;

public class ArchmagesRibbon() : WizardRelic, IAfterWizardCardEtchedRelic
{
    private object? _lastCombatState;
    private int _lastTurn = -1;

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(4M, ValueProp.Unpowered)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromKeyword(WizardKeywords.Etch)
    ];

    public async Task AfterWizardCardEtched(CardModel card)
    {
        if (card.Owner != Owner || !TryActivateThisTurn())
            return;

        Flash();
        await CreatureCmd.GainBlock(
            Owner.Creature,
            DynamicVars.Block,
            cardPlay: null);
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
