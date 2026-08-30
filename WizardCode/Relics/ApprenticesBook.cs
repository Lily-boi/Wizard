using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Relics;

public class ApprenticesBook() : WizardRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override RelicModel GetUpgradeReplacement() =>
        ModelDb.Relic<Archmage_s_Book>();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(WizardKeywords.Etch)
    ];

    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) ||
            Owner.PlayerCombatState.TurnNumber > 1)
        {
            return;
        }

        Flash();
        var etched = await EtchCmd.EtchChosenFromHand(
            choiceContext,
            Owner,
            this,
            DynamicVars.Cards.IntValue);

        if (etched.Count > 0)
            await CardPileCmd.Draw(choiceContext, 1M, Owner);
    }
}