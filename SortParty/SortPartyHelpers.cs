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
                    return flattenedRoster.OrderByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.TierAsc:
                    return flattenedRoster.OrderBy(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.TierDescType:
                    return flattenedRoster.OrderByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.IsMounted).ThenBy(x => x.Troop.IsArcher).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.TierAscType:
                    return flattenedRoster.OrderBy(x => x.Troop.Tier).ThenBy(x => x.Troop.IsMounted).ThenBy(x => x.Troop.IsArcher).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.MountRangeTierDesc:
                    return flattenedRoster.OrderByDescending(x => x.Troop.IsMounted).ThenBy(x => x.Troop.IsArcher).ThenByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.MountRangeTierAsc:
                    return flattenedRoster.OrderByDescending(x => x.Troop.IsMounted).ThenBy(x => x.Troop.IsArcher).ThenBy(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.CultureTierDesc:
                    return flattenedRoster.OrderBy(x => x.Troop.Culture.Name.ToString()).ThenByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.CultureTierAsc:
                    return flattenedRoster.OrderBy(x => x.Troop.Culture.Name.ToString()).ThenBy(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
            }

            return flattenedRoster.OrderByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
        }

        public static bool IsRangedUnit(CharacterObject troop)
        {
            var result = false;

            var battleEquipments = troop.BattleEquipments.ToList();

            if (battleEquipments.Count > 0)
            {
                var primaryWeaponType = battleEquipments[0].GetEquipmentFromSlot(EquipmentIndex.WeaponItemBeginSlot).Item.ItemType;
                result = primaryWeaponType == ItemObject.ItemTypeEnum.Bow || primaryWeaponType == ItemObject.ItemTypeEnum.Crossbow || primaryWeaponType == ItemObject.ItemTypeEnum.Thrown;
            }
            return result;
        }

        public static bool IsMountedUnit(CharacterObject troop)
        {

            var result = false;
            var battleEquipments = troop.BattleEquipments.ToList();
            if (battleEquipments.Count > 0)
            {
                result = !battleEquipments[0].Horse.IsEmpty;
            }

            return result;
        }


        public static void LogException(string method, Exception ex)
        {
            InformationManager.DisplayMessage(new InformationMessage($"SortParty {method} exception: {ex.Message}"));
        }
    }
}
