using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace PartyManager
{
    public class SortPartyHelpers
    {
        public static void SortPartyLogic(PartyScreenLogic PartyScreenLogic, PartyVM PartyVM, SortType sortType, bool rightTroops, bool rightPrisoners, bool leftTroops, bool leftPrisoners)
        {
            var left = (int) PartyScreenLogic.PartyRosterSide.Left;
            var right = (int)PartyScreenLogic.PartyRosterSide.Right;

            //Left Side
            if (leftTroops) SortPartyHelpers.SortUnits(PartyScreenLogic.MemberRosters[left], sortType, PartyVM?.OtherPartyTroops);
            if (leftPrisoners) SortUnits(PartyScreenLogic.PrisonerRosters[left], sortType, PartyVM?.OtherPartyPrisoners);
            //Right Side
            if (rightTroops) SortUnits(PartyScreenLogic.MemberRosters[right], sortType, PartyVM?.MainPartyTroops, true);
            if (rightPrisoners) SortPartyHelpers.SortUnits(PartyScreenLogic.PrisonerRosters[right], sortType, PartyVM?.MainPartyPrisoners);
        }

        public static void SortUnits(TroopRoster input, SortType sortType, MBBindingList<PartyCharacterVM> partyVmUnits = null, bool useStickySlots=false)
        {
            if (input == null || input.Count == 0)
            {
                return;
            }

            int sticky = 0;
            if (useStickySlots)
            {
                sticky = PartyManagerSettings.Settings.StickySlots;
            }

            var stickyUnits = input.Where(x => !x.Character.IsHero).Select(x => x.Character.Name.ToString()).Take(sticky).ToList();
            
            var inputList = input.ToList();

            var flattenedOrder = CreateFlattenedRoster(input, sortType, partyVmUnits);
            flattenedOrder = flattenedOrder.Where(x => !stickyUnits.Contains(x.Troop.Name.ToString())).ToList();
            
            for (int i = 0; i < inputList.Count; i++)
            {
                if (!inputList[i].Character.IsHero && !stickyUnits.Contains(inputList[i].Character.Name.ToString()))
                {
                    input.RemoveTroop(inputList[i].Character, inputList[i].Number);
                }
            }

            input.Add(flattenedOrder);
        }

        public static List<FlattenedTroopRosterElement> CreateFlattenedRoster(TroopRoster roster, SortType sortType, MBBindingList<PartyCharacterVM> partyVmUnits = null)
        {
            sortType = sortType == SortType.Default ? PartyManagerSettings.Settings.SortOrder : sortType;

            if (sortType == SortType.Default)
            {
                var sortOrder = PartyManagerSettings.Settings.SortOrder;
            }


            var flattenedRoster = roster.ToFlattenedRoster().Where(x => !x.Troop.IsHero);

            if (sortType == SortType.RecruitUpgrade && partyVmUnits != null)
            {
                //Units that can be upgraded
                var recruitUpgradeUnitTypes = partyVmUnits
                    .Where(x => !x.IsHero && (x.IsTroopRecruitable || (x.IsUpgrade1Available && !x.IsUpgrade1Insufficient) || (x.IsUpgrade2Available && !x.IsUpgrade2Insufficient)))
                    .Select(x => x.Name).Distinct().ToList();
                //Units that can be upgraded but are missing materials/skills
                var insufficientUpgrades = partyVmUnits
                    .Where(x => !x.IsHero && ((x.IsUpgrade1Available && x.IsUpgrade1Insufficient) || (x.IsUpgrade2Available && x.IsUpgrade2Insufficient)))
                    .Select(x => x.Name).Distinct().ToList();
                return flattenedRoster.OrderByDescending(x => recruitUpgradeUnitTypes.Contains(x.Troop.Name.ToString())).ThenByDescending(x => insufficientUpgrades.Contains(x.Troop.Name.ToString())).ThenByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
            }

            switch (sortType)
            {
                case SortType.TierDesc:
                    return flattenedRoster.OrderByDescending(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.TierAsc:
                    return flattenedRoster.OrderBy(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.TierDescType:
                    return flattenedRoster.OrderByDescending(x => x.Troop.Tier)
                        .ThenByDescending(x => PartyManagerSettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenBy(x => PartyManagerSettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.TierAscType:
                    return flattenedRoster.OrderBy(x => x.Troop.Tier)
                        .ThenByDescending(x => PartyManagerSettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenBy(x => PartyManagerSettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.MountRangeTierDesc:
                    return flattenedRoster.OrderByDescending(x => PartyManagerSettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenBy(x => PartyManagerSettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenByDescending(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.MountRangeTierAsc:
                    return flattenedRoster.OrderByDescending(x => PartyManagerSettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenBy(x => PartyManagerSettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenBy(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.CultureTierDesc:
                    return flattenedRoster.OrderBy(x => x.Troop.Culture.Name.ToString())
                        .ThenByDescending(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.CultureTierAsc:
                    return flattenedRoster.OrderBy(x => x.Troop.Culture.Name.ToString())
                        .ThenBy(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.RangeMountTierDesc:
                    return flattenedRoster.OrderBy(x => PartyManagerSettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenByDescending(x => PartyManagerSettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenByDescending(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.RangeMountTierAsc:
                    return flattenedRoster.OrderBy(x => PartyManagerSettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenByDescending(x => PartyManagerSettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenBy(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.Custom:
                    return flattenedRoster
                        .OrderBy(x => GetSortFieldValue(x, PartyManagerSettings.Settings.CustomSortOrderField1, partyVmUnits), new CustomComparer(PartyManagerSettings.Settings.CustomSortOrderField1))
                        .ThenBy(x => GetSortFieldValue(x, PartyManagerSettings.Settings.CustomSortOrderField2, partyVmUnits), new CustomComparer(PartyManagerSettings.Settings.CustomSortOrderField2))
                        .ThenBy(x => GetSortFieldValue(x, PartyManagerSettings.Settings.CustomSortOrderField3, partyVmUnits), new CustomComparer(PartyManagerSettings.Settings.CustomSortOrderField3))
                        .ThenBy(x => GetSortFieldValue(x, PartyManagerSettings.Settings.CustomSortOrderField4, partyVmUnits), new CustomComparer(PartyManagerSettings.Settings.CustomSortOrderField4))
                        .ThenBy(x => GetSortFieldValue(x, PartyManagerSettings.Settings.CustomSortOrderField5, partyVmUnits), new CustomComparer(PartyManagerSettings.Settings.CustomSortOrderField5))
                        .ToList();
                case SortType.CustomUpgrades:
                    var unitNames = PartyManagerSettings.Settings.SavedTroopUpgradePaths.Select(x=>x.UnitName);
                    return flattenedRoster.OrderByDescending(x => unitNames.Contains(x.Troop.Name.ToString()))
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
            }

            return flattenedRoster.OrderByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
        }

        public static string GetSortFieldValue(FlattenedTroopRosterElement element, CustomSortOrder customOrder, MBBindingList<PartyCharacterVM> partyVmUnits)
        {
            switch (customOrder)
            {
                case CustomSortOrder.TierAsc:
                case CustomSortOrder.TierDesc:
                    return element.Troop.Tier.ToString();
                case CustomSortOrder.CultureAsc:
                case CustomSortOrder.CultureDesc:
                    return element.Troop.Culture.Name.ToString();
                case CustomSortOrder.MountedAsc:
                case CustomSortOrder.MountedDesc:
                    return element.Troop.IsMounted ? "1" : "0";
                case CustomSortOrder.MeleeAsc:
                case CustomSortOrder.MeleeDesc:
                    return element.Troop.IsArcher ? "1" : "0";
                case CustomSortOrder.UnitNameAsc:
                case CustomSortOrder.UnitNameDesc:
                    return element.Troop.Name.ToString();
                case CustomSortOrder.UnitCountAsc:
                case CustomSortOrder.UnitCountDesc:
                    return partyVmUnits?.Where(x => x.Character.Name == element.Troop.Name)?.Select(x => x.Number)
                        ?.FirstOrDefault().ToString();
                case CustomSortOrder.CustomUpgradesPathAsc:
                case CustomSortOrder.CustomUpgradesPathDesc:
                    var unitHasUpgrade = PartyManagerSettings.Settings.SavedTroopUpgradePaths?.Exists(x =>
                        x.UnitName == element.Troop.Name.ToString());
                    if (unitHasUpgrade==true)
                    {
                        return "1";
                    }
                    return "0";
                default:
                    return "";
            }
        }

    }
}
