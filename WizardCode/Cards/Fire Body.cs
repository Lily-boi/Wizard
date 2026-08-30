using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Cards;

public class Fire_Body() :
    WizardCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new PowerVar<FireBodyPower>(2M) };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(
            Owner.Creature,
            "PowerUp",
            Owner.Character.PowerUpAnimDelay);

        await PowerCmd.Apply<FireBodyPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["FireBodyPower"].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade() =>
        DynamicVars["FireBodyPower"].UpgradeValueBy(1M);
}
