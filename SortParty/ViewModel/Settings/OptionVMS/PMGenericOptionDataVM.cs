using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace PartyManager.ViewModel.Settings.OptionVMS
{
    public interface IPMOptions
    {

    }

    public class PMGenericOptionDataVM<T> : TaleWorlds.Library.ViewModel, IPMOptions where T : IComparable
    {
        internal int _optionTypeId = -1;
        internal TextObject _nameObj;
        internal TextObject _descriptionObj;
        internal string _description;
        internal string _name;
        internal string[] _imageIDs;

        private T _value;
        [DataSourceProperty]
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }


        internal T _initialValue;
        internal Action<T> _updateCall;
        internal CampaignOptionItemVM.OptionTypes _optionType;

        public PMGenericOptionDataVM(
            T value,
            string name,
            string description
            , Action<T> updateCall, CampaignOptionItemVM.OptionTypes optionType)
        {
            Initialize(value, name, description, updateCall, optionType);
            RefreshValues();
        }


        internal void Initialize(T value,
            string name,
            string description
            , Action<T> updateCall, CampaignOptionItemVM.OptionTypes optionType)
        {
            _name = name;
            this._nameObj = new TextObject(name, new Dictionary<string, TextObject>());
            this._descriptionObj = new TextObject(description, new Dictionary<string, TextObject>());
            _updateCall = updateCall;
            _optionType = optionType;
            _initialValue = value;
            _value = value;

            switch (_optionType)
            {
                case CampaignOptionItemVM.OptionTypes.Boolean:
                    _optionTypeId = 0;
                    this.ImageIDs = new string[2]
                    {
                        _name.ToString() + "_0",
                        _name.ToString() + "_1"
                    };
                    break;
                case CampaignOptionItemVM.OptionTypes.Numeric:
                    _optionTypeId = 1;
                    break;
                case CampaignOptionItemVM.OptionTypes.Selection:
                    _optionTypeId = 3;
                    break;
            }

        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            this.Name = this._nameObj.ToString();
            this.Description = this._descriptionObj.ToString();
        }

        public object GetOptionType()
        {
            return _optionType;
        }

        public object GetOptionData()
        {
            return Value;
        }

        [DataSourceProperty]
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                if (!(value != this._description))
                    return;
                this._description = value;
                this.OnPropertyChanged(nameof(Description));
            }
        }

        [DataSourceProperty]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (!(value != this._name))
                    return;
                this._name = value;
                this.OnPropertyChanged(nameof(Name));
            }
        }

        [DataSourceProperty]
        public string[] ImageIDs
        {
            get
            {
                return this._imageIDs;
            }
            set
            {
                if (value == this._imageIDs)
                    return;
                this._imageIDs = value;
                this.OnPropertyChanged(nameof(ImageIDs));
            }
        }

        [DataSourceProperty]
        public int OptionTypeID
        {
            get
            {
                return this._optionTypeId;
            }
            set
            {
                if (value == this._optionTypeId)
                    return;
                this._optionTypeId = value;
                this.OnPropertyChanged(nameof(OptionTypeID));
            }
        }

        public void UpdateValue()
        {
            _updateCall.Invoke(Value);
        }

        public virtual void Cancel()
        {

        }

        public virtual bool IsChanged()
        {
            return _value.Equals(_initialValue);
        }

        public virtual void SetValue(T value)
        {
            _value = value;
        }

        public virtual void ResetData()
        {
            Value = _initialValue;
        }

        internal T ConvertToT<R>(R input, T oldValue)
        {
            try
            {
                if (typeof(T) == typeof(CustomSortOrder))
                {
                    return (T) Enum.Parse(typeof(CustomSortOrder), input.ToString());
                }
                else
                {
                    return (T) Convert.ChangeType(input, typeof(T));
                }
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("ConvertToT", ex);
            }

            return oldValue;
        }


        [DataSourceProperty]
        public bool OptionValueAsBoolean
        {
            get
            {
                if (typeof(T) == typeof(bool))
                {
                    return bool.Parse(Value.ToString());
                }

                return false;
            }
            set
            {
                if (typeof(T) == typeof(bool))
                {
                    var result = ConvertToT(value, Value);
                    if (!result.Equals(Value))
                    {
                        SetValue(result);
                        UpdateValue();
                    }
                }
            }
        }
    }
}
