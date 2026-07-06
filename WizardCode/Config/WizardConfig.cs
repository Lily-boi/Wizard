using BaseLib.Config;
using Godot;

namespace Wizard.WizardCode.Config;

[ConfigHoverTipsByDefault]
internal class WizardConfig : SimpleModConfig
{
    [ConfigSection("Wizard")]
    public static bool ShowSpellPileCardStack { get; set; } = true;
}