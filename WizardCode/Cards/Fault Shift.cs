using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public class Fault_Shift() : WizardCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override bool GainsBlock => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[]
        {
            HoverTipFactory.FromKeyword(WizardKeywords.Etch),
            HoverTipFactory.FromKeyword(WizardKeywords.Bountiful)
        };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        WizardKeywords.Bountiful
    };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[]
        {
            new PowerVar<WeakPower>(1M),
            new PowerVar<VulnerablePower>(1M)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Fault_Shift card = this;
        WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, card.DynamicVars.Weak.BaseValue, card.Owner.Creature, (CardModel) card);
        VulnerablePower vulnerablePower = await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, card.DynamicVars.Vulnerable.BaseValue, card.Owner.Creature, (CardModel) card);
        await EtchCmd.Etch(card);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Vulnerable.UpgradeValueBy(1);
        this.DynamicVars.Weak.UpgradeValueBy(1);
    }
}