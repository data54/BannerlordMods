using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;

namespace PartyManager
{
    internal static class ExtensionMethods
    {
        public static PartyVM GetPartyVM(this GauntletPartyScreen partyScreen)
        {
            return GenericHelpers.GetPrivateField<PartyVM, GauntletPartyScreen>(partyScreen, "_dataSource");
        }

        public static GauntletLayer GetGauntletLayer(this GauntletPartyScreen partyScreen)
        {
            return GenericHelpers.GetPrivateField<GauntletLayer, GauntletPartyScreen>(partyScreen, "_gauntletLayer");
        }

        //PartyVM calls
        public static PartyScreenLogic GetPartyScreenLogic(this PartyVM partyVM)
        {
            return GenericHelpers.GetPrivateField<PartyScreenLogic, PartyVM>(partyVM, "_partyScreenLogic");
        }

        public static MethodInfo GetRefreshPartyInformationMethod(this PartyVM partyVM)
        {
            return GenericHelpers.GetPrivateMethod("RefreshPartyInformation", partyVM);
        }

        public static MethodInfo GetInitializeTroopListsMethod(this PartyVM partyVM)
        {
            return GenericHelpers.GetPrivateMethod("InitializeTroopLists", partyVM);
        }

        public static void UpdateBrushesPublic(this Widget widget, float dt)
        {
            try
            {
                GenericHelpers.GetPrivateMethod<Widget>("UpdateBrushes", widget)?.Invoke(widget, new object[] { dt });
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("UpdateBrushesPublic", ex);
            }
        }
    }
}
