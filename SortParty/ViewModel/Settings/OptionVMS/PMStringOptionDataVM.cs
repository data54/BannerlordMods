using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace PartyManager.ViewModel.Settings.OptionVMS
{
    public class PMStringOptionDataType<T> : PMGenericOptionDataVM<T> where T : IComparable
    {
        private MBBindingList<string> _dropdownOptions;
        private int _initialIndex = 0;
        private int _selectedIndex = 0;
        public SelectorVM<SelectorItemVM> _selector;



        public PMStringOptionDataType(
            T value,
            string name,
            string description,
            MBBindingList<string> dropdownOptions
            , Action<T> updateCall, CampaignOptionItemVM.OptionTypes optionType) : base(value, name, description, updateCall, optionType)
        {
            _dropdownOptions = dropdownOptions;

            List<TextObject> textObjectList = new List<TextObject>();

            _imageIDs = new string[dropdownOptions.Count];

            for (int i = 0; i < dropdownOptions.Count; i++)
            {
                _imageIDs[i] = $"{name}_{i}";
                textObjectList.Add(new TextObject(dropdownOptions[i]));
            }
            _selectedIndex = _initialIndex = _dropdownOptions.IndexOf(value.ToString());


            this._selector = new SelectorVM<SelectorItemVM>((IEnumerable<TextObject>)textObjectList, _initialIndex, new Action<SelectorVM<SelectorItemVM>>(this.UpdateValue));
            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            this._selector?.RefreshValues();
        }

        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> Selector
        {
            get
            {
                SelectorVM<SelectorItemVM> selector = this._selector;
                return this._selector;
            }
            set
            {
                if (value == this._selector)
                    return;
                this._selector = value;
                this.OnPropertyChanged(nameof(Selector));
            }
        }

        public void UpdateValue(SelectorVM<SelectorItemVM> selector)
        {
            try
            {
                if (selector.SelectedIndex < 0)
                    return;
                var selectedOption = _dropdownOptions[selector.SelectedIndex];
                var newValue = ConvertToT(selectedOption, _initialValue);
                Value = newValue;
                _updateCall(Value);
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException($"StringOptionDataVM.{_name}.UpdateValue", ex);
            }
        }


        public override void Cancel()
        {
            this.Selector.SelectedIndex = _initialIndex;
            this.UpdateValue();
        }

        public void SetValue(int value)
        {
            this.Selector.SelectedIndex = value;
        }

        public override void ResetData()
        {
            this.Selector.SelectedIndex = 0;
        }

        public override bool IsChanged()
        {
            return this._initialIndex != this.Selector.SelectedIndex;
        }

    }
}
