using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;
public class Lightning_Strike() : WizardCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch), HoverTipFactory.FromKeyword(WizardKeywords.Bountiful) };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { WizardKeywords.Bountiful };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new DamageVar(20M, ValueProp.Move), new CardsVar(1) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Lightning_Strike card = this;
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        var attack = DamageCmd.Attack(card.DynamicVars.Damage.BaseValue);
        attack = card.WasCast ? attack.FromSpellPile(card, cardPlay) : attack.FromCard(card, cardPlay);
        AttackCommand attackCommand = await attack
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3")
            .Execute(choiceContext);
        for (int i = 0; i < card.DynamicVars.Cards.IntValue; i++)
            await EtchCmd.EtchNewCopy<Chain_Lightning>(card.Owner);
        await EtchCmd.Etch(card);
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}