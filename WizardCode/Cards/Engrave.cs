using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public sealed class Engrave : WizardCard
{
    public Engrave() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Engrave source = this;

        if (source.IsUpgraded)
        {
            await EtchCmd.EtchChosenFromHand(choiceContext, this._owner, this, 2);
        }
        else
        {
            await EtchCmd.EtchRandomFromHand(Owner, 2);
        }
    }
}