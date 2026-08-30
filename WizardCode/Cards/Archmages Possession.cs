using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public class Archmages_Possession() : WizardCard(3, CardType.Attack, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Cast) };

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] {CardKeyword.Exhaust };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Archmages_Possession card = this;
        var toCast = SpellCardPile.SpellPileType.GetPile(card.Owner).Cards.ToList();
        foreach (var spellCard in toCast)
        {
            await CastCmd.CastFromSpellPile(choiceContext, spellCard);
        }
    }
    
    protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Retain);
}