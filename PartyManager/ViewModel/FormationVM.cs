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
    public class FormationVM : TaleWorlds.Library.ViewModel
    {
        private readonly MBBindingList<PartyCharacterVM> _mainPartyList;
        private readonly PartyScreenLogic _partyLogic;
        private readonly PartyVM _partyVM;
        
        private HintViewModel _tooltip;

        [DataSourceProperty]
        public HintViewModel Tooltip
        {
            get => _tooltip;
            set
            {
                _tooltip = value;
                this.OnPropertyChanged(nameof(Tooltip));
            }
        }

        public FormationVM(PartyScreenLogic partyLogic, PartyVM partyVm)
        {
            this._partyLogic = partyLogic;
            this._partyVM = partyVm;
            this._mainPartyList = this._partyVM.MainPartyTroops;


            this._tooltip = new HintViewModel("Left Click to apply saved formation settings to troops\nCTRL+Left Click to save current unit formations.");

        }

        public void Click()
        {

        }

        public void AltClick()
        {

        }
    }
}
