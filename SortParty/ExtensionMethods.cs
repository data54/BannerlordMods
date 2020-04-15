﻿using SandBox.GauntletUI;
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
            return GenericHelpers.GetPrivateField<PartyVM, GauntletPartyScreen>(partyScreen, "_dataSource");
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


    }
}
