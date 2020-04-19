using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyManager.ViewModel;
using PartyManager.ViewModel.Settings;
using TaleWorlds.CampaignSystem.SandBox.Issues;
using TaleWorlds.Engine.Options;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.Localization;
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

        public PartyManagerSettingsVM()
        {
            //TaleWorlds.GauntletUI.ExtraWidgets.
            //TabToggleWidget
            _togglesController=new TogglesVM();
            _customSortVM= new CustomSortVM();
            OptionsLbl = "Test";
            CancelLbl = "Cancel";
            DoneLbl = "Done";

        }

        public void ExecuteCancel()
        {

        }

        public void ExecuteDone()
        {

        }
    }
}
