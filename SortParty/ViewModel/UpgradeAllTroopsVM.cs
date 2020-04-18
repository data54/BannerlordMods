using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.Screens;
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
                "Upgrade All Troops" +
                "\nRight Click to only upgrade custom paths" +
                "\nCTRL+Right Click to sort custom path units to the top"+
                "\nCTRL+Left Click unit upgrades to set/unset custom paths" +
                "\nCTRL+SHIFT+Left Click to even split the upgrade" );

        }

        public void UpgradeLeftClick()
        {
            PartyController.CurrentInstance.UpgradeAllTroops(false);
        }

        public void UpgradeRightClick()
        {
            if (!PartyManagerSettings.Settings.DisableCustomUpgradePaths && (ScreenManager.TopScreen is GauntletPartyScreen topScreen) && topScreen.DebugInput.IsControlDown())
            {
                PartyController.CurrentInstance.SortPartyScreen(SortType.CustomUpgrades, true, true, false, false, false);
            }
            else
            {
                PartyController.CurrentInstance.UpgradeAllTroops(true);
            }
        }

    }
}
