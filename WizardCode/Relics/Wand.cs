using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Relics;

public class Wand() : WizardRelic, IAfterWizardCardCastRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(WizardKeywords.Cast)
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2M, ValueProp.Move)
    ];

    public async Task AfterWizardCardCast(
        PlayerChoiceContext choiceContext,
        CardModel card)
    {
        if (card.Owner != Owner)
            return;

        List<Creature> enemies = Owner.Creature.CombatState
            .HittableEnemies
            .ToList();

        if (enemies.Count == 0)
            return;

        Flash();
        Creature target = Owner.RunState.Rng.CombatTargets.NextItem(enemies);
        await CreatureCmd.Damage(
            choiceContext,
            target,
            DynamicVars.Damage,
            Owner.Creature);
    }
}
