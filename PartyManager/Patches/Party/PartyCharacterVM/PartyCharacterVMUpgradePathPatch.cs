using HarmonyLib;
using SandBox.GauntletUI;
using TaleWorlds.Engine.Screens;

namespace PartyManager.Patches.Party.PartyCharacterVM
{
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.ViewModelCollection.PartyCharacterVM), "Upgrade")]
    public class PartyCharacterVMUpgradePathPatch
    {
        public static bool Prefix(int upgradeIndex, ref TaleWorlds.CampaignSystem.ViewModelCollection.PartyCharacterVM __instance)
        {
            if (!PartyManagerSettings.Settings.DisableCustomUpgradePaths && (ScreenManager.TopScreen is GauntletPartyScreen topScreen))
            {
                if (topScreen.DebugInput.IsControlDown() && topScreen.DebugInput.IsShiftDown())
                {
                    PartyController.ToggleUpgradePath(__instance, upgradeIndex,true);
                    return false;
                }
                else if (topScreen.DebugInput.IsControlDown())
                {
                    PartyController.ToggleUpgradePath(__instance, upgradeIndex, false);
                    return false;
                }
                return true;
            }

            return true;
        }

    }
}
