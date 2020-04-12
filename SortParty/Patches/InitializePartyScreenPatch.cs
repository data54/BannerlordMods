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
    public class InitializePartyScreenPatch
    {
        static void Postfix(PartyScreenLogic __instance,
      PartyBase leftParty,
      MobileParty ownerParty,
      bool isDismissMode,
      TextObject leftPartyName,
      int lefPartySizeLimit,
      TextObject header = null)
        {
            try
            {
                //Sort Troops
                SortPartyHelpers.SortUnits(__instance.MemberRosters[0]);
                SortPartyHelpers.SortUnits(__instance.MemberRosters[1]);

                //Sort Prisoners
                SortPartyHelpers.SortUnits(__instance.PrisonerRosters[0]);
                SortPartyHelpers.SortUnits(__instance.PrisonerRosters[1]);
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Error in TroopOrder: {ex.Message}"));
            }
        }


    }
}
