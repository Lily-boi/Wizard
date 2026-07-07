using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using BaseLib.Utils;
using MegaCrit.Sts2.addons.mega_text;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.ui;

namespace Wizard.WizardCode.ui;

public partial class NSpellPile1 : NCombatCardPile
{

    protected override PileType Pile => SpellCardPile.SpellPileType;

    private static readonly string _scenePath = "res://Wizard/scenes/SpellPile.tscn";

    public static AddedNode<NCombatPilesContainer, NSpellPile1> _ = new(container =>
    {
        var spellPileButton = ResourceLoader.Load<PackedScene>(_scenePath).Instantiate<NSpellPile1>();
        spellPileButton.Name = "SpellPile";
        spellPileButton.Position = SpellCardPile.ButtonPosition;

        var background = spellPileButton.GetNode<TextureRect>("CountContainer/Background");
        background.Texture = ResourceLoader.Load<Texture2D>("res://images/packed/combat_ui/pile_button_count.png");

        var countLabel = spellPileButton.GetNode<WizardMegaLabel>("CountContainer/Count");
        var font = PreloadManager.Cache.GetAsset<Font>("res://themes/kreon_bold_glyph_space_one.tres");
        countLabel.AddThemeFontOverride(ThemeConstants.Label.Font, font);
        countLabel.MinFontSize = 20;
        countLabel.MaxFontSize = 26;

        return spellPileButton;
    });

    public override void _Ready()
    {
        ConnectSignals();
        _emptyPileMessage = new LocString("combat_messages", "OPEN_EMPTY_SPELL_PILE");
        SetAnimInOutPositions();
        Disable();
    }

    protected override void SetAnimInOutPositions()
    {
        _showPosition = Position;
        _hidePosition = Position; 
    }

    public override void Initialize(Player player)
    {
        _localPlayer = player;
        _pile = Pile.GetPile(_localPlayer);
        _pile.ContentsChanged += HandleContentsChanged;
        base.Initialize(player);

        _currentCount = _pile.Cards.Count;
        _countLabel.SetTextAutoSize(_currentCount.ToString());
        Visible = true;
        Enable();
    }

    private void HandleContentsChanged()
    {
        _currentCount = _pile?.Cards.Count ?? 0;
        _countLabel.SetTextAutoSize(_currentCount.ToString());
        
        if (_currentCount > 0 && !Visible)
        {
            AnimIn();
            Enable();
        }
    }

    protected override void OnFocus()
    {
        NHoverTipSet.Remove(this);
        var title = new LocString("static_hover_tips", "SPELL_PILE.title");
        title.Add("Hotkey", "None");
        var hoverTip = new HoverTip(title, new LocString("static_hover_tips", "SPELL_PILE.description"));
        var tooltip = NHoverTipSet.CreateAndShow(this, hoverTip);
        if (tooltip != null)
        {
            tooltip.GlobalPosition = GlobalPosition + new Vector2(20f, -220f);
        }
    }

    protected override void OnUnfocus()
    {
        base.OnUnfocus();
    }

}