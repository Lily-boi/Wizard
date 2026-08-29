using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public class Foresight() : WizardCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch) };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new CardsVar(3) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Foresight card = this;
        var topCards = PileType.Draw.GetPile(card.Owner).Cards.Take(card.DynamicVars.Cards.IntValue).ToList();
        if (topCards.Count == 0) return;

        var prefs = new CardSelectorPrefs(card.SelectionScreenPrompt, 1);
        var chosen = (await CardSelectCmd.FromCombatPile(
            choiceContext, PileType.Draw.GetPile(card.Owner), card.Owner, prefs,
            c => topCards.Contains(c))).FirstOrDefault();

        if (chosen != null)
            await EtchCmd.Etch(chosen);
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(2M);
}