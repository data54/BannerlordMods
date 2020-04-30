using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
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
        private FormationVM _formationVm;

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
        public FormationVM FormationController
        {
            get => _formationVm;
            set
            {
                if (value == this._formationVm)
                    return;
                this._formationVm = value;
                this.OnPropertyChanged(nameof(FormationController));
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
        private TransferVM _transferController;

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
        public HintViewModel RightTroopOverviewTooltip
        {
            get => getTroopTooltip(false);
        }

        [DataSourceProperty]
        public bool LeftTroopOverviewHidden
        {
            get => PartyManagerSettings.Settings.DisablePartyCompositionIcon ||
                   _partyScreenLogic?.LeftOwnerParty == null;
        }

        [DataSourceProperty]
        public bool RightTroopOverviewHidden
        {
            get => PartyManagerSettings.Settings.DisablePartyCompositionIcon ||
                   _partyScreenLogic?.RightOwnerParty == null;
        }

        [DataSourceProperty]
        public HintViewModel LeftTroopOverviewTooltip
        {
            get => getTroopTooltip(true);
        }

        [DataSourceProperty]
        public TransferVM TransferController
        {
            get => _transferController;
            set => _transferController = value;
        }


        public HintViewModel getTroopTooltip(bool leftParty)
        {
            var composition = "";

            if (leftParty && _partyScreenLogic?.LeftOwnerParty != null)
            {
                composition = getUnitComposition(PartyScreenLogic.LeftOwnerParty, _partyScreenLogic.LeftOwnerParty.PartySizeLimit);
            }
            else if (!leftParty && _partyScreenLogic?.RightOwnerParty != null)
            {
                composition = getUnitComposition(PartyScreenLogic.RightOwnerParty, _partyScreenLogic.RightOwnerParty.PartySizeLimit);
            }
            else
            {
                composition = "";
            }

            var model = new HintViewModel(composition);

            return model;
        }

        public string getUnitComposition(PartyBase troops, int partySizeLimit)
        {
            try
            {
                var start = DateTime.Now;
                GenericHelpers.LogDebug($"getUnitComposition", "getUnitCompositionStarted");
                var totalTroops = troops.NumberOfAllMembers;
                var advancedMeleeBreakdown = "";
                var advancedRangedBreakdown = "";
                var advancedCavalryMeleeBreakdown = "";
                var advancedCavalryRangedBreakdown = "";

                if (PartyManagerSettings.Settings.UseAdvancedPartyComposition)
                {

                    try
                    {
                        var advancedTroopInfo = troops.MemberRoster.Where(x => !x.Character.IsHero)
                            .GroupBy(x => new
                            {
                                WeaponClass = GenericHelpers.GetCharacterWeaponClass(x.Character),
                                x.Character.IsMounted,
                                x.Character.IsArcher,

                            })
                            .Select(group =>
                                new
                                {
                                    IsMounted = group.Key.IsMounted,
                                    WeaponClass = group.Key.WeaponClass,
                                    IsArcher = group.Key.IsArcher,
                                    Count = group.Sum(x => x.Number)
                                })
                            .OrderByDescending(x => x.Count).ToList();

                        foreach (var weaponClass in advancedTroopInfo.Where(x => x.IsMounted && !x.IsArcher))
                        {
                            advancedCavalryMeleeBreakdown += formatTroopInfo(1, weaponClass.Count, weaponClass.WeaponClass.ToString(), totalTroops);
                        }

                        foreach (var weaponClass in advancedTroopInfo.Where(x => x.IsMounted && x.IsArcher))
                        {
                            advancedCavalryRangedBreakdown += formatTroopInfo(1, weaponClass.Count, weaponClass.WeaponClass.ToString(), totalTroops);
                        }

                        foreach (var weaponClass in advancedTroopInfo.Where(x => !x.IsMounted && !x.IsArcher))
                        {
                            advancedMeleeBreakdown += formatTroopInfo(1, weaponClass.Count, weaponClass.WeaponClass.ToString(), totalTroops);
                        }

                        foreach (var weaponClass in advancedTroopInfo.Where(x => !x.IsMounted && x.IsArcher))
                        {
                            advancedRangedBreakdown += formatTroopInfo(1, weaponClass.Count, weaponClass.WeaponClass.ToString(), totalTroops);
                        }
                    }
                    catch (Exception ex)
                    {
                        GenericHelpers.LogException("GetAdvancedPartyComposition", ex);
                    }
                }

                var ret = "";

                var advancedMode = PartyManagerSettings.Settings.UseAdvancedPartyComposition;

                var subgroupIndents = advancedMode ? 0 : 1;


                var sb = new StringBuilder();
                sb.Append($"{troops.Name.ToString()}\n");
                sb.Append($"{totalTroops}/{partySizeLimit} Troops\n");

                var heroes = troops.MemberRoster.Count(x => x.Character.IsHero);

                var mounted = troops.MemberRoster.Where(x => !x.Character.IsHero && x.Character.IsMounted).Sum(x=>x.Number);
                var horseArchers = troops.MemberRoster.Where(x => !x.Character.IsHero && x.Character.IsArcher && x.Character.IsMounted).Sum(x => x.Number);
                var horseMelee = troops.MemberRoster.Where(x => !x.Character.IsHero && !x.Character.IsArcher && x.Character.IsMounted).Sum(x => x.Number);


                sb.Append("\n".PadLeft(40, '-'));
                sb.Append(formatTroopInfo(0, heroes, "Heroes", totalTroops));


                StringBuilder mountedSB = new StringBuilder();

                if (mounted > 0)
                {
                    mountedSB.Append("\n".PadLeft(40, '-'));
                    mountedSB.Append(formatTroopInfo(0, mounted, "Cavalry", totalTroops));
                    mountedSB.Append("\n".PadLeft(40, '-'));
                    mountedSB.Append(formatTroopInfo(subgroupIndents, horseMelee, "Melee", totalTroops));
                    mountedSB.Append(advancedCavalryMeleeBreakdown);
                    mountedSB.Append(formatTroopInfo(subgroupIndents, horseArchers, "Ranged", totalTroops));
                    mountedSB.Append(advancedCavalryRangedBreakdown);
                }


                StringBuilder infantrySB = new StringBuilder();
                var infantry = troops.MemberRoster.Where(x => !x.Character.IsHero && !x.Character.IsMounted).Sum(x => x.Number);
                if (infantry > 0)
                {
                    var footArchers = troops.MemberRoster.Where(x => !x.Character.IsHero && x.Character.IsArcher && !x.Character.IsMounted)
                        .Sum(x => x.Number);
                    var footMelee = troops.MemberRoster.Where(x => !x.Character.IsHero && !x.Character.IsArcher && !x.Character.IsMounted)
                        .Sum(x => x.Number);

                    infantrySB.Append("\n".PadLeft(40, '-'));
                    infantrySB.Append(formatTroopInfo(0, infantry, "Infantry", totalTroops));
                    infantrySB.Append("\n".PadLeft(40, '-'));
                    infantrySB.Append(formatTroopInfo(subgroupIndents, footMelee, "Melee", totalTroops));
                    infantrySB.Append(advancedMeleeBreakdown);
                    infantrySB.Append(formatTroopInfo(subgroupIndents, footArchers, "Ranged", totalTroops));
                    infantrySB.Append(advancedRangedBreakdown);
                }



                bool drawDivider = !(infantry == 0 || mounted == 0);
                
                sb.Append(infantrySB);
                if (drawDivider) infantrySB.Append("\n".PadLeft(40, '-'));
                sb.Append(mountedSB);


                var duration = DateTime.Now.Subtract(start);
                GenericHelpers.LogDebug($"getUnitComposition", $"getUnitCompositionEnded Duration: {duration.Milliseconds}");

                return sb.ToString().Trim('\n'); ;
            }
            catch (Exception e)
            {
                GenericHelpers.LogException("getUnitComposition", e);
            }

            return "";
        }

        private string formatTroopInfo(int indentionCount, int count, string name, int totalTroops)
        {
            if (count == 0)
            {
                return null;
            }
            var indents = (1 * indentionCount);
            var percent = (count * 100f / totalTroops).ToString("n2");

            var ret = $"{count} {name} ({percent}%)\n";

            ret = ret.PadLeft(ret.Length + indents, '-');

            return ret;
        }

        public PartyManagerVM(
            PartyVM partyVM,
            PartyScreenLogic partyScreenLogic,
            GauntletPartyScreen parentScreen)
        {
            this._partyVM = partyVM;
            this._partyScreenLogic = partyScreenLogic;
            this._parentScreen = parentScreen;
            _transferController= new TransferVM(partyScreenLogic, partyVM);
            _upgradeTroopsVM = new UpgradeTroopsVM(partyScreenLogic, partyVM);
            _sortController = new SortUnitsVM(partyScreenLogic, partyVM);
            _recruitController = new RecruitVM(partyScreenLogic, partyVM);
            _formationVm = new FormationVM(partyScreenLogic, partyVM);
            _openSettingsTooltip = new HintViewModel("Open Party Manager Settings", "openSettingsTooltipUniqueEnoughYet?");
        }



        public void OpenSettings()
        {
            PartyController.OpenSettings();
        }
    }
}
