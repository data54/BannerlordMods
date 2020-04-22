using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyManager
{
    public class SavedTroopUpgradePath
    {
        public string UnitName { get; set; }
        public int? TargetUpgrade { get; set; }
        public bool EvenSplit { get; set; }
    }
}
