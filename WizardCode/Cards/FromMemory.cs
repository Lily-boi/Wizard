using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public sealed class From_Memory :
    WizardCard,
    IComplexCard
{
    public From_Memory() :
        base(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Cast) };

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[]
        {
            WizardKeywords.Complex,
            CardKeyword.Exhaust
        };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new CardsVar(1) };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            CardModel? castCard = await CastCmd.CastTopOfSpellPile(
                choiceContext,
                Owner);

            if (castCard == null)
                break;
        }
    }

    protected override void OnUpgrade() =>
        DynamicVars.Cards.UpgradeValueBy(1M);
}
