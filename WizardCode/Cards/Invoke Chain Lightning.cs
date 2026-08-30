using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Keywords;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Cards;

public class Invoke_Chain_Lightning() :
    WizardCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new IHoverTip[]
        {
            HoverTipFactory.FromKeyword(WizardKeywords.Etch),
            HoverTipFactory.FromCard<Chain_Lightning>()
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
            await PowerCmd.Apply<InvokeUChainLightningPower>(
                choiceContext, Owner.Creature, amount, Owner.Creature, this);
        }
        else
        {
            await PowerCmd.Apply<InvokeChainLightningPower>(
                choiceContext, Owner.Creature, amount, Owner.Creature, this);
        }
    }
}
