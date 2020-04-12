using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library.EventSystem;
using HarmonyLib;

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
            catch(Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Patch Failed {ex.Message}"));

            }
        }        
    }
}