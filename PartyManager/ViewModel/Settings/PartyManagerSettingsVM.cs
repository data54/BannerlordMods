using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyManager.ViewModel;
using PartyManager.ViewModel.Settings;
using PartyManager.ViewModel.Settings.OptionVMS;
using TaleWorlds.CampaignSystem.SandBox.Issues;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Options;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;


namespace PartyManager.ViewModel
{
    public class PartyManagerSettingsVM :TaleWorlds.Library.ViewModel
    {
        private PMGenericOptionVM _miscController;
        private PMGenericOptionVM _customSortVM;


        [DataSourceProperty] 
        public String OptionsLbl { get; set; }
        [DataSourceProperty]
        public String CancelLbl { get; set; }
        [DataSourceProperty]
        public String DoneLbl { get; set; }

        [DataSourceProperty]
        public PMGenericOptionVM TogglesController
        {
            get => _miscController;
            set
            {
                if (value == this._miscController)
                    return;
                this._miscController = value;
                this.OnPropertyChanged(nameof(TogglesController));
            }
        }

        [DataSourceProperty]
        public PMGenericOptionVM CustomSortController
        {
            get => _customSortVM;
            set
            {
                if (value == this._customSortVM)
                    return;
                this._customSortVM = value;
                this.OnPropertyChanged(nameof(CustomSortController));
            }
        }
        
        private PartyManagerSettings _settings;
        [DataSourceProperty]
        public PartyManagerSettings Settings
        {
            get => _settings;
            set
            {
                if (value == this._settings)
                    return;
                this._settings = value;
                this.OnPropertyChanged(nameof(Settings));
            }
        }


        private bool _isInitialized = false;
        private GauntletLayer _screenLayer;
        private ScreenBase _parentScreen;
        public PartyManagerSettingsVM(ScreenBase parentScreen, GauntletLayer screenLayer)
        {
            //TaleWorlds.GauntletUI.ExtraWidgets.
            //TabToggleWidget
            _settings = PartyManagerSettings.Settings.Clone();
            _customSortVM= CreateSortOptions();
            _miscController= CreateMiscOptions();
            OptionsLbl = "Party Manager Settings";
            CancelLbl = "Cancel";
            DoneLbl = "Done";
            _parentScreen = parentScreen;
            _screenLayer = screenLayer;
            this.RefreshValues();
            _isInitialized = true;


        }

