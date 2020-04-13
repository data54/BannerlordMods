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
    [HarmonyPatch(typeof(PartyScreenLogic), "ProcessCommand")]
    public class PartyScreenLogicProcessCommandPatch
    {
        static void Postfix(PartyScreenLogic __instance, PartyScreenLogic.PartyCommand command)
        {
            //bool updateUI = false;
            //switch (command.Code)
            //{
            //    case PartyScreenLogic.PartyCommandCode.TransferTroop:
            //        SortPartyHelpers.SortPartyScreen(__instance, true, true, true, true);
            //        updateUI = true;
            //        break;
            //    case PartyScreenLogic.PartyCommandCode.UpgradeTroop:
            //        SortPartyHelpers.SortPartyScreen(__instance, true, false, true, false);
            //        updateUI = true;
            //        break;
            //    case PartyScreenLogic.PartyCommandCode.ShiftTroop:
            //        //Drag and dropping
            //        break;
            //    case PartyScreenLogic.PartyCommandCode.RecruitTroop:
            //        //Move prisoner/troop from one side to the other
            //        SortPartyHelpers.SortPartyScreen(__instance, true, false, true, false);
            //        updateUI = true;
            //        break;
            //}

            //if (updateUI && __instance.UpdateDelegate != null)
            //{
            //    PartyScreenLogic.PresentationUpdate update = __instance.UpdateDelegate;
            //    __instance.UpdateDelegate(command);
            //}

            //InformationManager.DisplayMessage(new InformationMessage($"Sort executed: {command.Code.ToString()}"));
        }
    }
}
