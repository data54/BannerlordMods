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
            if (ScreenManager.TopScreen is GauntletPartyScreen topScreen)
            {
                if (PartyManagerSettings.Settings.DisableCustomUpgradePaths && topScreen.DebugInput.IsControlDown() && topScreen.DebugInput.IsShiftDown())
                {
                    PartyController.ToggleUpgradePath(__instance, upgradeIndex, true);
                    return false;
                }
                else if (PartyManagerSettings.Settings.DisableCustomUpgradePaths && topScreen.DebugInput.IsControlDown())
                {
                    PartyController.ToggleUpgradePath(__instance, upgradeIndex, false);
                    return false;
                }
                else if (topScreen.DebugInput.IsShiftDown())
                {
                    PartyController.CurrentInstance.UpdateBlackWhiteList(__instance, BlackWhiteListType.Upgrade);
                    return false;
                }
                return true;
            }

            return true;
        }

    }
}
