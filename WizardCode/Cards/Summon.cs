using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;
using MegaCrit.Sts2.Core.HoverTips;

namespace Wizard.WizardCode.Cards;

public sealed class Summon : WizardCard, IComplexCard
{
    public Summon() : base(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Cast) };
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] { WizardKeywords.Complex };

    protected override IEnumerable<DynamicVar> CanonicalVars {
        get
        {
            return new DynamicVar[]
            {
                new CardsVar(1)
            };
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Summon summon = this;
        for (int i = 0; i < summon.DynamicVars.Cards.IntValue; i++)
        {
            var casted = await CastCmd.CastTopOfSpellPile(choiceContext, summon.Owner);
            if (casted == null) break; // spell pile ran out, stop early
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}