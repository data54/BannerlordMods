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
using TaleWorlds.Engine.Screens;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;

namespace SortParty.Patches
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
