using BaseLib.Abstracts;
using BaseLib.Utils;
using Wizard.WizardCode.Character;

namespace Wizard.WizardCode.Potions;

[Pool(typeof(WizardPotionPool))]
public abstract class WizardPotion : CustomPotionModel;