using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;


public class Wizard_s_Gambit() : WizardCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[]
        {
            HoverTipFactory.FromKeyword(WizardKeywords.Etch),
        };
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        new DynamicVar[]
        {
            new CardsVar(1),
            new EnergyVar(3),
        };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Wizard_s_Gambit card = this;
        var toEtch = await EtchCmd.EtchChosenFromHand(choiceContext, card.Owner, card, 1);
        foreach (var item in toEtch)
        {
            if (item.EnergyCost.GetAmountToSpend() >= 3)
            {
                for (int i = 0; i <= card.DynamicVars.Cards.BaseValue; i++) await EtchCmd.EtchCopy(item);
            }
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1);
}