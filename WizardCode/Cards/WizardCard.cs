// WizardCard.cs — added property, everything else unchanged from before
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Wizard.WizardCode.Character;
using Wizard.WizardCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Commands;
using Wizard.WizardCode.Keywords;

namespace Wizard.WizardCode.Cards;

[Pool(typeof(WizardCardPool))]
public abstract class WizardCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    // True for the duration of OnPlay when this card is being played via our Cast
    // mechanic, rather than played normally from Hand. Set/cleared by CastCmd.
    public bool WasCast { get; internal set; }

    public override async Task AfterAutoPostPlayPhaseEntered(
        PlayerChoiceContext choiceContext, Player player)
    {
        await base.AfterAutoPostPlayPhaseEntered(choiceContext, player);

        CardPile? pile = Pile;
        if (pile == null || pile.Type != SpellCardPile.SpellPileType || player != Owner) return;
        if (!Keywords.Contains(WizardKeywords.Bountiful)) return;

        await CastCmd.CastFromSpellPile(choiceContext, this);
    }
}