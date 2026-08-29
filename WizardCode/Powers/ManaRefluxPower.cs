using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Powers;

namespace Wizard.WizardCode.Powers;

public sealed class ManaRefluxPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new EnergyVar(2) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.ForEnergy(this) };

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        var power = this;
        if (cardPlay.Card.Owner.Creature != power.Owner) return;
        if (cardPlay.Card is not WizardCard wc || !wc.WasCast) return;
        if (cardPlay.Card.EnergyCost.GetResolved() < power.DynamicVars.Energy.IntValue) return;
        power.Flash();
        await PlayerCmd.GainEnergy(power.Amount, power.Owner.Player);
    }
}