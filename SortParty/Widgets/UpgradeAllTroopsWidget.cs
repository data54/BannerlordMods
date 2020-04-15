using System.Collections.Generic;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;

namespace SortParty.Widgets
{
    public class UpgradeAllTroopsWidget : ButtonWidget
    {
        public UpgradeAllTroopsWidget(UIContext context) : base(context)
        {
            EventFire += EventHandler;
        }

        private void EventHandler(Widget widget, string eventName, object[] args)
        {
            if (IsVisible)
            {
                if (eventName == "HoverBegin")
                {
                    InformationManager.AddHintInformation("Upgrade All Troops");
                }
                if (eventName == "HoverEnd")
                {
                    InformationManager.HideInformations();
                }
            }
        }

        protected override void OnClick()
        {
            base.OnClick();
            UpgradeAllTroops();
        }

        public void UpgradeAllTroops()
        {
            PartyController.CurrentInstance.UpgradeAllTroops();
        }
    }
}