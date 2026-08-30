using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Cards;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;


public class Innovation() : WizardCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch), HoverTipFactory.FromCard<Fizzle>(), };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new BlockVar(2M, ValueProp.Move) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Innovation card = this;
        for (int i = 0; i < 3; i++)
            await EtchCmd.EtchNewCopy<Fizzle>(card.Owner);
        int count = SpellCardPile.SpellPileType.GetPile(card.Owner).Cards
            .Count(c => c.GetType() == typeof(Fizzle));
        decimal amount = card.DynamicVars.Block.BaseValue * count;
        await CreatureCmd.GainBlock(card.Owner.Creature, amount, ValueProp.Move, cardPlay);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(1M);
}