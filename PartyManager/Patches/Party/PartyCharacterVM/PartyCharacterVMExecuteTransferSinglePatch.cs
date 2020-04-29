using HarmonyLib;
using SandBox.GauntletUI;
using TaleWorlds.Engine.Screens;

namespace PartyManager.Patches.Party.PartyCharacterVM
{
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.ViewModelCollection.PartyCharacterVM), "ExecuteTransferSingle")]
    public class PartyCharacterVMExecuteTransferSinglePatch
    {
        public static bool Prefix(ref TaleWorlds.CampaignSystem.ViewModelCollection.PartyCharacterVM __instance)
        {
            if (ScreenManager.TopScreen is GauntletPartyScreen topScreen)
            {
                if (topScreen.DebugInput.IsAltDown())
                {
                    PartyController.CurrentInstance.UpdateBlackWhiteList(__instance, BlackWhiteListType.PrisonerTransfer);
                    return false;
                }
                return true;
            }

            return true;
        }

    }
}
