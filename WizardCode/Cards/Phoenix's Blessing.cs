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


public class Phoenix_s_Blessing() : WizardCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch) };
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] { CardKeyword.Exhaust };


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Phoenix_s_Blessing card = this;
        IEnumerable<CardModel> attacks = card.Owner.PlayerCombatState.AllCards
            .Where(c => c.Type == CardType.Attack && c.Pile.Type == PileType.Discard && c is not IComplexCard)
            .ToList();
        foreach (var item in attacks)
        {
            await EtchCmd.Etch(item);
        }
    }

    protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Exhaust);
}