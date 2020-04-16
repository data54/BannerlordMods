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
    public class UpgradeAllTroopsWidget : ButtonWidget
    {
        public UpgradeAllTroopsWidget(UIContext context) : base(context)
        {
            EventFire += EventHandler;

        }

        //public UpgradeAllTroopsWidget(UIContext context, GauntletMovie movie) : base(context)
        //{
        //    EventFire += EventHandler;

        //    IsVisible = true;
        //    Id = "UpgradeAllTroopsButton2";
        //    Tag = Guid.NewGuid().ToString();
        //    this.VisualDefinition = new VisualDefinition("test", 0f, 0f, false);
        //    DoNotPassEventsToChildren = true;
        //    WidthSizePolicy = SizePolicy.Fixed;
        //    HeightSizePolicy = SizePolicy.Fixed;

        //    HorizontalAlignment = HorizontalAlignment.Right;
        //    VerticalAlignment = VerticalAlignment.Top;

        //    Brush = UIResourceManager.BrushFactory.GetBrush("PartyManager.UpgradeAll");
        //    MarginLeft = 10f;
        //    MarginRight = 80f;
        //    MarginTop = 15f;
        //    MarginBottom = 0f;

        //    SuggestedWidth = 500f;
        //    SuggestedHeight = 500f;
        //}

        private void EventHandler(Widget widget, string eventName, object[] args)
        {
            if (IsVisible)
            {
                if (eventName == "HoverBegin")
                {
                    InformationManager.AddHintInformation("Upgrade All Troops");
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
            PartyController.CurrentInstance.UpgradeAllTroops();
        }

        protected override void OnAlternateClick()
        {
            base.OnAlternateClick();
            PartyController.CurrentInstance.SortPartyScreen(true);
        }
    }
}