using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Engine.Screens;

namespace SortParty
{
    internal static class ExtensionMethods
    {
        public static void SortTroops(this PartyVM partyVm, bool sortRecruitUpgrade = false)
        {
            SortPartyHelpers.SortPartyScreen(partyVm, sortRecruitUpgrade);
        }

        public static PartyVM GetPartyVM(this GauntletPartyScreen partyScreen)
        {
            try
            {
                var field = partyScreen.GetType().GetField("_dataSource", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field == null)
                {
                    return null;
                }
                return field.GetValue(partyScreen) as PartyVM;
            }
            catch (Exception ex)
            {
                SortPartyHelpers.LogException("GetPartyVM", ex);
            }

            return null;
        }
    }
}
