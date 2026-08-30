using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Cards;

/// <summary>
/// ITomeCard makes this the Wizard card granted by Darv's Dusty Tome.
/// Its default TomeCharacter implementation resolves the Wizard through this
/// card's WizardCardPool membership.
/// </summary>
public sealed class Time_Warp() :
    WizardCard(3, CardType.Power, CardRarity.Ancient, TargetType.Self),
    ITomeCard
{

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new PowerVar<TimeWarpPower>(1M) };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(
            Owner.Creature,
            "PowerUp",
            Owner.Character.PowerUpAnimDelay);

        await PowerCmd.Apply<TimeWarpPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["TimeWarpPower"].BaseValue,
            Owner.Creature,
            this);
    }

    // Dusty Tome grants Ancient cards upgraded, so Time Warp needs a useful +.
    protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Innate);
}
