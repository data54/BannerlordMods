using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyManager.ViewModel.Settings.OptionVMS;
using PartyManager.ViewModels;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Options.ManagedOptions;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace PartyManager.ViewModel.Settings
{
    public class TogglesVM : TaleWorlds.Library.ViewModel
    {
        private string _titleText;
        private string _name;

        [DataSourceProperty]
        public int OptionTypeID { get; set; }

        [DataSourceProperty]
        public string Name
        {
            get { return this._name; }
            set
            {
                if (!(value != this._name))
                    return;
                this._name = value;
                this.OnPropertyChanged(nameof(Name));
            }
        }


        [DataSourceProperty]
        public string TitleText
        {
            get { return this._titleText; }
            set
            {
                if (!(value != this._titleText))
                    return;
                this._titleText = value;
                this.OnPropertyChanged(nameof(TitleText));
            }
        }

        private MBBindingList<IPMOptions> _options;
        private OptionsVM _optionsVm;

        [DataSourceProperty]
        public MBBindingList<IPMOptions> Options
        {
            get { return this._options; }
            set
            {
                if (!(value != this._options))
                    return;
                this._options = value;
                this.OnPropertyChanged(nameof(Options));
            }
        }

        [DataSourceProperty]
        public OptionsVM OptionsVm
        {
            get { return this._optionsVm; }
            set
            {
                if (!(value != this._optionsVm))
                    return;
                this._optionsVm = value;
                this.OnPropertyChanged(nameof(OptionsVm));
            }
        }


        private PartyManagerSettings _settings;

        public TogglesVM(PartyManagerSettings settings)
        {
            _settings = settings;

            _options = new MBBindingList<IPMOptions>();
            _name = "Toggles";
            _titleText = "test";

            _options.Add(new PMGenericOptionDataVM<bool>(_settings.EnableAutoSort,"Enable Autosort", "Enable Auto Sort on opening the party screen",
                b => { _settings.EnableAutoSort = b;}, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.SortAfterRecruitAllUpgradeAllClick, "Sort After Recruit/Upgrade All Click", "Sort units after clicking the Recruit/Upgrade All buttons are clicked",
                b => { _settings.SortAfterRecruitAllUpgradeAllClick = b; }, CampaignOptionItemVM.OptionTypes.Boolean));


            _options.Add(new PMGenericOptionDataVM<bool>(_settings.CavalryAboveFootmen, "Sort Mounted Units Above Footmen", "Turn off if you want the default sorts to not have mounted units above footmen",
                b => { _settings.CavalryAboveFootmen = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.MeleeAboveArchers, "Sort Melee Units Above Archers", "Turn off if you want the default sorts to not have melee units above ranged",
                b => { _settings.MeleeAboveArchers = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            
            
            
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.EnableHotkey, "Enable Sort Hotkey", "Enable Sort Hotkey of CTRL+SHIFT+S",
                b => { _settings.EnableHotkey = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.EnableRecruitUpgradeSortHotkey, "Enable Recruit/Upgrade Sort Hotkey", "Enable Recruit/Upgrade Sort of CTRL+SHIFT+R",
                b => { _settings.EnableRecruitUpgradeSortHotkey = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.EnableSortTypeCycleHotkey, "Enable Cycle Sort Type Hotkey", "Enable Cycle Sort Type Hotkey of CTRL+SHIFT+(MINUS)",
                b => { _settings.EnableSortTypeCycleHotkey = b; }, CampaignOptionItemVM.OptionTypes.Boolean));

            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisableCustomUpgradePaths, "Disable Custom Upgrade Paths", "Custom Upgrade Paths will not be used when the upgrade button is clicked",
                b => { _settings.DisableCustomUpgradePaths = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisableUpdatedTroopLabel, "Disable Updated Wounded Troop Label", "Change the wounded troop label from from (250 + 5w [255]/275) back to  (250 + 5w / 275)",
                b => { _settings.DisableUpdatedTroopLabel = b; }, CampaignOptionItemVM.OptionTypes.Boolean));



            _options.Add(new PMGenericOptionDataVM<bool>(_settings.Debug, "Enable Debug Mode", "Enable Debug Mode, probably want to leave this off",
                b => { _settings.Debug = b; }, CampaignOptionItemVM.OptionTypes.Boolean));

            this.RefreshValues();
            
        }

    }
}
