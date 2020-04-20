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
    public class CustomSortVM : TaleWorlds.Library.ViewModel
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

        public CustomSortVM(PartyManagerSettings settings)
        {
            _settings = settings;

            _options = new MBBindingList<IPMOptions>();
            _name = "Custom Sort";
            _titleText = "Custom Sort Options";
            var sortOptions = PartyManagerSettings.GetSelectableSortOrderStrings();
            
            _options.Add(new PMStringOptionDataType<CustomSortOrder>(_settings.CustomSortOrderField1, "Custom Sort Field 1",
                "The first sort option to be applied in your custom sort", sortOptions,
                b => { _settings.CustomSortOrderField1 = b; }, CampaignOptionItemVM.OptionTypes.Selection));
            _options.Add(new PMStringOptionDataType<CustomSortOrder>(_settings.CustomSortOrderField2, "Custom Sort Field 2",
                "The second sort option to be applied in your custom sort", sortOptions,
                b => { _settings.CustomSortOrderField2 = b; }, CampaignOptionItemVM.OptionTypes.Selection));
            _options.Add(new PMStringOptionDataType<CustomSortOrder>(_settings.CustomSortOrderField3, "Custom Sort Field 3",
                "The third sort option to be applied in your custom sort", sortOptions,
                b => { _settings.CustomSortOrderField3 = b; }, CampaignOptionItemVM.OptionTypes.Selection));
            _options.Add(new PMStringOptionDataType<CustomSortOrder>(_settings.CustomSortOrderField4, "Custom Sort Field 4",
                "The fourth sort option to be applied in your custom sort", sortOptions,
                b => { _settings.CustomSortOrderField4 = b; }, CampaignOptionItemVM.OptionTypes.Selection));
            _options.Add(new PMStringOptionDataType<CustomSortOrder>(_settings.CustomSortOrderField5, "Custom Sort Field 5",
                "The fifth sort option to be applied in your custom sort", sortOptions,
                b => { _settings.CustomSortOrderField5 = b; }, CampaignOptionItemVM.OptionTypes.Selection));

            this.RefreshValues();
        }

    }
}
