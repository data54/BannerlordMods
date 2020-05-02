using System;
using System.Linq;
using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace PartyManager.Patches
{
    [HarmonyPatch(typeof(PartyVM), "RefreshPartyInformation")]
    public class PartyVMRefreshPartyInformationPatch
    {
        static void Postfix(PartyVM __instance)
        {
            if (!PartyManagerSettings.Settings.DisableUpdatedTroopLabel)
            {
                try
                {
                    GenericHelpers.LogDebug("PartyVM RefreshPartyInformation Patch", "Post called");
                    var logic = __instance.GetPartyScreenLogic();

                    if (logic == null)
                    {
                        GenericHelpers.LogDebug("RefreshPartyInformation.Postfix", "Logic null, skipping party label update");
                        return;
                    }

                    if (__instance.MainPartyTroops != null  && logic?.RightOwnerParty?.PartySizeLimit != null)
                    {
                        __instance.MainPartyTroopsLbl = GetPMPartyListLabel(__instance.MainPartyTroopsLbl, __instance.MainPartyTroops, logic.RightOwnerParty.PartySizeLimit);
                    }

                    if (__instance.OtherPartyTroops != null && !string.IsNullOrEmpty(logic?.LeftPartyName.ToString()) && logic?.LeftPartyLeader!=null && logic?.LeftOwnerParty?.PartySizeLimit != null)
                    {
                        __instance.OtherPartyTroopsLbl = GetPMPartyListLabel(__instance.OtherPartyTroopsLbl, __instance.OtherPartyTroops, logic.LeftOwnerParty.PartySizeLimit);
                    }
                }
                catch (Exception e)
                {
                    GenericHelpers.LogException("updateTroopLabelsPatch", e);
                }

            }
        }


        static string GetPMPartyListLabel(string initialValue,
            MBBindingList<PartyCharacterVM> partyList,
            int limit = 0)
        {
            int unwoundedCount = partyList.Sum<PartyCharacterVM>((Func<PartyCharacterVM, int>)(item => Math.Max(0, item.Number - item.WoundedCount)));
            int woundedCount = partyList.Sum<PartyCharacterVM>((Func<PartyCharacterVM, int>)(item => item.Number < item.WoundedCount ? 0 : item.WoundedCount));
            if (limit != 0 && woundedCount > 0)
            {
                return $"({unwoundedCount} + {woundedCount}w [{partyList.Sum(x => x.Number)}] / {limit})";
            }
            return initialValue;
        }
    }
}