        public PMGenericOptionVM CreateMiscOptions()
        {
            var _options = new MBBindingList<IPMOptions>();

            _options.Add(new PMGenericOptionDataVM<bool>(_settings.UseAdvancedPartyComposition, "Show Advanced Party Composition Information", "Party Composition will attempt to show unit weapon type breakdown",
                b => { _settings.UseAdvancedPartyComposition = b; }, CampaignOptionItemVM.OptionTypes.Boolean));


            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisableCtrlTransfer, "Disable CTRL Transfer", "Disable the ability to transfer all units by clicking with CTRL held down",
                b => { _settings.DisableCtrlTransfer = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisableCtrlShiftTransfer, "Disable CTRL+SHIFT Transfer", "Disable the ability to transfer half of units by clicking with CTRL+SHIFT held down",
                b => { _settings.DisableCtrlShiftTransfer = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisableCustomShiftTransfer, "Disable Custom SHIFT Transfer", "Disable the ability to transfer custom unit count units by clicking with SHIFT held down",
                b => { _settings.DisableCustomShiftTransfer = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMNumericOptionData(_settings.CustomShiftTransferCount, "Custom SHIFT Transfer Unit Count", "Number of units to transfer when the button is clicked with SHIFT held down",
                b => { _settings.CustomShiftTransferCount = (int)b; }, CampaignOptionItemVM.OptionTypes.Numeric, 0, 500, true));

            _options.Add(new PMGenericOptionDataVM<bool>(_settings.UpgradeTroopsUseWhitelist, "Upgrade Troops Whitelist", "Use a whitelist instead of blacklist for the upgrade troops filter",
                b => { _settings.UpgradeTroopsUseWhitelist = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            //_options.Add(new PMGenericOptionDataVM<bool>(_settings.TransferTroopsUseWhitelist, "Transfer Troops Whitelist", "Use a whitelist instead of blacklist for the transfer troops filter",
            //    b => { _settings.TransferTroopsUseWhitelist = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.RecruitPrisonersUseWhitelist, "Recruit Prisoners Whitelist", "Use a whitelist instead of blacklist for the recruit prisoners filter",
                b => { _settings.RecruitPrisonersUseWhitelist = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            //_options.Add(new PMGenericOptionDataVM<bool>(_settings.TransferPrisonersUseWhitelist, "Transfer Prisoners Whitelist", "Use a whitelist instead of blacklist for the transfer prisoners filter",
            //    b => { _settings.TransferPrisonersUseWhitelist = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.RansomPrisonersUseWhitelist, "Ransom Prisoners Whitelist", "Use a whitelist instead of blacklist for the ransom prisoners filter",
                b => { _settings.RansomPrisonersUseWhitelist = b; }, CampaignOptionItemVM.OptionTypes.Boolean));

            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisableCustomUpgradePaths, "Disable Custom Upgrade Paths", "Custom Upgrade Paths will not be used when the upgrade button is clicked",
                b => { _settings.DisableCustomUpgradePaths = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisableUpdatedTroopLabel, "Disable Updated Wounded Troop Label", "Change the wounded troop label format from from (250 + 5w [255]/275) back to  (250 + 5w / 275)",
                b => { _settings.DisableUpdatedTroopLabel = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisablePartyCompositionIcon, "Hide Party Composition Icon", "Hide the party composition icon",
                b => { _settings.DisablePartyCompositionIcon = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.Debug, "Enable Debug Mode", "Enable Debug Mode, probably want to leave this off",
                b => { _settings.Debug = b; }, CampaignOptionItemVM.OptionTypes.Boolean));


            var miscOptions = new PMGenericOptionVM("Misc", "Misc", _options);
            return miscOptions;
        }

        public PMGenericOptionVM CreateSortOptions()
        {
            var sortOptions = PartyManagerSettings.GetSelectableSortOrderStrings();
            var options = new MBBindingList<IPMOptions>();
            options.Add(new PMStringOptionDataType<CustomSortOrder>(_settings.CustomSortOrderField1, "Custom Sort Field 1",
                "The first sort option to be applied in your custom sort", sortOptions,
                b => { _settings.CustomSortOrderField1 = b; }, CampaignOptionItemVM.OptionTypes.Selection));
            options.Add(new PMStringOptionDataType<CustomSortOrder>(_settings.CustomSortOrderField2, "Custom Sort Field 2",
                "The second sort option to be applied in your custom sort", sortOptions,
                b => { _settings.CustomSortOrderField2 = b; }, CampaignOptionItemVM.OptionTypes.Selection));
            options.Add(new PMStringOptionDataType<CustomSortOrder>(_settings.CustomSortOrderField3, "Custom Sort Field 3",
                "The third sort option to be applied in your custom sort", sortOptions,
                b => { _settings.CustomSortOrderField3 = b; }, CampaignOptionItemVM.OptionTypes.Selection));
            options.Add(new PMStringOptionDataType<CustomSortOrder>(_settings.CustomSortOrderField4, "Custom Sort Field 4",
                "The fourth sort option to be applied in your custom sort", sortOptions,
                b => { _settings.CustomSortOrderField4 = b; }, CampaignOptionItemVM.OptionTypes.Selection));
            options.Add(new PMStringOptionDataType<CustomSortOrder>(_settings.CustomSortOrderField5, "Custom Sort Field 5",
                "The fifth sort option to be applied in your custom sort", sortOptions,
                b => { _settings.CustomSortOrderField5 = b; }, CampaignOptionItemVM.OptionTypes.Selection));

            options.Add(new PMNumericOptionData(_settings.StickySlots, "Sticky Slots", "The number of slots directly below your heroes to ignore when executing sorts on your party's troops.",
                val => { _settings.StickySlots = (int)val; }, CampaignOptionItemVM.OptionTypes.Numeric, 0, 50, true));


            options.Add(new PMGenericOptionDataVM<bool>(_settings.EnableAutoSort, "Enable Autosort", "Enable Auto Sort on opening the party screen",
                b => { _settings.EnableAutoSort = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            options.Add(new PMGenericOptionDataVM<bool>(_settings.SortAfterRecruitAllUpgradeAllClick, "Sort After Recruit/Upgrade All Click", "Sort units after clicking the Recruit/Upgrade All buttons are clicked",
                b => { _settings.SortAfterRecruitAllUpgradeAllClick = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            options.Add(new PMGenericOptionDataVM<bool>(_settings.CavalryAboveFootmen, "Sort Mounted Units Above Footmen", "Turn off if you want the default sorts to not have mounted units above footmen",
                b => { _settings.CavalryAboveFootmen = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            options.Add(new PMGenericOptionDataVM<bool>(_settings.MeleeAboveArchers, "Sort Melee Units Above Archers", "Turn off if you want the default sorts to not have melee units above ranged",
                b => { _settings.MeleeAboveArchers = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            options.Add(new PMGenericOptionDataVM<bool>(_settings.EnableHotkey, "Enable Sort Hotkey", "Enable Sort Hotkey of CTRL+SHIFT+S",
                b => { _settings.EnableHotkey = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            options.Add(new PMGenericOptionDataVM<bool>(_settings.EnableRecruitUpgradeSortHotkey, "Enable Recruit/Upgrade Sort Hotkey", "Enable Recruit/Upgrade Sort of CTRL+SHIFT+R",
                b => { _settings.EnableRecruitUpgradeSortHotkey = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            options.Add(new PMGenericOptionDataVM<bool>(_settings.EnableSortTypeCycleHotkey, "Enable Cycle Sort Type Hotkey", "Enable Cycle Sort Type Hotkey of CTRL+SHIFT+(MINUS)",
                b => { _settings.EnableSortTypeCycleHotkey = b; }, CampaignOptionItemVM.OptionTypes.Boolean));


            var sortTab = new PMGenericOptionVM("Sort Options", "Sort Options", options);
            return sortTab;
        }

        public void ExecuteCancel()
        {
            _parentScreen.RemoveLayer(_screenLayer);
            _screenLayer = null;
        }

        public void ExecuteDone()
        {
            RefreshValues();
            PartyManagerSettings.Settings = Settings;
            Settings.SaveSettings();

            _parentScreen.RemoveLayer(_screenLayer);

            _screenLayer = null;
        }


        
    }
}
