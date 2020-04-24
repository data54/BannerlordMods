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
        private TogglesVM _togglesController;
        private PMGenericOptionVM _customSortVM;


        [DataSourceProperty] 
        public String OptionsLbl { get; set; }
        [DataSourceProperty]
        public String CancelLbl { get; set; }
        [DataSourceProperty]
        public String DoneLbl { get; set; }

        [DataSourceProperty]
        public TogglesVM TogglesController
        {
            get => _togglesController;
            set
            {
                if (value == this._togglesController)
                    return;
                this._togglesController = value;
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
            _togglesController=new TogglesVM(_settings);
            _customSortVM= CreateSortOptions();
            OptionsLbl = "Party Manager Settings";
            CancelLbl = "Cancel";
            DoneLbl = "Done";
            _parentScreen = parentScreen;
            _screenLayer = screenLayer;
            this.RefreshValues();
            _isInitialized = true;


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
