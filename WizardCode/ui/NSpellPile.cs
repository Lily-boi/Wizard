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
using MegaCrit.Sts2.addons.mega_text;
using BaseLib.Utils;
using Wizard.WizardCode.CardPiles;
using Wizard.WizardCode.Config;

namespace Wizard.WizardCode.ui;

public partial class NSpellPile : NCombatCardPile
{
    private List<NPreviewCardHolder> _previewHolders = [];
    private const int MaxPreviewCards = 3;
    private const float PreviewDefaultScale = 0.6f;
    private const float PreviewScaleDecrement = 0.15f;
    private const float PreviewYOffset = -15f;
    private const float PreviewXOffset = 140f;
    private const float PreviewSpacingX = -35f;
    private const float PreviewHoverShiftX = 25f;
    private const float HideOffsetX = -150f;
    private const float TooltipOffsetY = -355f;

    // Our own storage — base's equivalents are private and unreachable from here.
    private CardPile? _spellPile;
    private Player? _owner;
    private Godot.Control _spellIcon = null!;
    private Tween? _spellBumpTween;
    private Tween? _previewTween;

    protected override PileType Pile => SpellCardPile.SpellPileType;

    private static readonly string _scenePath = "res://Wizard/scenes/SpellPile.tscn";

    public static AddedNode<NCombatPilesContainer, NSpellPile> _ = new(container =>
    {
        var spellPileButton = ResourceLoader.Load<PackedScene>(_scenePath).Instantiate<NSpellPile>();
        spellPileButton.Name = "SpellPile";
        spellPileButton.Position = new Vector2(35, 700);

        var background = spellPileButton.GetNode<TextureRect>("CountContainer/Background");
        background.Texture = ResourceLoader.Load<Texture2D>("res://images/packed/combat_ui/pile_button_count.png");

        var countLabel = spellPileButton.GetNode<MegaLabel>("CountContainer/Count");
        var font = PreloadManager.Cache.GetAsset<Font>("res://themes/kreon_bold_glyph_space_one.tres");
        countLabel.AddThemeFontOverride(ThemeConstants.Label.Font, font);
        countLabel.MinFontSize = 20;
        countLabel.MaxFontSize = 26;

        return spellPileButton;
    });

    public override void _Ready()
    {
        // Do NOT call base._Ready() — base explicitly forbids it and throws if you do.
        ConnectSignals();
        _emptyPileMessage = new LocString("combat_messages", "OPEN_EMPTY_SPELL_PILE"); // protected, fine
        Visible = false;
        SetAnimInOutPositions();
        Disable();
    }

    protected override void ConnectSignals()
    {
        base.ConnectSignals(); // sets up base's own private _icon/_countLabel + NButton wiring
        _spellIcon = GetNode<Godot.Control>("Icon"); // our own reference to the same node
    }

    protected override void SetAnimInOutPositions()
    {
        _showPosition = Position;      // protected, fine
        _hidePosition = Position + new Vector2(HideOffsetX, 0f); // protected, fine
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player); // wires base's count label + click-to-open behavior automatically

        _owner = player;
        _spellPile = Pile.GetPile(player);
        _spellPile.ContentsChanged += HandleContentsChanged;

