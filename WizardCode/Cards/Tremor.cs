using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Wizard.WizardCode.Cards;

public class Tremor() : WizardCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromPower<WeakPower>() };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[]
        {
            new BlockVar(3M, ValueProp.Move),
            new PowerVar<WeakPower>(1M)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Tremor card = this;
        int enemyCount = card.CombatState.HittableEnemies.Count();
        decimal blockAmount = card.DynamicVars.Block.BaseValue * enemyCount;

        await CreatureCmd.GainBlock(card.Owner.Creature, blockAmount, ValueProp.Move, cardPlay);
        await PowerCmd.Apply<WeakPower>(
            choiceContext, card.CombatState.HittableEnemies, card.DynamicVars.Weak.BaseValue, card.Owner.Creature, card);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(1M);
}