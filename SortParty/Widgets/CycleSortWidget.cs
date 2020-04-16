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
    public class CycleUnitSortWidget : ButtonWidget
    {
        public CycleUnitSortWidget(UIContext context) : base(context)
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
                InformationManager.AddHintInformation($"Cycle Sort Order\nRight click to cycle backwards\nCurrent:{PartyManagerSettings.Settings.SortOrderString}\nNext:{PartyManagerSettings.Settings.NextSortOrderString}\nPrevious:{PartyManagerSettings.Settings.PreviousSortOrderString}");
            }
            else
            {
                InformationManager.HideInformations();
            }
        }

        void RefreshTooltip()
        {
            ToggleTooltip(false);
            ToggleTooltip(true);
        }

        protected override void OnClick()
        {
            base.OnClick();
            PartyManagerSettings.Settings.CycleSortType(false);
            RefreshTooltip();
        }

        protected override void OnAlternateClick()
        {
            base.OnAlternateClick();
            PartyManagerSettings.Settings.CycleSortType(true);
            RefreshTooltip();
        }
    }
}