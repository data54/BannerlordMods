using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyManager.ViewModel.Settings;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;


namespace PartyManager.ViewModel
{
    public class PartyManagerSettingsVM : TaleWorlds.Library.ViewModel
    {
        private TogglesVM _togglesController;
        private CustomSortVM _customSortVM;

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

        public PartyManagerSettingsVM()
        {
            //TaleWorlds.GauntletUI.ExtraWidgets.
            //TabToggleWidget


        }


    }
}
