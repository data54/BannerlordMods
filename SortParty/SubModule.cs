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
                partyVM = GetPartyVM();

                if(partyVM!=null)
                {
                    SortUnits();
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage($"SortParty Tick exception: {ex.Message}"));
            }
        }

        private PartyVM GetPartyVM()
        {
            try
            {

            }
            catch(Exception ex)
            {

            }

            return null;
        }

        private void SortUnits()
        {
            try
            {

            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage($"SortPartyModule.SortUnits exception: {ex.Message}"));
            }
        }
    }
}