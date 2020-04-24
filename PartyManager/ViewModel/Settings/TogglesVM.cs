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
            _titleText = "Toggles";
            
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.UseAdvancedPartyComposition, "Show Advanced Party Composition Information", "Party Composition will attempt to show unit weapon type breakdown",
                b => { _settings.UseAdvancedPartyComposition = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisableCustomUpgradePaths, "Disable Custom Upgrade Paths", "Custom Upgrade Paths will not be used when the upgrade button is clicked",
                b => { _settings.DisableCustomUpgradePaths = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisableUpdatedTroopLabel, "Disable Updated Wounded Troop Label", "Change the wounded troop label format from from (250 + 5w [255]/275) back to  (250 + 5w / 275)",
                b => { _settings.DisableUpdatedTroopLabel = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.DisablePartyCompositionIcon, "Hide Party Composition Icon", "Hide the party composition icon",
                b => { _settings.DisablePartyCompositionIcon = b; }, CampaignOptionItemVM.OptionTypes.Boolean));
            _options.Add(new PMGenericOptionDataVM<bool>(_settings.Debug, "Enable Debug Mode", "Enable Debug Mode, probably want to leave this off",
                b => { _settings.Debug = b; }, CampaignOptionItemVM.OptionTypes.Boolean));

            this.RefreshValues();
            
        }

    }
}
