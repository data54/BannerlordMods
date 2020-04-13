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
        bool enableHotkey = false;
        bool enableAutoSort = false;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            InformationManager.DisplayMessage(new InformationMessage("Loaded SortParty. Press CTRL+SHIFT+S in Party Screen to sort", Color.FromUint(4282569842U)));
            //Just here to trigger file generation if needed
            enableHotkey = SortPartySettings.Settings.EnableHotkey;
            enableAutoSort = SortPartySettings.Settings.EnableAutoSort;
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            if (enableAutoSort)
            {
                try
                {
                    new Harmony("mod.sortparty").PatchAll();
                }
                catch (Exception ex)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"Patch Failed {ex.Message}"));
                }
            }
        }


        PartyVM partyVM;
        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);
            if (enableHotkey)
            {
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