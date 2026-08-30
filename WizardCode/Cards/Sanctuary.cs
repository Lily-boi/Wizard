using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Wizard.WizardCode.Cards;

namespace Wizard.WizardCode.Cards;


public class Sanctuary() : WizardCard(0, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] { CardKeyword.Innate };
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        new DynamicVar[]
        {
            new IntVar("artifact", 2)
        };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Sanctuary card = this;
        
        await PowerCmd.Apply<ArtifactPower>(choiceContext, card.Owner.Creature, card.DynamicVars["artifact"].BaseValue, card.Owner.Creature, card);

    }

    protected override void OnUpgrade() => this.DynamicVars["artifact"].UpgradeValueBy(1);
}