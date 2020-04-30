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
    public class TransferVM : TaleWorlds.Library.ViewModel
    {
        private readonly PartyScreenLogic _partyLogic;
        private readonly PartyVM _partyVM;

        [DataSourceProperty]
        public HintViewModel LeftTroopTransferTooltip { get; set; }
        [DataSourceProperty]
        public HintViewModel LeftPrisonerTransferTooltip { get; set; }
        [DataSourceProperty]
        public HintViewModel RightTroopTransferTooltip { get; set; }
        [DataSourceProperty]
        public HintViewModel RightPrisonerTransferTooltip { get; set; }


        public TransferVM(PartyScreenLogic partyLogic, PartyVM partyVm)
        {
            this._partyLogic = partyLogic;
            this._partyVM = partyVm;


            LeftTroopTransferTooltip = new HintViewModel("Transfer Troops to Party Limit");
            RightTroopTransferTooltip = new HintViewModel("Transfer Troops to Party Limit");
            LeftPrisonerTransferTooltip = new HintViewModel("Transfer Prisoners to Prisoner Limit");
            RightPrisonerTransferTooltip = new HintViewModel("Transfer Prisoners to Prisoner Limit");

        }

        public void TransferTroopsLeft()
        {

        }
        public void TransferPrisonersLeft()
        {

        }
        public void TransferTroopsRight()
        {

        }
        public void TransferPrisonersRight()
        {

        }

    }
}
