using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Keywords;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Cards;

public class Invoke_Firebolt() : WizardCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { 
            HoverTipFactory.FromKeyword(WizardKeywords.Etch), 
            HoverTipFactory.FromCard<Firebolt>() };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new IntVar("powCount", 1) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Invoke_Firebolt card = this;
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "PowerUp", card.Owner.Character.PowerUpAnimDelay);
        if (card.IsUpgraded)
        {
            var power = await PowerCmd.Apply<InvokeUFireboltPower>(choiceContext, card.Owner.Creature, card.DynamicVars["powCount"].BaseValue, card.Owner.Creature, card);
        }
        else
        {
            var power = await PowerCmd.Apply<InvokeFireboltPower>(choiceContext, card.Owner.Creature, card.DynamicVars["powCount"].BaseValue, card.Owner.Creature, card);

        }
    }
}