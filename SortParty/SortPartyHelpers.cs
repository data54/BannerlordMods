using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SortParty
{
    public class SortPartyHelpers
    {
        private static SortPartySettings Settings;

        public static void SortPartyScreen(PartyScreenLogic partyScreen)
        {
            SortPartyScreen(partyScreen, true, true, true, true);
        }

        public static void SortPartyScreen(PartyScreenLogic partyScreen, bool right, bool left, bool troops, bool prisoners)
        {
            try
            {
                if (left)
                {
                    if (troops)
                    {
                        SortUnits(partyScreen.MemberRosters[0]);
                    }
                    if (prisoners)
                    {
                        SortUnits(partyScreen.PrisonerRosters[0]);
                    }
                }

                if (right)
                {
                    if (troops)
                    {
                        SortUnits(partyScreen.MemberRosters[1]);
                    }
                    if (prisoners)
                    {
                        SortUnits(partyScreen.PrisonerRosters[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Error in SortParty: {ex.Message}"));
            }
        }

        public static void SortUnits(TroopRoster input)
        {
            var inputList = input.ToList();

            var flattenedOrder = CreateFlattenedRoster(input);

            for (int i = 0; i < inputList.Count; i++)
            {
                if (!inputList[i].Character.IsHero)
                {
                    input.RemoveTroop(inputList[i].Character, inputList[i].Number);
                }
            }

            input.Add(flattenedOrder);
        }

        public static List<FlattenedTroopRosterElement> CreateFlattenedRoster(TroopRoster roster)
        {
            var flattenedRoster = roster.ToFlattenedRoster().Where(x => !x.Troop.IsHero);

            switch (SortPartySettings.Settings.SortOrder)
            {
                case SortType.TierDesc:
                    return flattenedRoster.OrderByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.TierAsc:
                    return flattenedRoster.OrderBy(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.TierDescType:
                    return flattenedRoster.OrderByDescending(x => x.Troop.Tier).ThenBy(x => IsMountedUnit(x.Troop)).ThenBy(x => IsRangedUnit(x.Troop)).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.TierAscType:
                    return flattenedRoster.OrderBy(x => x.Troop.Tier).ThenBy(x => IsMountedUnit(x.Troop)).ThenBy(x => IsRangedUnit(x.Troop)).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.MountRangeTierDesc:
                    return flattenedRoster.OrderByDescending(x => IsMountedUnit(x.Troop)).ThenBy(x => IsRangedUnit(x.Troop)).ThenByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.MountRangeTierAsc:
                    return flattenedRoster.OrderByDescending(x => IsMountedUnit(x.Troop)).ThenBy(x => IsRangedUnit(x.Troop)).ThenBy(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
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

        public static MBBindingList<PartyCharacterVM> SortVMTroops(MBBindingList<PartyCharacterVM> input, bool sortRecruitUpgrade = false)
        {
            var order = SortType.TierDesc;
            List<PartyCharacterVM> sortedList = null;

            if (sortRecruitUpgrade)
            {
                sortedList = input.Where(x => !x.IsHero).OrderByDescending(x => x.IsTroopRecruitable || (x.IsUpgrade1Available && !x.IsUpgrade1Insufficient) || (x.IsUpgrade2Available && !x.IsUpgrade2Insufficient)).ThenByDescending(x=>(x.IsUpgrade1Available && x.IsUpgrade1Insufficient) || (x.IsUpgrade2Available && x.IsUpgrade2Insufficient)).ThenByDescending(x => x.Character.Tier).ThenBy(x => x.Character.Name.ToString()).ToList();
            }
            else
            {
                switch (order)
                {
                    case SortType.TierDesc:
                        sortedList = input.Where(x => !x.IsHero).OrderByDescending(x => x.Character.Tier).ThenBy(x => x.Character.Name.ToString()).ToList();
                        break;
                    case SortType.TierAsc:
                        sortedList = input.Where(x => !x.IsHero).OrderBy(x => x.Character.Tier).ThenBy(x => x.Character.Name.ToString()).ToList();
                        break;
                    case SortType.TierDescType:
                        sortedList = input.Where(x => !x.IsHero).OrderByDescending(x => x.Character.Tier).ThenBy(x => IsMountedUnit(x.Character)).ThenBy(x => IsRangedUnit(x.Character)).ThenBy(x => x.Character.Name.ToString()).ToList();
                        break;
                    case SortType.TierAscType:
                        sortedList = input.Where(x => !x.IsHero).OrderBy(x => x.Character.Tier).ThenBy(x => IsMountedUnit(x.Character)).ThenBy(x => IsRangedUnit(x.Character)).ThenBy(x => x.Character.Name.ToString()).ToList();
                        break;
                    case SortType.MountRangeTierDesc:
                        sortedList = input.Where(x => !x.IsHero).OrderByDescending(x => IsMountedUnit(x.Character)).ThenBy(x => IsRangedUnit(x.Character)).ThenByDescending(x => x.Character.Tier).ThenBy(x => x.Character.Name.ToString()).ToList();
                        break;
                    case SortType.MountRangeTierAsc:
                        sortedList = input.Where(x => !x.IsHero).OrderByDescending(x => IsMountedUnit(x.Character)).ThenBy(x => IsRangedUnit(x.Character)).ThenBy(x => x.Character.Tier).ThenBy(x => x.Character.Name.ToString()).ToList();
                        break;
                    case SortType.CultureTierDesc:
                        sortedList = input.Where(x => !x.IsHero).OrderBy(x => x.Character.Culture.Name.ToString()).ThenByDescending(x => x.Character.Tier).ThenBy(x => x.Character.Name.ToString()).ToList();
                        break;
                    case SortType.CultureTierAsc:
                        sortedList = input.Where(x => !x.IsHero).OrderBy(x => x.Character.Culture.Name.ToString()).ThenBy(x => x.Character.Tier).ThenBy(x => x.Character.Name.ToString()).ToList();
                        break;
                }
            }

            if (sortedList != null)
            {
                var output = new MBBindingList<PartyCharacterVM>();

                foreach (var hero in input.Where(x => x.IsHero))
                {
                    output.Add(hero);
                }

                foreach (var troop in sortedList)
                {
                    output.Add(troop);
                }

                return output;
            }


            return input;
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
