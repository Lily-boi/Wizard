using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public sealed class Inscribe : WizardCard
{
    public Inscribe() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Inscribe source = this;

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