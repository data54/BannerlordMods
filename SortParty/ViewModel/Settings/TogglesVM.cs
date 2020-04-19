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
        public OptionsVM OptionsVm { get; set; }

        [DataSourceProperty]
        public String Name { get; set; }




        public TogglesVM()
        {
            Options= new List<GenericOptionDataVM>();
            Name = "Toggles";
            _titleText = "test";
            OptionsVm = new OptionsVM(false, false, KeyOptionsVM);
            Options.Add(createBooleanOptionDataVM("test","Test Description", true, OptionsVm));

        }
        private void KeyOptionsVM(GameKeyOptionVM optonsVm)
        {

        }

        private BooleanOptionDataVM createBooleanOptionDataVM(string name, string description, bool value, OptionsVM optionsVm)
        {
            var data = new GauntletBooleanData(value);
            return new BooleanOptionDataVM(optionsVm, data,
                new TextObject(name, new Dictionary<string, TextObject>()),
                new TextObject(description, new Dictionary<string, TextObject>()));

        }
    }
}
