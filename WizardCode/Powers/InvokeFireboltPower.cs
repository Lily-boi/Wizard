using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Cards;

namespace Wizard.WizardCode.Powers;

public sealed class InvokeFireboltPower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        var power = this;
        if (!participants.Contains(power.Owner)) return;
        power.Flash();
        for (int i = 0; i < power.Amount; i++)
        {
            var card = power.CombatState.CreateCard<Firebolt>(power.Owner.Player);
            CardCmd.ApplyKeyword(card, CardKeyword.Exhaust);
            await CardPileCmd.AddGeneratedCardToCombat(card, SpellCardPile.SpellPileType, power.Owner.Player);
        }
    }
}