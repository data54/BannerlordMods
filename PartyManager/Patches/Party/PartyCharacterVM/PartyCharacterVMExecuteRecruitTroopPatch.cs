using HarmonyLib;
using SandBox.GauntletUI;
using TaleWorlds.Engine.Screens;

namespace PartyManager.Patches.Party.PartyCharacterVM
{
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.ViewModelCollection.PartyCharacterVM), "ExecuteRecruitTroop")]
    public class PartyCharacterVMExecuteRecruitTroopPatch
    {
        public static bool Prefix(ref TaleWorlds.CampaignSystem.ViewModelCollection.PartyCharacterVM __instance)
        {
            if (ScreenManager.TopScreen is GauntletPartyScreen topScreen)
            {
                if (topScreen.DebugInput.IsShiftDown())
                {
                    PartyController.CurrentInstance.UpdateBlackWhiteList(__instance, BlackWhiteListType.Recruit);
                    return false;
                }
                return true;
            }

            return true;
        }

    }
}
