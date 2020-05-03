using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PartyManager.Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using StringWriter = System.IO.StringWriter;

namespace PartyManager
{
    public static class GenericHelpers
    {
        public static MethodInfo GetPrivateMethod<T>(string methodName, T reflectionObject)
        {
            return GetMethod(methodName, reflectionObject, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        static List<WeaponClass> rangedWeaponClasses = new List<WeaponClass>() { WeaponClass.Crossbow, WeaponClass.Bow, WeaponClass.Javelin, WeaponClass.Stone };
        static List<WeaponClass> nonWeaponClasses = new List<WeaponClass>() { WeaponClass.Arrow, WeaponClass.Banner, WeaponClass.Bolt, WeaponClass.LargeShield, WeaponClass.SmallShield };

        public static string GetCharacterWeaponClass(CharacterObject character)
        {
            var ret = "";
            var weaponClass = character.FirstBattleEquipment?.GetEquipmentFromSlot(EquipmentIndex.Weapon0).Item
                ?.PrimaryWeapon?.WeaponClass;

            if (weaponClass != null && ((character.IsArcher && !rangedWeaponClasses.Contains(weaponClass.Value)) || (!character.IsArcher && rangedWeaponClasses.Contains(weaponClass.Value))))
            {
                if (character.IsHero)
                {
                    ret = TextHelper.GetText(weaponClass.ToString());
                    return $"Hero({ret})";
                }
                else
                {
                    var newWeaponClass = character.FirstBattleEquipment?.GetEquipmentFromSlot(EquipmentIndex.Weapon2).Item
                        ?.PrimaryWeapon?.WeaponClass;

                    //try to get the second weapon set's slot, otherwise try for the other hand's slot
                    if (newWeaponClass != null && !nonWeaponClasses.Contains(newWeaponClass.Value))
                    {
                        weaponClass = newWeaponClass;
                    }
                    else
                    {
                        newWeaponClass = character.FirstBattleEquipment?.GetEquipmentFromSlot(EquipmentIndex.Weapon1).Item
                            ?.PrimaryWeapon?.WeaponClass;
                        if (newWeaponClass != null && !nonWeaponClasses.Contains(newWeaponClass.Value))
                        {
                            weaponClass = newWeaponClass;
                        }
                    }
                }
            }

            ret = TextHelper.GetText(weaponClass.ToString());

            return ret;
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
            InformationManager.DisplayMessage(new InformationMessage($"PartyManager {method} exception: {ex.Message}", Color.ConvertStringToColor("#b51705FF")));
        }

        public static void LogDebug(string method, string message)
        {
            if (PartyManagerSettings.Settings.Debug)
            {
                InformationManager.DisplayMessage(new InformationMessage($"PartyManager({method}) Debug: {message}"));
            }
        }

        public static void LogMessage(string message, string color = "#ffffffFF")
        {
            InformationManager.DisplayMessage(new InformationMessage(message, Color.ConvertStringToColor(color)));
        }


        public static GauntletView CreateGauntletView(GauntletMovie gauntletMovie, GauntletView parent, Widget target)
        {
            try
            {
                var flags = BindingFlags.NonPublic | BindingFlags.Instance;
                var contructor = typeof(GauntletView).GetConstructors(flags)?.First();
                var view = contructor?.Invoke(new object[] { gauntletMovie, parent, target }) as GauntletView;


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

            using (StringWriter sr = new StringWriter())
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
