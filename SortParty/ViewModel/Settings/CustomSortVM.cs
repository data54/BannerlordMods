using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyManager.ViewModels;
using TaleWorlds.Library;

namespace PartyManager.ViewModel.Settings
{
    public class CustomSortVM : TaleWorlds.Library.ViewModel
    {
        private string _titleText;


        [DataSourceProperty]
        public String Name { get; set; }
        [DataSourceProperty]
        public string TitleText
        {
            get { return this._titleText; }
            set
            {
                if (!(value != this._titleText))
                    return;
                this._titleText = value;
                this.OnPropertyChanged(nameof(TitleText));
            }
        }

        public CustomSortVM()
        {
            Name = "Custom Sort";
            _titleText = "test";
        }
    }
}
