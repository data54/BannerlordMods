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


        public static void AddPartyWidgets(GauntletPartyScreen partyScreen)
        {
            try
            {
                if (!PartyManagerSettings.Settings.HideUIWidgets)
                {
                    var newLayer = new GauntletLayer(1, "GauntletLayer");

                    newLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
                    var partyVM = partyScreen.GetPartyVM();

                    newLayer?.LoadMovie("PartyManagerModScreen", partyVM);
                    CurrentInstance.WidgetsAdded = true;
                    partyScreen.AddLayer(newLayer);
                    GenericHelpers.LogDebug("AddPartyWidgets", "Party Widget Added");
                }
                else
                {
                    GenericHelpers.LogDebug("AddPartyWidgets", "Skipped adding widgets");
                }
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("AddPartyWidgets", ex);
            }
        }

        public void SortPartyScreen(SortType sortType = SortType.Default,
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

                SortPartyHelpers.SortPartyLogic(PartyScreenLogic, PartyVM, sortType, rightTroops, rightPrisoners, leftTroops, leftPrisoners);

                if (updateUI) InitializeTroopLists();
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("SortPartyScreen", ex);
            }
        }

        public void SortCustomUpgradesToTop()
        {

        }

        public void UpgradeAllTroops(bool customOnly = false)
        {
            var upgrades = PartyVM?.MainPartyTroops?
                    .Where(x => !x.IsHero
                    && ((x.IsUpgrade1Available && !x.IsUpgrade1Insufficient) || (x.IsUpgrade2Available && !x.IsUpgrade2Insufficient))).ToList();

            var upgradesCount = 0;

            if (!PartyManagerSettings.Settings.DisableCustomUpgradePaths)

            {
                var customUpgradeTroopsNames = PartyManagerSettings.Settings.SavedTroopUpgradePaths.Select(x => x.UnitName).ToList();
                var customUpgrades = upgrades.Where(x => customUpgradeTroopsNames.Contains(x.Name.ToString()));

                foreach (var troop in customUpgrades)
                {
                    var upgradePath = PartyManagerSettings.Settings.SavedTroopUpgradePaths
                        .Where(x => x.UnitName == troop.Name.ToString())?.Select(x => x.TargetUpgrade).FirstOrDefault();

                    if (upgradePath == 0 && !troop.IsUpgrade1Insufficient)
                    {
                        UpgradeUnit(troop, true);
                        upgradesCount++;
                    }
                    else if (upgradePath == 1 && !troop.IsUpgrade2Insufficient)
                    {
                        UpgradeUnit(troop, false);
                        upgradesCount++;
                    }
                }
            }

            if (!customOnly)
            {
                //single upgrade units
                var singleUpgrades = upgrades.Where(x => !(x.IsUpgrade1Exists && x.IsUpgrade2Exists)).ToList();


                foreach (var troop in singleUpgrades)
                {
                    PartyScreenLogic.PartyCommand command = new PartyScreenLogic.PartyCommand();
                    if (troop.NumOfTarget1UpgradesAvailable > 0)
                    {
                        UpgradeUnit(troop, true);
                    }
                    else
                    {
                        UpgradeUnit(troop, false);
                    }

                    upgradesCount++;
                }
            }

            if (upgradesCount > 0)
            {
                GenericHelpers.LogDebug("UpgradeAllTroops", $"{upgradesCount} troops upgraded");
                ButtonClickRefresh(true, false);
            }
            else
            {
                GenericHelpers.LogMessage("No troops found to upgrade");
            }
        }

        private void UpgradeUnit(PartyCharacterVM troop, bool path1)
        {
            PartyScreenLogic.PartyCommand command = new PartyScreenLogic.PartyCommand();
            command.FillForUpgradeTroop(PartyScreenLogic.PartyRosterSide.Right, troop.Type, troop.Character,
                path1 ? troop.NumOfTarget1UpgradesAvailable : troop.NumOfTarget2UpgradesAvailable,
                path1 ? PartyScreenLogic.PartyCommand.UpgradeTargetType.UpgradeTarget1 : PartyScreenLogic.PartyCommand.UpgradeTargetType.UpgradeTarget2);
            PartyScreenLogic.AddCommand(command);
        }

        private void ButtonClickRefresh(bool rightTroops, bool rightPrisoners, bool leftTroops = false, bool leftPrisoners = false)
        {
            if (PartyManagerSettings.Settings.SortAfterRecruitAllUpgradeAllClick)
            {
                SortPartyScreen(SortType.Default, true, rightTroops, rightPrisoners, leftTroops, leftPrisoners);
            }
            else
            {
                InitializeTroopLists();
            }
        }

        public void RecruitAllPrisoners(bool ignorePartyLimit)
        {
            try
            {
                var recruits = PartyVM?.MainPartyPrisoners?
                    .Where(x => !x.IsHero
                                && x.IsRecruitablePrisoner && x.NumOfRecruitablePrisoners > 0).ToList();

                if (recruits?.Count > 0)
                {
                    var freeUnitSlots = PartyScreenLogic.RightOwnerParty.PartySizeLimit - PartyScreenLogic.RightOwnerParty.NumberOfAllMembers;

                    var recruitCount = 0;

                    foreach (var troop in recruits)
                    {
                        if (freeUnitSlots > 0 || ignorePartyLimit)
                        {

                            var toBeRecruited = ignorePartyLimit ? troop.NumOfRecruitablePrisoners : Math.Min(freeUnitSlots, troop.NumOfRecruitablePrisoners);
                            for (int i = 0; i < toBeRecruited; i++)
                            {
                                PartyScreenLogic.PartyCommand command = new PartyScreenLogic.PartyCommand();
                                command.FillForRecruitTroop(PartyScreenLogic.PartyRosterSide.Right, troop.Type,
                                    troop.Character, 1);
                                PartyScreenLogic.AddCommand(command);
                            }

                            GenericHelpers.LogDebug("RecruitAll", $"Recruited {toBeRecruited} {troop.Character.Name}");
                            recruitCount += toBeRecruited;
                            freeUnitSlots -= toBeRecruited;
                        }
                    }

                    ButtonClickRefresh(true, true);
                }
                else
                {
                    GenericHelpers.LogMessage("No prisoners found to recruit");
                }
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("RecruitAllPrisoners", ex);
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

        public static void ToggleUpgradePath(PartyCharacterVM vm, int upgradeIndex)
        {
            string message = "";
            var upgrade = PartyManagerSettings.Settings.SavedTroopUpgradePaths.FirstOrDefault(x => x.UnitName == vm.Character.Name.ToString());

            if (upgrade != null)
            {
                if (upgradeIndex == upgrade.TargetUpgrade)
                {
                    PartyManagerSettings.Settings.SavedTroopUpgradePaths.Remove(upgrade);
                    message = $"Removed Upgrade Path for {vm.Character.Name.ToString()}";
                }
                else
                {
                    upgrade.TargetUpgrade = upgradeIndex;
                    message = $"Changed Upgrade Path for {vm.Character.Name.ToString()}";
                }
            }
            else
            {
                var newUpgrade = new SavedTroopUpgradePath()
                {
                    UnitName = vm.Name,
                    TargetUpgrade = upgradeIndex
                };

                PartyManagerSettings.Settings.SavedTroopUpgradePaths.Add(newUpgrade);
                message = $"Added Upgrade Path for {vm.Character.Name.ToString()}";
            }
            PartyManagerSettings.Settings.SaveSettings();
            GenericHelpers.LogMessage(message);
        }

        public void TriggerGauntletViewOnEventFired(GauntletView view)
        {
            //var refreshCall = GenericHelpers.GetPrivateMethod("OnEventFired", view);
            //refreshCall.Invoke(view, new[] { view.Target });
        }

    }
}
