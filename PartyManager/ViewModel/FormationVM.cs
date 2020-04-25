using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyManager.Settings;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace PartyManager.ViewModels
{
    public class FormationVM : TaleWorlds.Library.ViewModel
    {
        private readonly MBBindingList<PartyCharacterVM> _mainPartyList;
        private readonly PartyScreenLogic _partyLogic;
        private readonly PartyVM _partyVM;
        
        private HintViewModel _tooltip;

        [DataSourceProperty]
        public HintViewModel Tooltip
        {
            get => _tooltip;
            set
            {
                _tooltip = value;
                this.OnPropertyChanged(nameof(Tooltip));
            }
        }

        public FormationVM(PartyScreenLogic partyLogic, PartyVM partyVm)
        {
            this._partyLogic = partyLogic;
            this._partyVM = partyVm;
            this._mainPartyList = this._partyVM.MainPartyTroops;


            this._tooltip = new HintViewModel("Left Click to apply saved formation settings to troops\nCTRL+Left Click to save current unit formations.");

        }

        public void Click()
        {
            if (Input.DebugInput.IsControlDown())
            {
                UpdateSavedFormations();
            }
            else
            {
                ApplySavedFormations();
            }

        }

        private void UpdateSavedFormations()
        {
            try
            {
                var troops = _partyVM.MainPartyTroops.ToList();
                foreach (var partyCharacterVm in troops)
                {
                    var name = partyCharacterVm?.Character?.Name?.ToString();
                    var formation = partyCharacterVm?.Character?.CurrentFormationClass;

                    if (name != null && formation != null)
                    {
                        var savedFormation = new SavedFormation(){ TroopName = name, Formation = formation.Value};
                        PartyManagerSettings.Settings.SavedFormations[name] = savedFormation;
                    }
                }
                PartyManagerSettings.Settings.SaveSettings();
            }
            catch (Exception e)
            {
                GenericHelpers.LogException("UpdateSavedFormations", e);
            }

        }

        private void ApplySavedFormations()
        {
            try
            {
                var troops = _partyVM.MainPartyTroops.ToList();
                foreach (var partyCharacterVm in troops)
                {
                    var name = partyCharacterVm?.Character?.Name?.ToString();

                    if (PartyManagerSettings.Settings.SavedFormations.ContainsKey(name))
                    {
                        var formation = PartyManagerSettings.Settings.SavedFormations[name];
                        partyCharacterVm.Character.CurrentFormationClass = formation.Formation;
                    }
                }

                var refreshCall = _partyVM.GetInitializeTroopListsMethod();
                refreshCall.Invoke(_partyVM, new object[] { });

            }
            catch (Exception e)
            {
                GenericHelpers.LogException("ApplySavedFormations", e);
            }
        }

        public void AltClick()
        {

        }
    }
}
