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
using TaleWorlds.Engine.Options;
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
                this.OnPropertyChanged(nameof(_togglesController));
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
                this.OnPropertyChanged(nameof(_customSortVM));
            }
        }

        public OptionsVM OptionsVm { get; set; }

        private PartyManagerSettings _settings;

        private bool _isInitialized = false;
        public PartyManagerSettingsVM()
        {
            //TaleWorlds.GauntletUI.ExtraWidgets.
            //TabToggleWidget
            _settings = PartyManagerSettings.Settings.Clone();
            _togglesController=new TogglesVM(_settings);
            _customSortVM= new CustomSortVM();
            OptionsLbl = "Party Manager Settings";
            CancelLbl = "Cancel";
            DoneLbl = "Done";
            this.RefreshValues();
            _isInitialized = true;

        }

        public void ExecuteCancel()
        {

        }

        public void ExecuteDone()
        {

        }


        
    }
}
