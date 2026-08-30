using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Relics;

public class ArchmagesMetronome() : WizardRelic, IAfterWizardCardCastRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override bool ShowCounter => CombatManager.Instance.IsInProgress;

    public override int DisplayAmount =>
        !CombatManager.Instance.IsInProgress || IsCanonical
            ? -1
            : Math.Min(
                CastState.GetCastCount(Owner),
                DynamicVars["Casts"].IntValue);

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Casts", 3M),
        new EnergyVar(1),
        new CardsVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(WizardKeywords.Cast)
    ];

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature))
            InvokeDisplayAmountChanged();

        return Task.CompletedTask;
    }

    public async Task AfterWizardCardCast(
        PlayerChoiceContext choiceContext,
        CardModel card)
    {
        if (card.Owner != Owner)
            return;

        InvokeDisplayAmountChanged();
        if (CastState.GetCastCount(Owner) != DynamicVars["Casts"].IntValue)
            return;

        Flash();
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await CardPileCmd.Draw(
            choiceContext,
            DynamicVars.Cards.BaseValue,
            Owner);
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }
}
