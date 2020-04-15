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

namespace SortParty.Patches
{
    [HarmonyPatch(typeof(PartyScreenLogic), "Initialize", new[] {typeof(PartyBase), typeof(MobileParty),
        typeof(bool), typeof(TextObject), typeof(int), typeof(TextObject)})]
    public class PartyScreenLogicInitializePatch
    {
        static void Postfix(PartyScreenLogic __instance,
      PartyBase leftParty,
      MobileParty ownerParty,
      bool isDismissMode,
      TextObject leftPartyName,
      int lefPartySizeLimit,
      TextObject header = null)
        {
            if (SortPartySettings.Settings.EnableAutoSort) //makes testing in debug easier since it won't require you restart the program, won't be patched otherwise
            {
                SortPartyHelpers.SortPartyScreen(__instance);
            }            
        }


    }
}
