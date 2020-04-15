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
                if (_partyController == null || _partyController?.PartyScreen?.IsActive != true)
                {
                    _partyController = new PartyController();
                }
                return _partyController;
            }
        }

        private GauntletPartyScreen _partyScreen;
        public GauntletPartyScreen PartyScreen
        {
            get
            {
                if (_partyScreen == null)
                {
                    _partyScreen = ScreenManager.TopScreen as GauntletPartyScreen;
                }
                return _partyScreen;
            }
        }

        private PartyVM _partyVM;
        public PartyVM PartyVM
        {
            get
            {
                if (_partyVM == null)
                {
                    _partyVM = PartyScreen?.GetPartyVM();
                }
                return _partyVM;
            }
            set
            {
                _partyVM = value;
            }
        }

        private PartyScreenLogic _partyScreenLogic;
        private PartyScreenLogic PartyScreenLogic
        {
            get
            {
                if (_partyScreenLogic == null)
                {
                    _partyScreenLogic = PartyVM?.GetPartyScreenLogic();
                }
                return _partyScreenLogic;
            }
        }

        #region Methods

        private MethodInfo _refreshPartyInformationMethod;
        private MethodInfo RefreshPartyInformationMethod
        {
            get
            {
                if (_refreshPartyInformationMethod == null)
                {
                    _refreshPartyInformationMethod= PartyVM?.GetRefreshPartyInformationMethod();
                }
                return _refreshPartyInformationMethod;
            }
        }


        private MethodInfo _initializeTroopListsMethod;
        private MethodInfo InitializeTroopListsMethod
        {
            get
            {
                if (_initializeTroopListsMethod == null)
                {
                    _initializeTroopListsMethod = PartyVM?.GetInitializeTroopListsMethod();
                }
                return _initializeTroopListsMethod;
            }
        }

        #endregion Methods

        public bool Validate(bool includeUI)
        {
            return !(PartyScreen == null
                || PartyVM == null
                || PartyScreenLogic == null
                || (includeUI && (RefreshPartyInformationMethod == null || InitializeTroopListsMethod == null)));
        }


        public PartyController()
        {
            GenericHelpers.LogDebug("PartyController.Constructor", "Party Controller Generated");
        }

        public void SortPartyScreen(bool sortRecruitUpgrade = false,
            bool updateUI = true, bool rightTroops = true,
            bool rightPrisoners = true, bool leftTroops = true, bool leftPrisoners = true)
        {
            try
            {
                if (!Validate(updateUI))
                {
                    GenericHelpers.LogDebug("SortPartyScreen", "Sort validation failed");
                    return;
                }
                GenericHelpers.LogDebug("SortPartyScreen", "Sort Called");

                SortPartyHelpers.SortPartyLogic(PartyScreenLogic, PartyVM, sortRecruitUpgrade, rightTroops, rightPrisoners, leftTroops, leftPrisoners);

                if (updateUI) InitializeTroopLists();
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("SortPartyScreen", ex);
            }
        }

        public void UpgradeAllTroops()
        {

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
