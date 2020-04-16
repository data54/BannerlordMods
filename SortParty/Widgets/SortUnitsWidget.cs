using System;
using System.Collections.Generic;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;

namespace PartyManager.Widgets
{
    public class SortUnitsWidget : ButtonWidget
    {
        public SortUnitsWidget(UIContext context) : base(context)
        {
            EventFire += EventHandler;
        }


        private void EventHandler(Widget widget, string eventName, object[] args)
        {
            if (IsVisible)
            {
                if (eventName == "HoverBegin")
                {
                    InformationManager.AddHintInformation("Sort All Units\nRight click to sort all recruits/upgrades to the top");
                }
                else if (eventName == "HoverEnd")
                {
                    InformationManager.HideInformations();
                }
            }
        }

        protected override void OnClick()
        {
            base.OnClick();
            PartyController.CurrentInstance.SortPartyScreen();
        }

        protected override void OnAlternateClick()
        {
            base.OnAlternateClick();
            PartyController.CurrentInstance.SortPartyScreen(true);
        }
    }
}