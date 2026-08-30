using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;


public class Ice_Wall() : WizardCard(0, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Cast) };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[]
        {
            new IntVar("buffer", 1)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Ice_Wall card = this;
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        if (!card.WasCast) return;

        BufferPower bufferPower = await PowerCmd.Apply<BufferPower>(choiceContext, card.Owner.Creature, card.DynamicVars["buffer"].BaseValue, card.Owner.Creature, (CardModel) card);

    }

    protected override void OnUpgrade() => this.DynamicVars["buffer"].UpgradeValueBy(1);
}