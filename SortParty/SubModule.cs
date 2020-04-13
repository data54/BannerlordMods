using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library.EventSystem;
using HarmonyLib;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;

namespace SortParty
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                new Harmony("mod.sortparty").PatchAll();
                InformationManager.DisplayMessage(new InformationMessage("Loaded SortParty. Press CTRL+SHIFT+S in Party Screen to sort", Color.FromUint(4282569842U)));
                
                //Just here to trigger file generation if needed
                var test = SortPartySettings.Settings;
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Patch Failed {ex.Message}"));
            }
        }


        PartyVM partyVM;
        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);
            try
            {
                if (Campaign.Current == null || !Campaign.Current.GameStarted || (!(ScreenManager.TopScreen is GauntletPartyScreen) || !InputKey.LeftShift.IsDown()) || !InputKey.LeftControl.IsDown() || !InputKey.S.IsPressed())
                {
                    return;
                }

                var partyScreen = (GauntletPartyScreen)ScreenManager.TopScreen;
                partyVM = partyScreen.GetPartyVM();

                if (partyVM != null)
                {
                    SortUnits();
                }
            }
            catch (Exception ex)
            {
                SortPartyHelpers.LogException("Tick", ex);
            }
        }


        private void SortUnits()
        {
            try
            {
                partyVM.SortTroops();
            }
            catch (Exception ex)
            {
                SortPartyHelpers.LogException("SortUnits", ex);
            }
        }
    }
}