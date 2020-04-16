using HarmonyLib;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;

namespace PartyManager.Patches
{
    [HarmonyPatch(typeof(GauntletLayer), "LoadMovie")] //, new Type[0]
    public class GauntletPartyScreenAddWidgetsPatch
    {
        static bool Prefix(GauntletLayer __instance, string movieName, ViewModel dataSource)
        {
            return true;
        }
        static void Postfix(GauntletLayer __instance, string movieName, ViewModel dataSource)
        {
            if (movieName == "PartyScreen")
            {
                PartyController.AddPartyWidgets(__instance);
            }
        }
    }
}
