using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace PartyManager.ViewModels
{
    public class UpgradeAllTroopsVM : TaleWorlds.Library.ViewModel
    {
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

        public UpgradeAllTroopsVM()
        {
            _upgradeTroopsHint = new HintViewModel(
                "Upgrade All Troops\nRight click to only upgrade custom paths\nCTRL+Left click unit upgrades to set/unset custom paths\nCTRL+SHIFT+Left Click to even split the upgrade\nCTRL+Right click to sort custom path units to the top"
                , "PMUpgradeAllTroops");

        }

        public void UpgradeAllTroops()
        {

        }

    }
}
