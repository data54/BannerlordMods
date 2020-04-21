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
                this.OnPropertyChanged(nameof(UpgradeTroopsController));
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
                this.OnPropertyChanged(nameof(RecruitController));
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
                this.OnPropertyChanged(nameof(SortController));
            }
        }


        private HintViewModel _openSettingsTooltip;

        [DataSourceProperty]
        public HintViewModel OpenSettingsTooltip
        {
            get => _openSettingsTooltip;
            set
            {
                _openSettingsTooltip = value;
                this.OnPropertyChanged(nameof(OpenSettingsTooltip));
            }
        }


        [DataSourceProperty]
        public HintViewModel TroopOverviewTooltip
        {
            get => getTroopTooltip();
            set
            {
                _openSettingsTooltip = value;
                this.OnPropertyChanged(nameof(OpenSettingsTooltip));
            }
        }

        public HintViewModel getTroopTooltip()
        {

            var composition = getUnitComposition(PartyScreenLogic.RightOwnerParty);

            var model = new HintViewModel(composition);
            
            return model;
        }

        public string getUnitComposition(PartyBase troops)
        {
            try
            {
                var ret = "";

                var totalTroops = troops.NumberOfAllMembers;
                var mounted = troops.NumberOfMenWithHorse;
                var mountedPercent = mounted * 100f / totalTroops;
                var onFoot = troops.NumberOfMenWithoutHorse;
                var onFootPercent = onFoot * 100f / totalTroops;
            
                var footArchers = troops.MemberRoster.Where(x => x.Character.IsArcher && !x.Character.IsMounted).Sum(x => x.Number);
                var footArcherPercent = footArchers * 100f / totalTroops;
                var horseArchers = troops.MemberRoster.Where(x => x.Character.IsArcher && x.Character.IsMounted).Sum(x => x.Number);
                var horseArcherPercent = horseArchers * 100f / totalTroops;

                var footMelee = troops.MemberRoster.Where(x => !x.Character.IsArcher && !x.Character.IsMounted).Sum(x => x.Number);
                var footMeleePercent = footMelee * 100f / totalTroops;
                var horseMelee = troops.MemberRoster.Where(x => !x.Character.IsArcher && x.Character.IsMounted).Sum(x => x.Number);
                var horseMeleePercent = horseMelee * 100f / totalTroops;



                var sb = new StringBuilder();
                sb.Append($"{troops.Name.ToString()}\n");
                sb.Append($"{totalTroops}/{_partyScreenLogic.RightOwnerParty.PartySizeLimit} Troops\n");
                sb.Append($"-{mounted} Mounted ({mountedPercent.ToString("n2")}%)\n");
                sb.Append($"--{horseMelee} Melee ({horseMeleePercent.ToString("n2")}%)\n");
                sb.Append($"--{horseArchers} Ranged ({horseArcherPercent.ToString("n2")}%)\n");
                sb.Append($"-{onFoot} On Foot ({onFootPercent.ToString("n2")}%)\n");
                sb.Append($"--{footMelee} Melee ({footMeleePercent.ToString("n2")}%)\n");
                sb.Append($"--{footArchers} Ranged ({footArcherPercent.ToString("n2")}%)\n");
                
                return sb.ToString();
            }
            catch (Exception e)
            {
                GenericHelpers.LogException("getUnitComposition", e);
            }

            return "";
        }

        public string getIndentedTroopString(int indentLevel, int count, string label, float percent)
        {

            var indentSpaces = 2; //assuming no one is going to have more than 9999 troops
            var labelLength = 15;

            var result = "";
            var countstring = count.ToString();
            var percentstring =$"{percent.ToString("n2")}%";
            int startingIndent = indentLevel * indentSpaces + 4;

            result += countstring.PadLeft(startingIndent);
            result = result.PadRight(5);
            result += label;
            result = result.PadRight(5);
            result += percentstring.PadLeft(4)+"\n";
            return result;
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
            _openSettingsTooltip = new HintViewModel("Open Party Manager Settings","openSettingsTooltipUniqueEnoughYet?");
        }



        public void OpenSettings()
        {
            PartyController.OpenSettings();
        }
    }
}
