using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;

namespace Wizard.WizardCode.Cards;


public class Gust() : WizardCard(1, CardType.Skill,
    CardRarity.Common, TargetType.AnyEnemy)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[]
        {
            new BlockVar(3M, ValueProp.Move),
            new PowerVar<WeakPower>(1M)
        };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromPower<WeakPower>() };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Gust card = this;
        ArgumentNullException.ThrowIfNull((object) play.Target, "play.Target");
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Cast", card.Owner.Character.CastAnimDelay);
        Decimal num = await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block, play);
        Decimal castWeak = (card.WasCast ? 1 : 0) + card.DynamicVars.Block.BaseValue;
        WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, castWeak, card.Owner.Creature, (CardModel) card);

    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Block.UpgradeValueBy(2M);
    }
}