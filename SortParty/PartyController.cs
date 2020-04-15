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
    public class PartyController
    {
        static PartyController _partyController;
        public static PartyController CurrentInstance
        {
            get
            {
                if (_partyController == null)
                {
                    _partyController = new PartyController();
                }
                return _partyController;
            }
        }

        public GauntletPartyScreen PartyScreen { get; set; }
        public PartyVM PartyVM { get; set; }
        public PartyScreenLogic PartyScreenLogic { get; set; }
        #region Methods
        private MethodInfo RefreshPartyInformationMethod { get; set; }
        private MethodInfo InitializeTroopListsMethod { get; set; }
        #endregion Methods

        public bool Validate()
        {
            return !( PartyScreen == null 
                || PartyVM == null 
                || PartyScreenLogic == null 
                || RefreshPartyInformationMethod == null 
                || InitializeTroopListsMethod == null);
        }


        public PartyController()
        {
            PartyScreen = ScreenManager.TopScreen as GauntletPartyScreen;

            PartyVM = PartyScreen?.GetPartyVM();
            PartyScreenLogic = PartyVM?.GetPartyScreenLogic();
            RefreshPartyInformationMethod = PartyVM?.GetRefreshPartyInformationMethod();
            InitializeTroopListsMethod = PartyVM.GetInitializeTroopListsMethod();

        }

        public void SortPartyScreen(bool sortRecruitUpgrade = false)
        {
            try
            {
                if (!Validate()) return;

                //Left Side
                SortPartyHelpers.SortUnits(PartyScreenLogic.MemberRosters[0], sortRecruitUpgrade, PartyVM.OtherPartyTroops);
                SortPartyHelpers.SortUnits(PartyScreenLogic.PrisonerRosters[0], sortRecruitUpgrade, PartyVM.OtherPartyPrisoners);
                //Right Side
                SortPartyHelpers.SortUnits(PartyScreenLogic.MemberRosters[1], sortRecruitUpgrade, PartyVM.MainPartyTroops);
                SortPartyHelpers.SortUnits(PartyScreenLogic.PrisonerRosters[1], sortRecruitUpgrade, PartyVM.MainPartyPrisoners);

                InitializeTroopLists();
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("SortPartyScreen", ex);
            }
        }

        public void InitializeTroopLists()
        {
            InitializeTroopListsMethod.Invoke(PartyVM, new object[0] { });
        }

        public void RefreshPartyInformation()
        {
            RefreshPartyInformationMethod.Invoke(PartyVM, new object[0] { });
        }

    }
}
