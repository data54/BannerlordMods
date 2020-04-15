using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.Screens;

namespace SortParty
{
    public class PartyController
    {
        static PartyController _partyController;
        public static PartyController CurrentInstance 
        { 
            get
            {
                if(_partyController==null)
                {
                    _partyController = new PartyController();
                }
                return _partyController;
            }
        }

        public GauntletPartyScreen PartyScreen { get; set; }
        public PartyScreenLogic PartyScreenLogic { get; set; }
        #region Methods
        public MethodInfo RefreshPartyInformation { get; set; }
        public MethodInfo InitializeTroopLists { get; set; }
        #endregion Methods


        public PartyController()
        {
            PartyScreen = ScreenManager.TopScreen as GauntletPartyScreen;
            if(PartyScreen==null)
            {

            }

        }

    }
}
