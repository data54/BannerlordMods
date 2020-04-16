using SandBox;
using SandBox.GauntletUI;
using PartyManager.Widgets;
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
using TaleWorlds.GauntletUI.PrefabSystem;
using TaleWorlds.Library;

namespace PartyManager
{
    public class PartyController
    {
        public bool WidgetsAdded { get; set; }
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


        private GauntletLayer _gauntletLayer;
        private GauntletLayer GauntletLayer
        {
            get
            {
                if (_gauntletLayer == null)
                {
                    _gauntletLayer = PartyScreen?.GetGauntletLayer();
                }
                return _gauntletLayer;
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
                    _refreshPartyInformationMethod = PartyVM?.GetRefreshPartyInformationMethod();
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

        
        public  static void AddPartyWidgets(GauntletPartyScreen partyScreen)
        {
            try
            {
                var newLayer = new GauntletLayer(1, "GauntletLayer");

                newLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
                var partyVM = partyScreen.GetPartyVM();

                newLayer?.LoadMovie("PartyManager", partyVM);
                CurrentInstance.WidgetsAdded = true;
                partyScreen.AddLayer(newLayer);
            }
            catch (Exception ex)
            {

            }
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
            var upgrades = PartyVM.MainPartyTroops
                    .Where(x => !x.IsHero
                    && !(x.IsUpgrade1Exists && x.IsUpgrade2Exists)
                    && ((x.IsUpgrade1Available && !x.IsUpgrade1Insufficient) || (x.IsUpgrade2Available && !x.IsUpgrade2Insufficient))).ToList();

            if (upgrades.Count > 0)
            {
                foreach (var troop in upgrades)
                {
                    PartyScreenLogic.PartyCommand command = new PartyScreenLogic.PartyCommand();
                    if (troop.NumOfTarget1UpgradesAvailable > 0)
                    {
                        command.FillForUpgradeTroop(PartyScreenLogic.PartyRosterSide.Right, troop.Type, troop.Character, troop.NumOfTarget1UpgradesAvailable, PartyScreenLogic.PartyCommand.UpgradeTargetType.UpgradeTarget1);
                    }
                    else
                    {
                        command.FillForUpgradeTroop(PartyScreenLogic.PartyRosterSide.Right, troop.Type, troop.Character, troop.NumOfTarget2UpgradesAvailable, PartyScreenLogic.PartyCommand.UpgradeTargetType.UpgradeTarget2);
                    }
                    PartyScreenLogic.AddCommand(command);
                }

                SortPartyScreen();
            }
            else
            {
                GenericHelpers.LogMessage("No troops found with only a single upgrade path");
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


        public void TriggerGauntletViewOnEventFired(GauntletView view)
        {
            //var refreshCall = GenericHelpers.GetPrivateMethod("OnEventFired", view);
            //refreshCall.Invoke(view, new[] { view.Target });
        }

    }
}
