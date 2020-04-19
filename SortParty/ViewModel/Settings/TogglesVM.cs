using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyManager.Settings.CustomSettingClasses;
using PartyManager.ViewModels;
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

        private List<GenericOptionDataVM> _options;
        private OptionsVM _optionsVm;

        [DataSourceProperty]
        public List<GenericOptionDataVM> Options
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


        public TogglesVM()
        {
            Options= new List<GenericOptionDataVM>();
            _name = "Toggles";
            _titleText = "test";
            OptionsVm = new OptionsVM(false, false, KeyOptionsVM);

            Options.Add(createBooleanOptionDataVM("test","Test Description", true, OptionsVm));
            OptionTypeID = -1;
            this.RefreshValues();
        }
        private void KeyOptionsVM(GameKeyOptionVM optonsVm)
        {

        }

        private BooleanOptionDataVM createBooleanOptionDataVM(string name, string description, bool value, OptionsVM optionsVm)
        {
            var data = new GauntletBooleanData(value);
            var result = new BooleanOptionDataVM(optionsVm, data,
                new TextObject(name, new Dictionary<string, TextObject>()),
                new TextObject(description, new Dictionary<string, TextObject>()));
            
            result.ImageIDs = new string[0];
            return result;

        }
    }
}
