using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Wizard.WizardCode.Cards;
public class Archmages_Hat() : WizardCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] 
        {             
            new IntVar("powCount", 1),
            new IntVar("costReduce", 1) 
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Archmages_Hat card = this;
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "PowerUp", card.Owner.Character.PowerUpAnimDelay);
        if (card.IsUpgraded)
        {
            await PowerCmd.Apply<ArchmagesHatPower>(choiceContext, card.Owner.Creature,
                card.DynamicVars["powCount"].BaseValue, card.Owner.Creature, card);
        }
        else
        {
            await PowerCmd.Apply<UArchmagesHatPower>(choiceContext, card.Owner.Creature,
                card.DynamicVars["powCount"].BaseValue, card.Owner.Creature, card);

        }
        
    }

    protected override void OnUpgrade() => this.DynamicVars["costReduce"].UpgradeValueBy(1);
}