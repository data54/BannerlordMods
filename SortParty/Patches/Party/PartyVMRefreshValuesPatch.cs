using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection;

namespace PartyManager.Patches
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
