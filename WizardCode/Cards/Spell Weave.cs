using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;


public class Spell_Weave() : WizardCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[]
        {
            HoverTipFactory.FromKeyword(WizardKeywords.Etch)
        };
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] {CardKeyword.Exhaust };
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        new DynamicVar[]
        {
            new CardsVar(2),
            new IntVar("etchCards", 2)
        };
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Spell_Weave card = this;
        IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, card.DynamicVars.Cards.BaseValue, card.Owner);
        await EtchCmd.EtchChosenFromHand(choiceContext, this._owner, this, 2);
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(2);
}