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
    public class SortUnitsVM : TaleWorlds.Library.ViewModel
    {
        private readonly MBBindingList<PartyCharacterVM> _mainPartyList;
        private readonly PartyScreenLogic _partyLogic;
        private readonly PartyVM _partyVM;

        
        [DataSourceProperty]
        public HintViewModel CycleSortTooltip
        {
            get => new HintViewModel(getCycleTooltip());
        }

        private HintViewModel _sorttoolTip;

        [DataSourceProperty]
        public HintViewModel SortTooltip
        {
            get => _sorttoolTip;
            set
            {
                _sorttoolTip = value;
                this.OnPropertyChanged(nameof(SortTooltip));
            }
        }

        public SortUnitsVM(PartyScreenLogic partyLogic, PartyVM partyVm)
        {
            this._partyLogic = partyLogic;
            this._partyVM = partyVm;
            this._mainPartyList = this._partyVM.MainPartyTroops;


            this._sorttoolTip = new HintViewModel("Sort All Units\nRight click to sort all recruits/upgrades to the top");

        }

        public void SortClick()
        {
            PartyController.CurrentInstance.SortPartyScreen();
        }

        public void SortAltClick()
        {
            PartyController.CurrentInstance.SortPartyScreen(SortType.RecruitUpgrade);
        }

        public void CycleClick()
        {
            try
            {
                PartyManagerSettings.Settings.CycleSortType(false);
                this.CycleSortTooltip.HintText = getCycleTooltip();
                CycleSortTooltip.ExecuteCommand("ExecuteEndHint", new object[0]);
                CycleSortTooltip.ExecuteCommand("ExecuteBeginHint", new object[0]);
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("CycleClick", ex);
            }
        }

        public void CycleAltClick()
        {
            try
            {
                PartyManagerSettings.Settings.CycleSortType(true);
                this.CycleSortTooltip.HintText = getCycleTooltip();
                CycleSortTooltip.ExecuteCommand("ExecuteEndHint", new object[0]);
                CycleSortTooltip.ExecuteCommand("ExecuteBeginHint", new object[0]);
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("CycleAltClick", ex);
            }
        }

        private string getCycleTooltip()
        {
            return
                $"Cycle Sort Order\nRight click to cycle backwards\nNext:\n{PartyManagerSettings.Settings.NextSortOrderString}\nPrevious:\n{PartyManagerSettings.Settings.PreviousSortOrderString}\nCurrent:\n{PartyManagerSettings.Settings.SortOrderString}";
        }


    }
}
