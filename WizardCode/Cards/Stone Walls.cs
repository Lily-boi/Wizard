using MegaCrit.Sts2.Core.Commands;
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


public class Stone_Walls() : WizardCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    
    public override bool GainsBlock => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch) };
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new BlockVar(3M, ValueProp.Move) };


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Stone_Walls card = this;
        IEnumerable<CardModel> toEtch = card.Owner.PlayerCombatState.AllCards
            .Where(c =>c.Pile.Type == PileType.Hand && c is not IComplexCard)
            .ToList();
        foreach (var item in toEtch)
        {
            await EtchCmd.Etch(item);
            await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block, play);
        }
        
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(2M);
}