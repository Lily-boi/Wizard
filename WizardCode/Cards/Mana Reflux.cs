using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Cards;

public class Mana_Reflux() : WizardCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new[] { this.EnergyHoverTip };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[]
        {
            new EnergyVar(2),
            new PowerVar<ManaRefluxPower>(1M)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Mana_Reflux card = this;
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "PowerUp", card.Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<ManaRefluxPower>(choiceContext, card.Owner.Creature, card.DynamicVars["ManaRefluxPower"].BaseValue, card.Owner.Creature, card);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}