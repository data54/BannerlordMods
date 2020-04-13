using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

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
                    return flattenedRoster.OrderByDescending(x => x.Troop.Culture.ToString()).ThenBy(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
                    break;
                case SortType.CultureTierAsc:
                    return flattenedRoster.OrderBy(x => x.Troop.Culture.ToString()).ThenBy(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();
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

    }
}
