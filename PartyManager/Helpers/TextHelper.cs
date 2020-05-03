using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace PartyManager.Helpers
{
    public static class TextHelper
    {
        public static string GetText(string textAndKey)
        {
            return GetText(textAndKey, textAndKey);
        }

        public static string GetText(string id, string text)
        {
            return GetText(id, text, null);
        }

        public static string GetText(string id, string text, string[] args)
        {
            try
            {
                var textObject = new TextObject($"{{={id}}} {text}");

                var ret = textObject.ToString();
                ret = ret.Replace("\\n", "\n");
                ret = ret.Replace("~[", "{");
                ret = ret.Replace("]~", "}");

                if (args != null)
                {
                    return string.Format(ret, args);
                }
                return ret;

            }
            catch (Exception e)
            {
                GenericHelpers.LogException($"GetText({id})", e);
            }

            return text;
        }


    }
}
