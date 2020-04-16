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
using System.Linq;

namespace PartyManager
{
    public class SubModule : MBSubModuleBase
    {
        bool enableHotkey = false;
        bool enableRecruitUpgradeSort = false;
        bool enableAutoSort = false;
        bool enableSortTypeCycleHotkey = false;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            InformationManager.DisplayMessage(new InformationMessage("Loaded SortParty. Press CTRL+SHIFT+S in Party Screen to sort, CTRL+SHIFT+R to upgrade/recruit sort, CTRL+SHIFT+(Minus) to cycle sort types", Color.FromUint(4282569842U)));
            if (!string.IsNullOrEmpty(SortPartySettings.Settings.NewFileMessage))
            {
                InformationManager.DisplayMessage(new InformationMessage(SortPartySettings.Settings.NewFileMessage, Color.FromUint(4282569842U)));
            }
        }

        bool showFileCreateMessage = false;
        protected override void OnSubModuleLoad()
        {
            enableHotkey = SortPartySettings.Settings.EnableHotkey;
            enableAutoSort = SortPartySettings.Settings.EnableAutoSort;
            enableRecruitUpgradeSort = SortPartySettings.Settings.EnableRecruitUpgradeSortHotkey;
            enableSortTypeCycleHotkey = SortPartySettings.Settings.EnableSortTypeCycleHotkey;

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
        public static float lastTick = 0;
        long lastHotkeyExecute = 0;
        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);
            lastTick = dt;
            if (enableHotkey || enableRecruitUpgradeSort || enableSortTypeCycleHotkey)
            {
                string key = "";
                try
                {
                    if (Campaign.Current == null || !Campaign.Current.GameStarted || (!(ScreenManager.TopScreen is GauntletPartyScreen) || (!InputKey.LeftShift.IsDown()) && !InputKey.LeftControl.IsDown() && !InputKey.Minus.IsDown()))
                    {
                        return;
                    }


                    if ((enableHotkey && InputKey.S.IsDown()) || (enableRecruitUpgradeSort && InputKey.R.IsDown()) || (enableSortTypeCycleHotkey && InputKey.Minus.IsDown()))
                    {
                        var diff = (DateTime.Now.Ticks - lastHotkeyExecute) / TimeSpan.TicksPerMillisecond;

                        //Prevent the key from triggering more than once per tenth of a second
                        if (diff < 100)
                        {
                            return;
                        }

                        if (InputKey.Minus.IsDown() && enableSortTypeCycleHotkey)
                        {
                            key = "-";
                            SortPartySettings.Settings.CycleSortType();
                        }

                        //SortHotkey
                        if (InputKey.S.IsDown() && enableHotkey)
                        {
                            key = "S";
                            PartyController.CurrentInstance.SortPartyScreen();
                        }//RecruitSort
                        else if (InputKey.R.IsDown() && enableRecruitUpgradeSort)
                        {
                            key = "R";
                            PartyController.CurrentInstance.SortPartyScreen(true);
                        }
                        lastHotkeyExecute = DateTime.Now.Ticks;
                    }
                }
                catch (Exception ex)
                {
                    lastHotkeyExecute = DateTime.Now.Ticks;
                    GenericHelpers.LogException($"Tick('{key}')", ex);
                }
            }
        }
    }
}