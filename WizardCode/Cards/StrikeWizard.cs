using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;

namespace Wizard.WizardCode.Cards;


public class StrikeWizard() : WizardCard(1,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
protected override HashSet<CardTag> CanonicalTags
{
    get => new HashSet<CardTag>() { CardTag.Strike };
}

protected override IEnumerable<DynamicVar> CanonicalVars
{
    get
    {
        return new DynamicVar[]
        {
            new DamageVar(6, ValueProp.Move)
        };
    }
}

protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
{
    StrikeWizard card = this;
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard((CardModel) card, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
}

protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}