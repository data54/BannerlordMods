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
                    return PartyController.CurrentInstance.UpdateBlackWhiteList(__instance, BlackWhiteListType.Transfer);
                }
                else if (topScreen.DebugInput.IsShiftDown() && topScreen.DebugInput.IsControlDown())
                {
                    if (!PartyManagerSettings.Settings.DisableCtrlShiftTransfer)
                    {
                        return PartyController.CurrentInstance.TransferUnits(__instance, PMTransferType.Half);
                    }
                }
                else if (topScreen.DebugInput.IsControlDown())
                {
                    if (!PartyManagerSettings.Settings.DisableCtrlTransfer)
                    {
                        return PartyController.CurrentInstance.TransferUnits(__instance, PMTransferType.All);
                    }
                }
                else if (topScreen.DebugInput.IsShiftDown())
                {
                    if (!PartyManagerSettings.Settings.DisableCustomShiftTransfer)
                    {
                        return PartyController.CurrentInstance.TransferUnits(__instance, PMTransferType.Custom);
                    }
                }


                return true;
            }

            return true;
        }

    }
}
