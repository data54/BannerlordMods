using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;

namespace PartyManager.ViewModels
{
    public class PartyManagerVM : TaleWorlds.Library.ViewModel
    {
        protected readonly PartyVM _partyVM;
        protected readonly PartyScreenLogic _partyScreenLogic;
        private GauntletLayer _settingLayer;
        private GauntletPartyScreen _parentScreen;
        private GauntletMovie _currentMovie;
        private HintViewModel _settingsHint;
        private UpgradeTroopsVM _upgradeTroopsVM;
        private RecruitVM _recruitController;
        private SortUnitsVM _sortController;

        [DataSourceProperty]
        public PartyScreenLogic PartyScreenLogic
        {
            get
            {
                return this._partyScreenLogic;
            }
        }

        [DataSourceProperty]
        public UpgradeTroopsVM UpgradeTroopsController
        {
            get => _upgradeTroopsVM;
            set
            {
                if (value == this._upgradeTroopsVM)
                    return;
                this._upgradeTroopsVM = value;
                this.OnPropertyChanged(nameof(_upgradeTroopsVM));
            }
        }

        [DataSourceProperty]
        public RecruitVM RecruitController
        {
            get => _recruitController;
            set
            {
                if (value == this._recruitController)
                    return;
                this._recruitController = value;
                this.OnPropertyChanged(nameof(_recruitController));
            }
        }

        [DataSourceProperty]
        public SortUnitsVM SortController
        {
            get => _sortController;
            set
            {
                if (value == this._sortController)
                    return;
                this._sortController = value;
                this.OnPropertyChanged(nameof(_sortController));
            }
        }


        private HintViewModel _reloadSettingsTooltip;

        [DataSourceProperty]
        public HintViewModel ReloadSettingsTooltip
        {
            get => _reloadSettingsTooltip;
            set
            {
                _reloadSettingsTooltip = value;
                this.OnPropertyChanged(nameof(ReloadSettingsTooltip));
            }
        }


        public PartyManagerVM(
            PartyVM partyVM,
            PartyScreenLogic partyScreenLogic,
            GauntletPartyScreen parentScreen)
        {
            this._partyVM = partyVM;
            this._partyScreenLogic = partyScreenLogic;
            this._parentScreen = parentScreen;
            _upgradeTroopsVM = new UpgradeTroopsVM(partyScreenLogic, partyVM);
            _sortController = new SortUnitsVM(partyScreenLogic, partyVM);
            _recruitController = new RecruitVM(partyScreenLogic, partyVM);
            _reloadSettingsTooltip = new HintViewModel("Reload Party Manager Settings","reloadSettingsTooltipUniqueEnoughYet?");

        }


        public void ReloadSettings()
        {

        }




    }
}
