using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Wizard.WizardCode.CardPiles;


namespace Wizard.WizardCode.Powers;

public sealed class FontOfKnowledgePower : WizardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        var power = this;
        if (!participants.Contains(power.Owner)) return;
        int spellPileCount = SpellCardPile.SpellPileType.GetPile(power.Owner.Player).Cards.Count;
        int drawAmount = spellPileCount / 4;
        if (drawAmount <= 0) return;
        power.Flash();
        for (int i = 0; i < power.Amount; i++)
        {
            await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), drawAmount, power.Owner.Player);
        }
    }
}