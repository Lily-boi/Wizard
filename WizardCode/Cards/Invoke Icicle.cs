using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Keywords;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Cards;

public class Invoke_Icicle() :
    WizardCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new IHoverTip[]
        {
            HoverTipFactory.FromKeyword(WizardKeywords.Etch),
            HoverTipFactory.FromCard<Icicle>()
        };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new IntVar("PowerAmount", 1) };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(
            Owner.Creature,
            "PowerUp",
            Owner.Character.PowerUpAnimDelay);

        decimal amount = DynamicVars["PowerAmount"].BaseValue;
        if (IsUpgraded)
        {
            await PowerCmd.Apply<InvokeUIciclePower>(
                choiceContext, Owner.Creature, amount, Owner.Creature, this);
        }
        else
        {
            await PowerCmd.Apply<InvokeIciclePower>(
                choiceContext, Owner.Creature, amount, Owner.Creature, this);
        }
    }
}
