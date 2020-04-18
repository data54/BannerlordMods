using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace PartyManager.ViewModels
{
    public class UpgradeAllTroopsVM : TaleWorlds.Library.ViewModel
    {
        private readonly MBBindingList<PartyCharacterVM> _mainPartyList;
        private readonly PartyScreenLogic _partyLogic;
        private readonly PartyVM _partyVM;
        private HintViewModel _upgradeHint;



        private HintViewModel _upgradeTroopsHint;
        
        [DataSourceProperty]
        public HintViewModel UpgradeTroopsHint
        {
            get => _upgradeTroopsHint;
            set
            {
                _upgradeTroopsHint = value; 
                this.OnPropertyChanged(nameof(UpgradeTroopsHint));
            }
        }

        public UpgradeAllTroopsVM(PartyScreenLogic partyLogic, PartyVM partyVm)
        {
            this._partyLogic = partyLogic;
            this._partyVM = partyVm;
            this._mainPartyList = this._partyVM.MainPartyTroops;


            this.
            _upgradeTroopsHint = new HintViewModel(
                "Upgrade All Troops\nRight click to only upgrade custom paths\nCTRL+Left click unit upgrades to set/unset custom paths\nCTRL+SHIFT+Left Click to even split the upgrade\nCTRL+Right click to sort custom path units to the top");

        }

        public void UpgradeAllTroops()
        {

        }

    }
}
