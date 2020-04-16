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
    public class ReloadSettingsWidget : ButtonWidget
    {
        public ReloadSettingsWidget(UIContext context) : base(context)
        {
            EventFire += EventHandler;
        }


        private void EventHandler(Widget widget, string eventName, object[] args)
        {
            if (IsVisible)
            {
                if (eventName == "HoverBegin")
                {
                    ToggleTooltip(true);
                }
                else if (eventName == "HoverEnd")
                {
                    ToggleTooltip(false);
                }
            }
        }

        private void ToggleTooltip(bool on)
        {
            if (on)
            {
                InformationManager.AddHintInformation($"Reload Party Manager Settings");
            }
            else
            {
                InformationManager.HideInformations();
            }
        }

        protected override void OnClick()
        {
            base.OnClick();
            PartyManagerSettings.ReloadSettings();
        }
    }
}