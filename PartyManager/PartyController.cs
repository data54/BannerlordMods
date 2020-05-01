using SandBox;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PartyManager.ViewModel;
using PartyManager.ViewModels;
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
                    var newLayer = new GauntletLayer(100, "GauntletLayer");


                    newLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);

                    var partyManagerModVm = new PartyManagerVM(partyScreen.GetPartyVM(), partyScreen.GetPartyVM()?.GetPartyScreenLogic(), partyScreen);

                    newLayer?.LoadMovie("PartyManagerModScreen", (TaleWorlds.Library.ViewModel)partyManagerModVm);
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

        public void UpgradeAllTroops(bool customOnly = false)
        {
            var currentlyUpgradingUnit = "";
            try
            {
                var upgrades = PartyVM?.MainPartyTroops?
                    .Where(x => !x.IsHero
                                && ((x.IsUpgrade1Available && !x.IsUpgrade1Insufficient) || (x.IsUpgrade2Available && !x.IsUpgrade2Insufficient))).ToList();

                var upgradesCount = 0;

                if (!PartyManagerSettings.Settings.DisableCustomUpgradePaths)

                {
                    var splitTroops = PartyManagerSettings.Settings.SavedTroopUpgradePaths.Where(x => x.EvenSplit).Select(x => x.UnitName).ToList();
                    var customTroops = PartyManagerSettings.Settings.SavedTroopUpgradePaths.Where(x => !x.EvenSplit).Select(x => x.UnitName).ToList();
                    var splitUpgrades = upgrades.Where(x => splitTroops.Contains(x.Name.ToString())).ToList();
                    var customUpgrades = upgrades.Where(x => customTroops.Contains(x.Name.ToString())).ToList();
                    upgrades = upgrades.Where(x => !splitTroops.Contains(x.Name.ToString()) && !customTroops.Contains(x.Name.ToString())).ToList();

                    foreach (var troop in splitUpgrades)
                    {
                        currentlyUpgradingUnit = troop?.Character?.Name?.ToString();
                        var unitUpgrades = Math.Min(troop.NumOfTarget1UpgradesAvailable,
                            troop.NumOfTarget2UpgradesAvailable) / 2;
                        GenericHelpers.LogDebug("UpgradeAllTroops", $"Split {troop.Name.ToString()}: {unitUpgrades}");

                        var unitsUpgraded = false;

                        for (int i = 0; i < unitUpgrades; i++)
                        {
                            if (troop.IsUpgrade1Insufficient || troop.IsUpgrade2Insufficient || !troop.IsUpgrade1Available || !troop.IsUpgrade2Available)
                            {
                                break;
                            }

                            PartyScreenLogic.PartyCommand command1 = new PartyScreenLogic.PartyCommand();
                            PartyScreenLogic.PartyCommand command2 = new PartyScreenLogic.PartyCommand();
                            command1.FillForUpgradeTroop(PartyScreenLogic.PartyRosterSide.Right, troop.Type, troop.Character, 1,
                                PartyScreenLogic.PartyCommand.UpgradeTargetType.UpgradeTarget1);
                            var c1Valid = PartyScreenLogic.ValidateCommand(command1);
                            PartyScreenLogic.AddCommand(command1);


                            if (troop.IsUpgrade2Insufficient)
                            {
                                GenericHelpers.LogDebug("UpgradeAllTroops", $"Upgrade2 Insufficient");
                                break;
                            }

                            command2.FillForUpgradeTroop(PartyScreenLogic.PartyRosterSide.Right, troop.Type, troop.Character, 1,
                                PartyScreenLogic.PartyCommand.UpgradeTargetType.UpgradeTarget2);
                            var c2Valid = PartyScreenLogic.ValidateCommand(command1);
                            PartyScreenLogic.AddCommand(command2);
                            unitsUpgraded = true;
                        }



                        if (unitsUpgraded)
                        {
                            upgradesCount++;
                        }
                    }

                    foreach (var troop in customUpgrades)
                    {
                        currentlyUpgradingUnit = troop?.Character?.Name?.ToString();
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

                    if (PartyManagerSettings.Settings.UpgradeTroopsUseWhitelist)
                    {
                        singleUpgrades = singleUpgrades.Where(x =>
                            PartyManagerSettings.Settings.UpgradeTroopsBlackWhiteList.Contains(x.Name)).ToList();
                    }
                    else
                    {
                        singleUpgrades = singleUpgrades.Where(x =>
                            !PartyManagerSettings.Settings.UpgradeTroopsBlackWhiteList.Contains(x.Name)).ToList();

                    }


                    foreach (var troop in singleUpgrades)
                    {
                        currentlyUpgradingUnit = troop?.Character?.Name?.ToString();
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
            catch (Exception ex)
            {
                GenericHelpers.LogException($"UpgradeAllTroops unittype({currentlyUpgradingUnit})", ex);
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
            var currentlyRecruitingPrisoner = "";
            try
            {
                var recruits = PartyVM?.MainPartyPrisoners?
                    .Where(x => !x.IsHero
                                && x.IsRecruitablePrisoner && x.NumOfRecruitablePrisoners > 0).ToList();


                if (PartyManagerSettings.Settings.RecruitPrisonersUseWhitelist)
                {
                    recruits = recruits.Where(x =>
                        PartyManagerSettings.Settings.RecruitPrisonerBlackWhiteList.Contains(x.Name)).ToList();
                }
                else
                {
                    recruits = recruits.Where(x =>
                        !PartyManagerSettings.Settings.RecruitPrisonerBlackWhiteList.Contains(x.Name)).ToList();
                }

                if (recruits?.Count > 0)
                {
                    var freeUnitSlots = PartyScreenLogic.RightOwnerParty.PartySizeLimit - PartyScreenLogic.RightOwnerParty.NumberOfAllMembers;

                    var recruitCount = 0;

                    foreach (var troop in recruits)
                    {
                        currentlyRecruitingPrisoner = troop?.Name?.ToString();
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
                GenericHelpers.LogException($"RecruitAllPrisoners UnitType({currentlyRecruitingPrisoner})", ex);
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

        public bool UpdateBlackWhiteList(PartyCharacterVM character, BlackWhiteListType listType)
        {
            try
            {
                List<string> targetList = null;
                var unitName = character.Name;
                var listName = "";

                if (listType==BlackWhiteListType.Transfer && character.IsPrisoner && PartyScreenLogic.PrisonerTransferState==PartyScreenLogic.TransferState.TransferableWithTrade)
                {
                    targetList = PartyManagerSettings.Settings.RansomPrisonerBlackWhiteList;
                    listName = "sell prisoners";
                }
                else if (listType == BlackWhiteListType.Transfer && character.IsPrisoner && PartyScreenLogic.PrisonerTransferState == PartyScreenLogic.TransferState.Transferable)
                {
                    targetList = PartyManagerSettings.Settings.TransferPrisonerBlackWhiteList;
                    listName = "transfer prisoners";
                }
                else if (listType == BlackWhiteListType.Transfer && !character.IsPrisoner)
                {
                    //targetList = PartyManagerSettings.Settings.TransferTroopsBlackWhiteList;
                    //listName = "transfer troops";
                }
                else if (listType == BlackWhiteListType.Recruit)
                {
                    targetList = PartyManagerSettings.Settings.RecruitPrisonerBlackWhiteList;
                    listName = "recruit prisoners";
                }
                else if (listType == BlackWhiteListType.Upgrade)
                {
                    targetList = PartyManagerSettings.Settings.UpgradeTroopsBlackWhiteList;
                    listName = "upgrade troops";
                }

                if (targetList == null)
                {
                    return true;
                }

                if (targetList.Contains(unitName))
                {
                    GenericHelpers.LogMessage($"Removed {unitName} from {listName} filter list");
                    targetList.Remove(unitName);
                }
                else
                {
                    GenericHelpers.LogMessage($"Added {unitName} to {listName} filter list");
                    targetList.Add(unitName);
                }

                PartyManagerSettings.Settings.SaveSettings();
                return false;
            }
            catch (Exception e)
            {
                GenericHelpers.LogException("UpdateBlackWhiteList", e);
            }

            return true;
        }

        public static void ToggleUpgradePath(PartyCharacterVM vm, int upgradeIndex, bool split = false)
        {
            try
            {
                split = split && vm.IsUpgrade1Exists && vm.IsUpgrade2Exists;

                string message = "";
                var upgrade = PartyManagerSettings.Settings.SavedTroopUpgradePaths.FirstOrDefault(x => x.UnitName == vm.Character.Name.ToString());

                if (upgrade != null)
                {

                    if ((upgradeIndex == upgrade.TargetUpgrade && (split == upgrade.EvenSplit)) || (split && upgrade.EvenSplit))
                    {
                        PartyManagerSettings.Settings.SavedTroopUpgradePaths.Remove(upgrade);
                        message = $"Removed Upgrade Path for {vm.Character.Name.ToString()}";
                    }
                    else if (split)
                    {
                        upgrade.EvenSplit = true;
                        message = $"Changed Upgrade Path for {vm.Character.Name.ToString()} to be split evenly";
                    }
                    else
                    {
                        upgrade.EvenSplit = false;
                        upgrade.TargetUpgrade = upgradeIndex;
                        message = $"Changed Upgrade Path for {vm.Character.Name.ToString()}";
                    }
                }
                else
                {
                    var newUpgrade = new SavedTroopUpgradePath()
                    {
                        UnitName = vm.Name,
                        TargetUpgrade = upgradeIndex,
                        EvenSplit = split
                    };

                    PartyManagerSettings.Settings.SavedTroopUpgradePaths.Add(newUpgrade);
                    if (split)
                    {
                        message = $"Added Split Upgrade Path for {vm.Character.Name.ToString()}";
                    }
                    else
                    {
                        message = $"Added Upgrade Path for {vm.Character.Name.ToString()}";
                    }
                }
                PartyManagerSettings.Settings.SaveSettings();
                GenericHelpers.LogMessage(message);
            }
            catch (Exception e)
            {
                GenericHelpers.LogException("UpgradeAllTroops", e);
            }
        }

        private static GauntletLayer settingsLayer;
        public static void OpenSettings()
        {
            try
            {
                if (settingsLayer == null || !settingsLayer.IsActive)
                {

                    var screen = ScreenManager.TopScreen;
                    settingsLayer = new GauntletLayer(10000, "GauntletLayer");
                    settingsLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
                    var settingsVm = new PartyManagerSettingsVM(screen, settingsLayer);
                    settingsLayer?.LoadMovie("PartyManagerSettings", settingsVm);
                    screen.AddLayer(settingsLayer);
                }
            }
            catch (Exception e)
            {
                GenericHelpers.LogException("OpenSettings", e);
            }
        }

        public bool TransferUnits(PartyCharacterVM troop, PMTransferType transferType)
        {
            try
            {
                if (troop == null || troop.Number == 0)
                {
                    return true;
                }

                int unitCount = 0;

                switch (transferType)
                {
                    case PMTransferType.All:
                        unitCount = troop.Number;
                        break;
                    case PMTransferType.Half:
                        unitCount = (int) Math.Round(troop.Number / 2f, MidpointRounding.AwayFromZero);
                        break;
                    case PMTransferType.Custom:
                        unitCount = Math.Min(troop.Number, PartyManagerSettings.Settings.CustomShiftTransferCount);
                        break;
                }

                if (unitCount == 0 || unitCount<0)
                {
                    return true;
                }

                troop.OnTransfer(troop, -1, unitCount,  troop.Side);
                PartyVM.ExecuteRemoveZeroCounts();
                return false;
            }
            catch (Exception e)
            {
                GenericHelpers.LogException("TransferUnits", e);
            }


            return true;
        }
    }

    public enum BlackWhiteListType
    {
        Transfer,
        Recruit,
        Upgrade
    }


    public enum PMTransferType
    {
        All,
        Half,
        Custom
    }
}
