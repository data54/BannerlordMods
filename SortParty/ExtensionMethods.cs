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
        public static void SortTroops(this PartyVM partyVm, bool updateUI = false)
        {
            try
            {
                var refreshPartyCall = partyVm.GetType().GetMethod("RefreshPartyInformation", BindingFlags.Instance | BindingFlags.NonPublic);

                if (refreshPartyCall == null) return;


                partyVm.MainPartyTroops = SortPartyHelpers.SortVMTroops(partyVm.MainPartyTroops);
                partyVm.MainPartyPrisoners = SortPartyHelpers.SortVMTroops(partyVm.MainPartyPrisoners);

                partyVm.OtherPartyTroops = SortPartyHelpers.SortVMTroops(partyVm.OtherPartyTroops);
                partyVm.OtherPartyPrisoners = SortPartyHelpers.SortVMTroops(partyVm.OtherPartyPrisoners);

                refreshPartyCall.Invoke(partyVm, new object[0] { });


                //var partyScreenLogicField = partyVm.GetType().GetField("_partyScreenLogic", BindingFlags.Instance | BindingFlags.NonPublic);

                //if(partyScreenLogicField!=null)
                //{
                //    var logic = partyScreenLogicField.GetValue(partyVm) as PartyScreenLogic;


                //}
            }
            catch(Exception ex)
            {
                SortPartyHelpers.LogException("ExtensionMethods.SortTroops", ex);
            }
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
                return field.GetValue((object)partyScreen) as PartyVM;
            }
            catch (Exception ex)
            {
                SortPartyHelpers.LogException("GetPartyVM", ex);
            }

            return null;
        }
    }
}
