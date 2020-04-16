//using System;
//using HarmonyLib;
//using SandBox.GauntletUI;
//using TaleWorlds.Core;
//using TaleWorlds.Engine;
//using TaleWorlds.Engine.GauntletUI;
//using TaleWorlds.Engine.Screens;
//using TaleWorlds.Library;


//namespace PartyManager.Patches
//{
//    [HarmonyPatch(typeof(ScreenBase), "AddLayer")] //, new Type[0]
//    public class GauntletPartyScreenAddWidgetsPatch
//    {
//        static void Postfix(ScreenBase __instance, ScreenLayer layer)
//        {
//            if (__instance is GauntletPartyScreen screen)
//            {
//                PartyController.AddPartyWidgets(screen);
//            }
//        }
//    }
//}
