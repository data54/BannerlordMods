using System;
using HarmonyLib;
using PartyManager.Settings;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;

namespace PartyManager.Patches
{
    [HarmonyPatch(typeof(PartyVM), "UpdateCurrentCharacterFormationClass")]
    public class PartyVMUpdateCurrentCharacterFormationPatch
    {
        static bool Prefix(PartyVM __instance, SelectorVM<SelectorItemVM> s)
        {
            try
            {
                if (!__instance.CurrentCharacter.IsPrisoner && s.SelectedIndex != (int)__instance.CurrentCharacter.Character.CurrentFormationClass)
                {
                    var newFormation = (FormationClass)s.SelectedIndex;
                    var name = __instance.CurrentCharacter.Character.Name.ToString();
                    var savedFormation = new SavedFormation() { TroopName = name, Formation = newFormation };
                    PartyManagerSettings.Settings.SavedFormations[name] = savedFormation;
                    PartyManagerSettings.Settings.SaveSettings();
                    GenericHelpers.LogDebug("PartyVMUpdateCurrentCharacterFormationPatch", $"{name}:{newFormation}");
                }
            }
            catch (Exception e)
            {
                GenericHelpers.LogException("PartyVMUpdateCurrentCharacterFormationPatch", e);
            }

            return true;
        }
    }
}
