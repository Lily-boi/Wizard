using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public class Chain_Lightning() : WizardCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch) };

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] { CardKeyword.Exhaust };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new DamageVar(1M, ValueProp.Move) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Chain_Lightning card = this;
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // Snapshot the count up front so newly-Etched copies don't chain into
        // an infinite loop within this same resolution.
        int existingCopies = SpellCardPile.SpellPileType.GetPile(card.Owner).Cards
            .Count(c => c.GetType() == typeof(Chain_Lightning));
        int activations = 1 + existingCopies + (this.WasCast ? 1 : 0);

        for (int i = 0; i < activations; i++)
        {
            AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue)
                .FromCard(card, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash", tmpSfx: "blunt_attack.mp3")
                .Execute(choiceContext);

            await EtchCmd.EtchCopy(card);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(1M);
}