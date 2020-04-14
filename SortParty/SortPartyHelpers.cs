using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SortParty
{
    public class SortPartyHelpers
    {
        private static SortPartySettings Settings;


        public static void SortPartyScreen(PartyVM partyVm, bool sortRecruitUpgrade = false)
        {
            try
            {
                var refreshPartyCall = partyVm.GetType().GetMethod("RefreshPartyInformation", BindingFlags.Instance | BindingFlags.NonPublic);
                var initializeTroopListsCall = partyVm.GetType().GetMethod("InitializeTroopLists", BindingFlags.Instance | BindingFlags.NonPublic);
                var partyLogicField = partyVm.GetType().GetField("_partyScreenLogic", BindingFlags.Instance | BindingFlags.NonPublic);

                if (refreshPartyCall == null || initializeTroopListsCall == null || partyLogicField == null) return;

                var partyLogic = partyLogicField.GetValue(partyVm) as PartyScreenLogic;

                //Left Side
                SortUnits(partyLogic.MemberRosters[0], sortRecruitUpgrade, partyVm.OtherPartyTroops);
                SortUnits(partyLogic.PrisonerRosters[0], sortRecruitUpgrade, partyVm.OtherPartyPrisoners);
                //Right Side
                SortUnits(partyLogic.MemberRosters[1], sortRecruitUpgrade, partyVm.MainPartyTroops);
                SortUnits(partyLogic.PrisonerRosters[1], sortRecruitUpgrade, partyVm.MainPartyPrisoners);

                initializeTroopListsCall.Invoke(partyVm, new object[0] { });
            }
            catch (Exception ex)
            {
                LogException("SortPartyScreen", ex);
            }
        }


        public static void SortPartyScreen(PartyScreenLogic partyScreen, bool sortRecruitUpgrade = false)
        {
            try
            {
                //Left Side
                SortUnits(partyScreen.MemberRosters[0]);
                SortUnits(partyScreen.PrisonerRosters[0]);
                //Right Side
                SortUnits(partyScreen.MemberRosters[1]);
                SortUnits(partyScreen.PrisonerRosters[1]);
            }
            catch (Exception ex)
            {
                LogException("SortPartyScreen", ex);
            }
        }
        public static void SortUnits(TroopRoster input, bool sortRecruitUpgrade = false, MBBindingList<PartyCharacterVM> partyVmUnits = null)
        {
            if (input == null || input.Count == 0)
            {
                return;
            }

            var inputList = input.ToList();

            var flattenedOrder = CreateFlattenedRoster(input, sortRecruitUpgrade, partyVmUnits);

            for (int i = 0; i < inputList.Count; i++)
            {
                if (!inputList[i].Character.IsHero)
                {
                    input.RemoveTroop(inputList[i].Character, inputList[i].Number);
                }
            }

            input.Add(flattenedOrder);
        }

        public static List<FlattenedTroopRosterElement> CreateFlattenedRoster(TroopRoster roster, bool sortRecruitUpgrade, MBBindingList<PartyCharacterVM> partyVmUnits = null)
        {
            var flattenedRoster = roster.ToFlattenedRoster().Where(x => !x.Troop.IsHero);

            if (sortRecruitUpgrade && partyVmUnits != null)
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

            switch (SortPartySettings.Settings.SortOrder)
            {
                case SortType.TierDesc:
                    return flattenedRoster.OrderByDescending(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.TierAsc:
                    return flattenedRoster.OrderBy(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.TierDescType:
                    return flattenedRoster.OrderByDescending(x => x.Troop.Tier)
                        .ThenByDescending(x => SortPartySettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenBy(x => SortPartySettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.TierAscType:
                    return flattenedRoster.OrderBy(x => x.Troop.Tier)
                        .ThenByDescending(x => SortPartySettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenBy(x => SortPartySettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.MountRangeTierDesc:
                    return flattenedRoster.OrderByDescending(x => SortPartySettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenBy(x => SortPartySettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenByDescending(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.MountRangeTierAsc:
                    return flattenedRoster.OrderByDescending(x => SortPartySettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenBy(x => SortPartySettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
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
                    return flattenedRoster.OrderBy(x => SortPartySettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenByDescending(x => SortPartySettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenByDescending(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.RangeMountTierAsc:
                    return flattenedRoster.OrderBy(x => SortPartySettings.Settings.MeleeAboveArchers ? x.Troop.IsArcher : !x.Troop.IsArcher)
                        .ThenByDescending(x => SortPartySettings.Settings.CavalryAboveFootmen ? x.Troop.IsMounted : !x.Troop.IsMounted)
                        .ThenByDescending(x => x.Troop.Tier)
                        .ThenBy(x => x.Troop.Name.ToString()).ToList();
                case SortType.Custom:
                    return flattenedRoster
                        .OrderBy(x => GetSortFieldValue(x, SortPartySettings.Settings.CustomSortOrderField1), new CustomComparer(SortPartySettings.Settings.CustomSortOrderField1))
                        .ThenBy(x => GetSortFieldValue(x, SortPartySettings.Settings.CustomSortOrderField2), new CustomComparer(SortPartySettings.Settings.CustomSortOrderField2))
                        .ThenBy(x => GetSortFieldValue(x, SortPartySettings.Settings.CustomSortOrderField3), new CustomComparer(SortPartySettings.Settings.CustomSortOrderField3))
                        .ThenBy(x => GetSortFieldValue(x, SortPartySettings.Settings.CustomSortOrderField4), new CustomComparer(SortPartySettings.Settings.CustomSortOrderField4))
                        .ThenBy(x => GetSortFieldValue(x, SortPartySettings.Settings.CustomSortOrderField5), new CustomComparer(SortPartySettings.Settings.CustomSortOrderField5))
                        .ToList();
            }

            return flattenedRoster.OrderByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
        }

        public static string GetSortFieldValue(FlattenedTroopRosterElement element, CustomSortOrder customOrder)
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
                default:
                    return "";
            }
        }

        public static void LogException(string method, Exception ex)
        {
            InformationManager.DisplayMessage(new InformationMessage($"SortParty {method} exception: {ex.Message}"));
        }
    }

    public class CustomComparer : IComparer<int>, IComparer<string>, IComparer<bool>
    {
        int multiplier = 1;

        public CustomComparer(CustomSortOrder sortOrder)
        {
            if (sortOrder.ToString().ToUpper().EndsWith("DESC"))
            {
                multiplier = -1;
            }
        }

        public int Compare(string x, string y)
        {
            return string.Compare(x, y, true) * multiplier;
        }

        public int Compare(int x, int y)
        {
            return x.CompareTo(y) * multiplier;
        }

        public int Compare(bool x, bool y)
        {
            return x.CompareTo(y) * multiplier;
        }
    }
}
