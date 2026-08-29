using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public class Archmages_Possession() : WizardCard(3, CardType.Attack, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Cast) };

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] { CardKeyword.Retain, CardKeyword.Exhaust };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Archmages_Possession card = this;
        const int safetyCap = 50; // guards against a genuine infinite loop (e.g. Chain Lightning re-etching itself); raise/remove if undesired
        for (int i = 0; i < safetyCap; i++)
        {
            var cast = await CastCmd.CastTopOfSpellPile(choiceContext, card.Owner);
            if (cast == null) break;
        }
    }
}


//MAKE A LIST OF CARDS IN SPELL PILE AND THEN ITERATE + CAST