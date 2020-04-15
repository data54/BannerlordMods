using HarmonyLib;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using SortParty;
using TaleWorlds.CampaignSystem.ViewModelCollection;

namespace SortParty.Patches
{
    [HarmonyPatch(typeof(PartyVM), "RefreshValues")]
    public class PartyVMRefreshValuesPatch
    {
        static bool Prefix(PartyVM __instance)
        {
            if (SortPartySettings.Settings.EnableAutoSort)
            {
                GenericHelpers.LogDebug("PartyVM RefreshValues Patch", "Pre Update called");
                PartyController.CurrentInstance.PartyVM = __instance;
                PartyController.CurrentInstance.SortPartyScreen(false, true);
            }

            return true;
        }
    }
}
