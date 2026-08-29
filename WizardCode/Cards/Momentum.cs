using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.Commands;

namespace Wizard.WizardCode.Cards;

public class Momentum() : WizardCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new DamageVar(5M, ValueProp.Move) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Momentum card = this;
        int hits = CastState.GetCastCount(card.Owner);

        var attack = DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).WithHitCount(hits);
        attack = card.WasCast ? attack.FromSpellPile(card, cardPlay) : attack.FromCard(card, cardPlay);
        AttackCommand attackCommand = await attack
            .TargetingRandomOpponents(card.CombatState)
            .WithHitFx("vfx/vfx_attack_slash", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}