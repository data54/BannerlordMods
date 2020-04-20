using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyManager
{
    public class CustomComparer : IComparer<int>, IComparer<string>, IComparer<bool>
    {
        int multiplier = 1;

        public CustomComparer(CustomSortOrder sortOrder)
        {
            if (sortOrder.ToString().ToUpper().EndsWith("DESC"))
            {
                multiplier = -1;
            }
        }

        public int Compare(string x, string y)
        {
            int parsedX;
            int parsedY;

            if (int.TryParse(x, out parsedX) && int.TryParse(y, out parsedY))
            {
                return Compare(parsedX, parsedY);
            }


            return string.Compare(x, y, true) * multiplier;
        }

        public int Compare(int x, int y)
        {
            return x.CompareTo(y) * multiplier;
        }

        public int Compare(bool x, bool y)
        {
            return x.CompareTo(y) * multiplier;
        }
    }
}
