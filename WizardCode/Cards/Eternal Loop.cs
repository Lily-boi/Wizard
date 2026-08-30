using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Keywords;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Cards;

public class Eternal_Loop() :
    WizardCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch) };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new PowerVar<EternalLoopPower>(1M) };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(
            Owner.Creature,
            "PowerUp",
            Owner.Character.PowerUpAnimDelay);

        await PowerCmd.Apply<EternalLoopPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["EternalLoopPower"].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
}
