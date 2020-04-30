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

        [DataSourceProperty]
        public bool LeftTroopTransferHidden { get; set; }
        [DataSourceProperty]
        public bool LeftPrisonerTransferHidden { get; set; }
        [DataSourceProperty]
        public bool RightTroopTransferHidden { get; set; }
        [DataSourceProperty]
        public bool RightPrisonerTransferHidden { get; set; }



        public TransferVM(PartyScreenLogic partyLogic, PartyVM partyVm)
        {
            this._partyLogic = partyLogic;
            this._partyVM = partyVm;

            LeftTroopTransferHidden = true;
            LeftPrisonerTransferHidden = true;
            RightTroopTransferHidden = true;
            RightPrisonerTransferHidden = true;

            LeftTroopTransferTooltip = new HintViewModel("Transfer Troops to Party Limit");
            LeftPrisonerTransferTooltip = new HintViewModel("Transfer Prisoners to Prisoner Limit");
            RightTroopTransferTooltip = new HintViewModel("Transfer Troops to Party Limit");
            RightPrisonerTransferTooltip = new HintViewModel("Transfer Prisoners to Prisoner Limit");

            if (partyLogic.PrisonerTransferState == PartyScreenLogic.TransferState.TransferableWithTrade)
            {
                LeftPrisonerTransferHidden = false;
                LeftPrisonerTransferTooltip = new HintViewModel("Ransom black/white listed prisoners");
            }
            else if (partyLogic.PrisonerTransferState == PartyScreenLogic.TransferState.Transferable &&
                     partyLogic.MemberTransferState == PartyScreenLogic.TransferState.Transferable)
            {
                RightPrisonerTransferHidden = false;
                RightTroopTransferHidden = false;
            }
            else if (partyLogic.PrisonerTransferState == PartyScreenLogic.TransferState.Transferable)
            {
                RightPrisonerTransferHidden = false;

            }
            else if (partyLogic.MemberTransferState == PartyScreenLogic.TransferState.Transferable)
            {
                RightTroopTransferHidden = false;
            }
        }

        public void TransferTroopsLeft()
        {

        }
        public void TransferPrisonersLeft()
        {
            TransferPrisoners(false);
        }

        public void TransferPrisoners(bool fromLeft)
        {
            try
            {
                List<PartyCharacterVM> prisoners;

                if (fromLeft)
                {
                    prisoners = _partyVM.OtherPartyPrisoners.ToList();
                }
                else
                {
                    prisoners = _partyVM.MainPartyPrisoners.ToList();
                }

                if (_partyLogic.PrisonerTransferState == PartyScreenLogic.TransferState.TransferableWithTrade && PartyManagerSettings.Settings.RansomPrisonersUseWhitelist)
                {
                    prisoners = prisoners.Where(x => PartyManagerSettings.Settings.RansomPrisonerBlackWhiteList
                        .Contains(x.Character.Name.ToString())).ToList();
                }
                else if (_partyLogic.PrisonerTransferState == PartyScreenLogic.TransferState.TransferableWithTrade)
                {
                    prisoners = prisoners.Where(x => !PartyManagerSettings.Settings.RansomPrisonerBlackWhiteList
                        .Contains(x.Character.Name.ToString())).ToList();
                }
                else if (PartyManagerSettings.Settings.TransferPrisonersUseWhitelist)
                {
                    prisoners = prisoners.Where(x => PartyManagerSettings.Settings.TransferPrisonerBlackWhiteList
                        .Contains(x.Character.Name.ToString())).ToList();
                }
                else
                {
                    prisoners = prisoners.Where(x => !PartyManagerSettings.Settings.TransferPrisonerBlackWhiteList
                        .Contains(x.Character.Name.ToString())).ToList();
                }


                foreach (var prisoner in prisoners)
                {
                    TransferUnit(prisoner, fromLeft);
                }
                _partyVM.ExecuteRemoveZeroCounts();
            }
            catch (Exception e)
            {
                GenericHelpers.LogException("TransferPrisonersLeft", e);
            }

        }

        public void TransferUnit(PartyCharacterVM troop, bool fromLeft)
        {
            troop.OnTransfer(troop, -1, troop.Number,
                fromLeft ? PartyScreenLogic.PartyRosterSide.Left : PartyScreenLogic.PartyRosterSide.Right);
        }

        public void TransferTroopsRight()
        {

        }
        public void TransferPrisonersRight()
        {

        }

    }
}
