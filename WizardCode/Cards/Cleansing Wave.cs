using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Wizard.WizardCode.Cards;

public class Cleansing_Wave() : WizardCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new BlockVar(6M, ValueProp.Move) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Cleansing_Wave card = this;
        await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block, cardPlay);

        var prefs = new CardSelectorPrefs(card.SelectionScreenPrompt, 1);
        var chosen = (await CardSelectCmd.FromCombatPile(
            choiceContext, PileType.Draw.GetPile(card.Owner), card.Owner, prefs)).FirstOrDefault();

        if (chosen != null)
            await CardCmd.Exhaust(choiceContext, chosen);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(2M);
}