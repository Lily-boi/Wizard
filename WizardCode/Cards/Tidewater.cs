using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Cards;

public class Tidewater() : WizardCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new EnergyVar(2), new CardsVar(1), new IntVar("castEnergy", 1)};

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Tidewater card = this;
        if (card.WasCast)
        {
            await PlayerCmd.GainEnergy(card.DynamicVars["castEnergy"].BaseValue, card.Owner);
            await CardPileCmd.Draw(choiceContext, card.DynamicVars.Cards.BaseValue, card.Owner);
        }
        else
        {
            EnergyNextTurnPower energyNextTurnPower = 
                await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, card.Owner.Creature, card.DynamicVars.Energy.BaseValue, card.Owner.Creature, (CardModel) card);        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1);
}