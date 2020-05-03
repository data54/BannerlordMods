using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyManager.Helpers;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace PartyManager.ViewModels
{
    public class RecruitVM : TaleWorlds.Library.ViewModel
    {
        private readonly MBBindingList<PartyCharacterVM> _mainPartyList;
        private readonly PartyScreenLogic _partyLogic;
        private readonly PartyVM _partyVM;
        private HintViewModel _upgradeHint;



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

        public RecruitVM(PartyScreenLogic partyLogic, PartyVM partyVm)
        {
            this._partyLogic = partyLogic;
            this._partyVM = partyVm;
            this._mainPartyList = this._partyVM.MainPartyTroops;


            this.
                _tooltip = new HintViewModel(TextHelper.GetText("RecruitTooltip","Recruit All Prisoners\nRight click to recruit past party limit"));

        }

        public void Click()
        {
            PartyController.CurrentInstance.RecruitAllPrisoners(false);
        }

        public void AltClick()
        {
            PartyController.CurrentInstance.RecruitAllPrisoners(true);
        }

    }
}
