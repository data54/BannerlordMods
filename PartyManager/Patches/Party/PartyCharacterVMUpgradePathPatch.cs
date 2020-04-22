using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace PartyManager.Patches.Party
{
    [HarmonyPatch(typeof(PartyCharacterVM), "Upgrade")]
    public class PartyCharacterVMUpgradePathPatch
    {
        public static bool Prefix(int upgradeIndex, ref PartyCharacterVM __instance)
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
