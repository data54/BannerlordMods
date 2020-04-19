using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Engine.Options;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace PartyManager.Settings.CustomSettingClasses
{
    public class GauntletBooleanData : IBooleanOptionData
    {
        public bool BoolValue;

        public GauntletBooleanData(bool value)
        {
            BoolValue = value;
        }

        public float GetDefaultValue()
        {
            return 0;
        }

        public void Commit()
        {

        }

        public float GetValue()
        {
            return BoolValue ? 1f : 0f;
        }

        public void SetValue(float value)
        {
            if (value == 1f)
            {
                BoolValue = true;
            }
            else
            {
                BoolValue = false;
            }
        }

        public object GetOptionType()
        {
            return CampaignOptionItemVM.OptionTypes.Boolean;
        }

        public bool IsNative()
        {
            return true;
        }
    }
}
