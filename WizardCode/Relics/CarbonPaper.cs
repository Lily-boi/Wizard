using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using Wizard.WizardCode.Commands;

namespace Wizard.WizardCode.Relics;

public class CarbonPaper() : WizardRelic, IAfterWizardSpellCreatedRelic
{
    public override RelicRarity Rarity => RelicRarity.Shop;

    public async Task AfterWizardSpellCreated(CardModel card)
    {
        if (card.Owner != Owner)
            return;

        Flash();
        await EtchCmd.EtchBonusGeneratedCopy(card);
    }
}
