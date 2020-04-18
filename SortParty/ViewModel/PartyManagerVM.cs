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
        private UpgradeAllTroopsVM _upgradeTroopsVM;

        [DataSourceProperty]
        public PartyScreenLogic PartyScreenLogic
        {
            get
            {
                return this._partyScreenLogic;
            }
        }

        [DataSourceProperty]
        public UpgradeAllTroopsVM UpgradeAllTroops
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


        public PartyManagerVM(
            PartyVM partyVM,
            PartyScreenLogic partyScreenLogic,
            GauntletPartyScreen parentScreen)
        {
            this._partyVM = partyVM;
            this._partyScreenLogic = partyScreenLogic;
            this._parentScreen = parentScreen;
            _upgradeTroopsVM = new UpgradeAllTroopsVM(partyScreenLogic, partyVM);

        }


        public void Test()
        {

        }




    }
}
