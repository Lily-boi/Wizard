using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

public class Divide() : WizardCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch) };

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new[] { CardKeyword.Exhaust };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Divide card = this;
        var statuses = card.Owner.PlayerCombatState.AllCards
            .Where(c => c.Type == CardType.Status && c.Pile.Type != PileType.Exhaust)
            .ToList();

        foreach (var status in statuses)
        {
            if (card.IsUpgraded)
            {
                var result = await CardCmd.TransformTo<Fizzle>(status);
                if (result is { success: true })
                    await EtchCmd.Etch(result.Value.cardAdded);
            }
            else
            {
                await EtchCmd.Etch(status);
            }
        }
    }
}