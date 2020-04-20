using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyManager.ViewModel;
using PartyManager.ViewModel.Settings;
using TaleWorlds.CampaignSystem.SandBox.Issues;
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
        private CustomSortVM _customSortVM;


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
        public CustomSortVM CustomSortController
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

        public OptionsVM OptionsVm { get; set; }



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
            _customSortVM= new CustomSortVM();
            OptionsLbl = "Party Manager Settings";
            CancelLbl = "Cancel";
            DoneLbl = "Done";
            _parentScreen = parentScreen;
            _screenLayer = screenLayer;
            this.RefreshValues();
            _isInitialized = true;

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
