using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace SortParty
{
    public static class GenericHelpers
    {
        public static MethodInfo GetPrivateMethod<T>(string methodName, T reflectionObject)
        {
            return GetMethod(methodName, reflectionObject, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static MethodInfo GetMethod<T>(string methodName, T reflectionObject, BindingFlags bindingFlags)
        {
            return reflectionObject.GetType().GetMethod(methodName, bindingFlags);
        }


        public static T GetPrivateField<T, R>(R reflectionObject, string fieldName) where T : class
        {
            return GetField<T, R>(reflectionObject, fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        }
        public static T GetField<T, R>(R reflectionObject, string fieldName, BindingFlags bindingFlags) where T : class
        {
            try
            {
                var fieldInfo = reflectionObject.GetType().GetField(fieldName, bindingFlags);
                return fieldInfo?.GetValue(reflectionObject) as T;
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException($"GetPartyVM({typeof(R)?.ToString()})", ex);
            }
            return null;
        }

        public static void LogException(string method, Exception ex)
        {
            InformationManager.DisplayMessage(new InformationMessage($"SortParty {method} exception: {ex.Message}"));
        }

        public static void LogDebug(string method, string message)
        {
            if (SortPartySettings.Settings.Debug)
            {
                InformationManager.DisplayMessage(new InformationMessage($"SortParty({method}) Debug: {message}"));
            }
        }
    }
}
