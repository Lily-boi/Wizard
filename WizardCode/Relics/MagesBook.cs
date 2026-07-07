using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;
using Wizard.WizardCode.Relics;

namespace Wizard.WizardCode.Relics;


public class MagesBook() : WizardRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(WizardKeywords.Etch) };

    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext, 
        CombatSide side, 
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        MagesBook magesBook = this;
        if (!participants.Contains<Creature>(magesBook.Owner.Creature) ||
            magesBook.Owner.PlayerCombatState.TurnNumber > 1)
        {
            GD.Print($"[TheWizard] Is no longer etching");
            return;
        }

        magesBook.Flash();
        //await EtchCmd.EtchChosenFromHand(choiceContext, magesBook._owner, , combatState);
        GD.Print($"[TheWizard] Is etching");
        
    }
}