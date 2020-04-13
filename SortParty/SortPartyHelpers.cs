using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SortParty
{
    public class SortPartyHelpers
    {

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
