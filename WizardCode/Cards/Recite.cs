using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;
using MegaCrit.Sts2.Core.HoverTips;

namespace Wizard.WizardCode.Cards;

public sealed class Recite : WizardCard, IComplexCard
{
    public Recite() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch) };
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] { WizardKeywords.Complex };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new[] { (DynamicVar)new CardsVar(1) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Recite recite = this;
        for (int i = 0; i < recite.DynamicVars.Cards.IntValue; i++)
        {
            var casted = await CastCmd.CastTopOfSpellPile(choiceContext, recite.Owner);
            if (casted == null) break; // spell pile ran out, stop early
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}