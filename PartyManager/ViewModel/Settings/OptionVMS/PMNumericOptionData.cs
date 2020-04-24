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
    public class PMNumericOptionData : PMGenericOptionDataVM<float>
    {

        private float _min, _max, _optionValue;
        [DataSourceProperty]
        public float Min
        {
            get
            {
                return this._min;
            }
            set
            {
                if ((double)value == (double)this._min)
                    return;
                this._min = (int)value;
                this.OnPropertyChanged(nameof(Min));
            }
        }

        [DataSourceProperty]
        public float Max
        {
            get
            {
                return this._max;
            }
            set
            {
                if ((double)value == (double)this._max)
                    return;
                this._max = value;
                this.OnPropertyChanged(nameof(Max));
            }
        }


        [DataSourceProperty]
        public float OptionValue
        {
            get => (int)this._optionValue;
            set
            {
                try
                {
                    if (value == this._optionValue)
                        return;
                    this._optionValue = (int)value;
                    this.OnPropertyChanged(nameof(OptionValue));
                    this.OnPropertyChanged("OptionValueAsString");
                    this.UpdateValue();
                }
                catch (Exception ex)
                {

                }
            }
        }


        private bool _isDiscrete;
        [DataSourceProperty]
        public bool IsDiscrete
        {
            get
            {
                return this._isDiscrete;
            }
            set
            {
                if (value == this._isDiscrete)
                    return;
                this._isDiscrete = value;
                this.OnPropertyChanged(nameof(IsDiscrete));
            }
        }

        [DataSourceProperty]
        public string OptionValueAsString
        {
            get
            {
                return !this.IsDiscrete ? this._optionValue.ToString("F") : ((int)this._optionValue).ToString();
            }
        }



        public PMNumericOptionData(
            float value,
            string name,
            string description
            , Action<float> updateCall, CampaignOptionItemVM.OptionTypes optionType, float min, float max, bool discrete=false) : base(value, name, description, updateCall, optionType)
        {
            _isDiscrete = discrete;
            _min = min;
            _max = max;
            _optionValue = value;
            Initialize(value, name, description, updateCall, optionType);
            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
        }

        public override void SetValue(float value)
        {
            this.Value = !this.IsDiscrete ? value : ((int)value);
        }

        public override void UpdateValue()
        {
            SetValue(this.OptionValue);
            base.UpdateValue();
        }
    }
}
