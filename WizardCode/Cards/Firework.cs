using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public class Fireworks() : WizardCard(10, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies), IComplexCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromCard<Fizzle>(), HoverTipFactory.FromKeyword(WizardKeywords.Complex) };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { WizardKeywords.Complex };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new DamageVar(30M, ValueProp.Move), new EnergyVar(1) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Fireworks card = this;
        var attack = DamageCmd.Attack(card.DynamicVars.Damage.BaseValue);
        attack = card.WasCast ? attack.FromSpellPile(card, cardPlay) : attack.FromCard(card, cardPlay);
        AttackCommand attackCommand = await attack
            .TargetingAllOpponents(card.CombatState)
            .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-2);

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || this.IsClone) return Task.CompletedTask;
        int fizzlesPlayed = CombatManager.Instance.History.CardPlaysFinished
            .Count(e => e.CardPlay.Card is Fizzle && e.CardPlay.Player == this.Owner);
        this.EnergyCost.AddThisCombat(-fizzlesPlayed * this.DynamicVars.Energy.IntValue);
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != this.Owner || cardPlay.Card is not Fizzle) return Task.CompletedTask;
        this.EnergyCost.AddThisCombat(-this.DynamicVars.Energy.IntValue);
        return Task.CompletedTask;
    }
}