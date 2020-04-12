using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace SortParty
{
    public class SortPartyHelpers
    {
        public static void SortUnits(TroopRoster input)
        {
            var inputList = input.ToList();

            var flattenedOrder = input.ToFlattenedRoster().Where(x => !x.Troop.IsHero).OrderByDescending(x => x.Troop.Tier).ThenBy(x => x.Troop.Name.ToString()).ToList();

            for (int i = 0; i < inputList.Count; i++)
            {
                if (!inputList[i].Character.IsHero)
                {
                    input.RemoveTroop(inputList[i].Character, inputList[i].Number);
                }
            }

            input.Add(flattenedOrder);
        }

    }
}
