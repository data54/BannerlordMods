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
            SortPartyHelpers.SortPartyScreen(__instance);
            
        }


    }
}
