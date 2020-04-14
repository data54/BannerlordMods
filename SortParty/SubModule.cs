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
        bool enableRecruitUpgradeSort = false;
        bool enableAutoSort = false;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            InformationManager.DisplayMessage(new InformationMessage("Loaded SortParty. Press CTRL+SHIFT+S in Party Screen to sort, CTRL+SHIFT+R to upgrade/recruit sort", Color.FromUint(4282569842U)));
            //Just here to trigger file generation if needed
            enableHotkey = SortPartySettings.Settings.EnableHotkey;
            enableAutoSort = SortPartySettings.Settings.EnableAutoSort;
            enableRecruitUpgradeSort = SortPartySettings.Settings.EnableRecruitUpgradeSortHotkey;
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
            if (enableHotkey || enableRecruitUpgradeSort)
            {
                string key = "";
                try
                {
                    if (Campaign.Current == null || !Campaign.Current.GameStarted || (!(ScreenManager.TopScreen is GauntletPartyScreen) || !InputKey.LeftShift.IsDown()) || !InputKey.LeftControl.IsDown())
                    {
                        return;
                    }

                    if ((enableHotkey && InputKey.S.IsDown()) || (enableRecruitUpgradeSort && InputKey.R.IsDown()))
                    {
                        var partyScreen = (GauntletPartyScreen)ScreenManager.TopScreen;
                        partyVM = partyScreen.GetPartyVM();

                        if (partyVM == null) return;

                        //SortHotkey
                        if (InputKey.S.IsDown() && enableHotkey)
                        {
                            key = "S";
                            SortUnits();
                        }
                        else if (InputKey.R.IsDown() && enableRecruitUpgradeSort)
                        {
                            key = "R";
                            SortUnits(true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SortPartyHelpers.LogException($"Tick('{key}')", ex);
                }
            }
        }


        private void SortUnits(bool recruitUpgradeSort = false)
        {
            try
            {
                partyVM.SortTroops(recruitUpgradeSort);
            }
            catch (Exception ex)
            {
                SortPartyHelpers.LogException("SortUnits", ex);
            }
        }
    }
}