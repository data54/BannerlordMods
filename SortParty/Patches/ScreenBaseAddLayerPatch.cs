using HarmonyLib;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace PartyManager.Patches
{
    [HarmonyPatch(typeof(ScreenBase))]
    public class ScreenBaseAddLayerPatch
    {
        [HarmonyPatch("AddLayer")]
        public static void Postfix(ScreenBase __instance)
        {
            if (__instance is GauntletPartyScreen screen && !PartyController.CurrentInstance.WidgetsAdded)
            {
                PartyController.AddPartyWidgets(screen);
            }
        }

    }
}
