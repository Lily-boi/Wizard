using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public class Death() : WizardCard(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Cast) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Death card = this;
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        if (!card.WasCast) return;

        decimal halfHp = Math.Ceiling(cardPlay.Target.CurrentHp / 2M);
        await CreatureCmd.Damage(choiceContext, new[] { cardPlay.Target }, halfHp,
            ValueProp.Unblockable | ValueProp.Unpowered, card.Owner.Creature, card, cardPlay);
    }
}