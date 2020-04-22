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
    public class PMGenericOptionVM : TaleWorlds.Library.ViewModel
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

        public PMGenericOptionVM(PartyManagerSettings settings, string name, string titleText, MBBindingList<IPMOptions> options)
        {
            _settings = settings;

            _options = options;
            _name = name;
            _titleText = titleText;

            this.RefreshValues();
            
        }

    }
}
