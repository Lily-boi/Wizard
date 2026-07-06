using BaseLib.Abstracts;
using Wizard.WizardCode.Extensions;
using Godot;

namespace Wizard.WizardCode.Character;

public class WizardRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Wizard.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}