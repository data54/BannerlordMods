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
        //internal static GauntletLayer screenLayer;
        //[HarmonyPatch("AddLayer")]
        //public static void Postfix(ref ScreenBase __instance)
        //{
        //    if (SortPartySettings.Settings.EnableUIChanges)
        //    {
        //        if (screenLayer == null && PartyController.CurrentInstance.PartyScreen == null)
        //        {
        //        screenLayer = new GauntletLayer(100, "GauntletLayer");
        //            __instance.RemoveLayer((ScreenLayer)screenLayer);
        //        }
        //    }
        //}

        //[HarmonyPatch("RemoveLayer")]
        //public static void Prefix(ref ScreenBase __instance)
        //{
        //    if (screenLayer != null && PartyController.CurrentInstance.PartyScreen == null)
        //    {
        //        __instance.RemoveLayer((ScreenLayer)screenLayer);
        //    }
        //}

    }
}
