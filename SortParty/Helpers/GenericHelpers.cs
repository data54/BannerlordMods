using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaleWorlds.Core;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;

namespace PartyManager
{
    public static class GenericHelpers
    {
        public static MethodInfo GetMethod(string methodName, Type type)
        {
            return type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static object CallMethod(string methodName, Type type, params object[] args)
        {
            var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
            return method.Invoke(null, args);
        }

        public static object CallInstanceMethod<T>(string methodName, T reflectionObject, params object[] args)
        {
            var method = GetPrivateMethod<T>(methodName, reflectionObject);
            return method.Invoke(reflectionObject, args);
        }

        public static MethodInfo GetPrivateMethod<T>(string methodName, T reflectionObject)
        {
            return GetMethod(methodName, reflectionObject, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static MethodInfo GetMethod<T>(string methodName, T reflectionObject, BindingFlags bindingFlags)
        {
            if (reflectionObject == null) return null;
            try
            {
                return reflectionObject.GetType()?.GetMethod(methodName, bindingFlags);
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException($"GetMethod({typeof(T)?.ToString()})", ex);
            }
            return null;
        }

        public static T GetPrivateField<T, R>(R reflectionObject, string fieldName) where T : class
        {
            return GetField<T, R>(reflectionObject, fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        }
        public static T GetField<T, R>(R reflectionObject, string fieldName, BindingFlags bindingFlags) where T : class
        {
            if (reflectionObject == null) return null;
            try
            {
                var fieldInfo = reflectionObject?.GetType().GetField(fieldName, bindingFlags);
                return fieldInfo?.GetValue(reflectionObject) as T;
            }
            catch (Exception ex)
            {
                LogException($"GetPartyVM({typeof(R)?.ToString()})", ex);
            }
            return null;
        }

        public static void LogException(string method, Exception ex)
        {
            InformationManager.DisplayMessage(new InformationMessage($"PartyManager {method} exception: {ex.Message}"));
        }

        public static void LogDebug(string method, string message)
        {
            if (PartyManagerSettings.Settings.Debug)
            {
                InformationManager.DisplayMessage(new InformationMessage($"PartyManager({method}) Debug: {message}"));
            }
        }

        public static void LogMessage(string message)
        {
            InformationManager.DisplayMessage(new InformationMessage(message));
        }

        public static GauntletView CreateGauntletView(GauntletMovie gauntletMovie, GauntletView parent, Widget target)
        {
            try
            {
                var flags = BindingFlags.NonPublic | BindingFlags.Instance;
                var contructor = typeof(GauntletView).GetConstructors(flags)?.First();
                var view = contructor?.Invoke(new object[]
                {
                    gauntletMovie,
                    parent,
                    target
                }) as GauntletView;

                return view;
            }
            catch (Exception ex)
            {
                LogException("CreateGauntletView", ex);
            }
            return null;
        }

        public static string ConvertObjectToXML<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using(StringWriter sr = new StringWriter())
            {
                serializer.Serialize(sr, obj);
                return sr.ToString();
            }
            return null;
        }

        public static void RecursiveCompareObjects<T>(T object1, T object2)
        {
            List<string> results = CompareObjects(object1, object2);

        }

        private static List<string> CompareObjects<T>(T object1, T object2, string property = "root")
        {
            List<string> results = new List<string>();
            if (object1 == null || object2 == null)
            {
                return results;
            }
            var properties = object1.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var p1 = prop.GetValue(object1);
                var p2 = prop.GetValue(object2);
                if ((p1 == null && p2 != null) || (p1 != null && p2 == null) || !p1.Equals(p2))
                {
                    var propertyString = $"{property}.{prop.Name}";
                    results.Add($"{propertyString} not equal: {p1.ToString()} vs {p2.ToString()}");

                    if (object1 != null && object2 != null)
                    {
                        results.AddRange(CompareObjects(p1, p2, propertyString));
                    }
                }

            }

            return results;
        }

    }
}