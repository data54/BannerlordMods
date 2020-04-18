﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;

namespace PartyManager.ViewModels
{
    public class PartyManagerModVM : TaleWorlds.Library.ViewModel
    {
        protected readonly PartyVM _partyVM;
        protected readonly PartyScreenLogic _partyScreenLogic;
        private GauntletLayer _settingLayer;
        private GauntletPartyScreen _parentScreen;
        private GauntletMovie _currentMovie;
        private HintViewModel _settingsHint;


        public PartyManagerModVM(
            PartyVM partyVM,
            PartyScreenLogic partyScreenLogic,
            GauntletPartyScreen parentScreen)
        {
            this._partyVM = partyVM;
            this._partyScreenLogic = partyScreenLogic;
            this._parentScreen = parentScreen;
        }

    }
}
