using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Keywords;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Cards;

public class Font_of_Knowledge() : WizardCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[]
        {
            new IntVar("powCount", 1),
            new IntVar("cardDraw", 4)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Font_of_Knowledge card = this;
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "PowerUp", card.Owner.Character.PowerUpAnimDelay);
        if (card.IsUpgraded)
        {
            var power = await PowerCmd.Apply<UFontOfKnowledgePower>(choiceContext, card.Owner.Creature, card.DynamicVars["powCount"].BaseValue, card.Owner.Creature, card);
        }
        else
        {
            var power = await PowerCmd.Apply<FontOfKnowledgePower>(choiceContext, card.Owner.Creature, card.DynamicVars["powCount"].BaseValue, card.Owner.Creature, card);

        }
    }
    
    protected override void OnUpgrade() => this.DynamicVars["cardDraw"].UpgradeValueBy(-1);
}