        if (_spellPile.Cards.Count <= 0) return;
        Visible = true;
        Enable();
        CreateCardPreview();
    }

    private void HandleContentsChanged()
    {
        var count = _spellPile?.Cards.Count ?? 0;

        if (count > 0 && Visible)
        {
            UpdateCardPreview();
        }
        else if (count > 0 && !Visible)
        {
            AnimIn();
            Enable();
            CreateCardPreview();
        }
        else if (count == 0 && Visible)
        {
            RemoveCardPreview();
        }
    }

    private void CreateCardPreview()
    {
        if (_spellPile == null || _spellPile.Cards.Count == 0 || !WizardConfig.ShowSpellPileCardStack)
            return;

        var count = Math.Min(MaxPreviewCards, _spellPile.Cards.Count);
        for (var i = 0; i < count; i++)
        {
            var isTop = i == 0;
            var xOffset = PreviewXOffset + i * PreviewSpacingX;
            var scale = PreviewDefaultScale * (1f - i * PreviewScaleDecrement);
            var holder = CreatePreviewCard(_spellPile.Cards[i], isTop, xOffset, scale);
            if (holder != null)
                _previewHolders.Add(holder);
        }
    }

    private NPreviewCardHolder? CreatePreviewCard(CardModel card, bool isTop, float xOffset, float scale)
    {
        var cardNode = NCard.Create(card);
        if (cardNode == null) return null;

        var holder = NPreviewCardHolder.Create(cardNode, showHoverTips: isTop, scaleOnHover: false);
        if (holder == null) return null;

        AddChild(holder);
        MoveChild(holder, 0);
        holder.MouseFilter = Control.MouseFilterEnum.Pass;
        holder.FocusMode = Control.FocusModeEnum.None;
        holder.Hitbox.MouseFilter = Control.MouseFilterEnum.Pass;

        PositionPreviewCard(holder, xOffset, scale);
        cardNode.UpdateVisuals(SpellCardPile.SpellPileType, CardPreviewMode.Normal);

        return holder;
    }

    private void PositionPreviewCard(NPreviewCardHolder holder, float xOffset, float scale)
    {
        holder.SetCardScale(new Vector2(scale, scale));
        holder.GlobalPosition = GlobalPosition + new Vector2(xOffset, PreviewYOffset);
    }

    private void UpdateCardPreview()
    {
        RemoveCardPreview();
        CreateCardPreview();
    }

    private void RemoveCardPreview()
    {
        foreach (var holder in _previewHolders)
            holder.QueueFree();
        _previewHolders.Clear();
    }

    // Fully overridden, NOT calling base.OnFocus() — base throws on any PileType it doesn't
    // explicitly recognize (Draw/Discard/Exhaust only).
    protected override void OnFocus()
    {
        NHoverTipSet.Remove(this);
        var hoverTip = new HoverTip(
            new LocString("static_hover_tips", "SPELL_PILE.title"),
            new LocString("static_hover_tips", "SPELL_PILE.description"));
        var tooltip = NHoverTipSet.CreateAndShow(this, hoverTip);
        if (tooltip != null)
        {
            var yOffset = _previewHolders.Count > 0 ? TooltipOffsetY : -220f;
            tooltip.GlobalPosition = GlobalPosition + new Vector2(0, yOffset);
        }

        _spellBumpTween?.Kill();
        _spellBumpTween = CreateTween();
        _spellBumpTween.TweenProperty(_spellIcon, "scale", new Vector2(1.25f, 1.25f), 0.05);
        TweenPreviewCards(PreviewHoverShiftX);
    }

    protected override void OnUnfocus()
    {
        // Also skip base.OnUnfocus() to stay consistent — it references base's private _icon.
        NHoverTipSet.Remove(this);
        _spellBumpTween?.Kill();
        _spellBumpTween = CreateTween().SetParallel();
        _spellBumpTween.TweenProperty(_spellIcon, "scale", Vector2.One, 0.5)
            .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
        TweenPreviewCards(0f);
    }

    private void TweenPreviewCards(float targetShiftX)
    {
        if (_previewHolders.Count == 0) return;
        _previewTween?.Kill();
        _previewTween = CreateTween();
        _previewTween.SetParallel();
        for (var i = 0; i < _previewHolders.Count; i++)
        {
            var xOffset = PreviewXOffset + i * PreviewSpacingX + targetShiftX;
            var targetPos = GlobalPosition + new Vector2(xOffset, PreviewYOffset);
            _previewTween.TweenProperty(_previewHolders[i], "global_position", targetPos, 0.1);
        }
    }
}