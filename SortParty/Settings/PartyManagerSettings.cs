using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace PartyManager
{
    public class PartyManagerSettings
    {
        const string filePath = "..\\..\\Modules\\PartyManager\\ModuleData\\PartyManager.xml";
        const int version = 4;

        public int? Version { get; set; }
        public bool EnableHotkey { get; set; }
        public bool EnableRecruitUpgradeSortHotkey { get; set; }
        public bool EnableSortTypeCycleHotkey { get; set; }
        public bool EnableAutoSort { get; set; }
        public bool CavalryAboveFootmen { get; set; }
        public bool MeleeAboveArchers { get; set; }
        public bool SortAfterRecruitAllUpgradeAllClick { get; set; }
        public bool HideUIWidgets { get; set; }
        public bool DisableCustomUpgradePaths { get; set; }

        public bool Debug { get; set; }

        public CustomSortOrder CustomSortOrderField1 { get; set; }
        public CustomSortOrder CustomSortOrderField2 { get; set; }
        public CustomSortOrder CustomSortOrderField3 { get; set; }
        public CustomSortOrder CustomSortOrderField4 { get; set; }
        public CustomSortOrder CustomSortOrderField5 { get; set; }


        private List<SavedTroopUpgradePath> _savedTroopUpgradePaths;

        public List<SavedTroopUpgradePath> SavedTroopUpgradePaths
        {
            get
            {
                if (_savedTroopUpgradePaths == null)
                {
                    _savedTroopUpgradePaths = new List<SavedTroopUpgradePath>();
                }

                return _savedTroopUpgradePaths;
            }
            set { _savedTroopUpgradePaths = value; }

        }

        public SortType SortOrder { get; set; }

        [XmlIgnore]
        public string SortOrderString => GetSortTypeString(SortOrder);

        [XmlIgnore]
        public string NextSortOrderString => GetCycledSortType(false).ToString();

        [XmlIgnore]
        public string PreviousSortOrderString => GetCycledSortType(true).ToString();

        [XmlIgnore]
        private static XmlSerializer Serializer
        {
            get
            {
                if (_serializer == null)
                {
                    _serializer = new XmlSerializer(typeof(PartyManagerSettings));
                }
                return _serializer;
            }
        }
        static XmlSerializer _serializer;

        public string NewFileMessage = null;

        private static PartyManagerSettings _settings;
        [XmlIgnore]
        public static PartyManagerSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = LoadSettings();
                }
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        public void CycleSortType()
        {
            CycleSortType(false);


            InformationManager.DisplayMessage(new InformationMessage($"PartyManager sort changed to {GetSortTypeString(SortOrder)}", Color.FromUint(4282569842U)));
        }

        public void CycleSortType(bool backward)
        {
            SortOrder = GetCycledSortType(backward);
            CreateUpdateFile(this);
        }

        private int? sortModulus;
        public SortType GetCycledSortType(bool backward = false)
        {

            if (!sortModulus.HasValue)
            {
                sortModulus = (int)Enum.GetValues(typeof(SortType)).Cast<SortType>().Max() + 1;
            }

            var change = backward ? -1 : 1;

            var intValue = (int)SortOrder;

            var intResult = (intValue + change) % sortModulus;
            if (intResult < 0)
            {
                intResult = sortModulus - 1;
            }

            return (SortType)intResult;
        }

        public string GetSortTypeString(SortType type)
        {
            if (type == SortType.Custom)
            {
                return $"Custom({CustomSortOrderField1},{CustomSortOrderField2},{CustomSortOrderField3},{CustomSortOrderField4},{CustomSortOrderField5})";
            }
            return type.ToString();
        }

        public PartyManagerSettings()
        {
            EnableHotkey = true;
            EnableRecruitUpgradeSortHotkey = true;
            EnableSortTypeCycleHotkey = true;
            CavalryAboveFootmen = true;
            MeleeAboveArchers = true;
            Version = version;
            SortOrder = SortType.TierDesc;
        }

        public static void ReloadSettings()
        {
            _settings = LoadSettings();
            GenericHelpers.LogMessage("Settings Reloaded");
        }

        public void SaveSettings()
        {
            CreateUpdateFile(this);
            GenericHelpers.LogDebug("PartyManagerSettings", "Settings Saved");
        }

        public static PartyManagerSettings LoadSettings()
        {
            if (!File.Exists(filePath))
            {
                return CreateUpdateFile();
            }
            else
            {
                PartyManagerSettings settings;

                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    try
                    {
                        settings = Serializer.Deserialize(fs) as PartyManagerSettings;
                    }
                    catch (Exception ex)
                    {
                        GenericHelpers.LogException("LoadSettings", ex);
                        settings = new PartyManagerSettings();
                    }
                }

                if (settings.Version != version)
                {
                    settings = CreateUpdateFile(settings);
                }
                return settings;
            }
        }

        private static PartyManagerSettings CreateUpdateFile(PartyManagerSettings settings = null)
        {
            try
            {
                var newFileGenerated = false;
                if (settings == null)
                {
                    settings = new PartyManagerSettings();
                    newFileGenerated = true;
                }
                else
                {
                    for (int i = settings.Version.Value; i < version; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                settings.EnableHotkey = true;
                                settings.EnableAutoSort = true;
                                break;
                            case 2:
                                settings.EnableSortTypeCycleHotkey = true;
                                settings.EnableRecruitUpgradeSortHotkey = true;
                                break;
                            case 3:
                                settings.CavalryAboveFootmen = true;
                                settings.MeleeAboveArchers = true;
                                break;
                        }
                    }

                    settings.Version = version;
                }


                var saveDirectory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(saveDirectory))
                {
                    Directory.CreateDirectory(saveDirectory);
                }


                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    Serializer.Serialize(fs, settings);
                }

                if (newFileGenerated)
                {
                    var fullPath = Path.GetFullPath(filePath);
                    settings.NewFileMessage = $"PartyManager config generated at {fullPath}";
                }
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("PartyManagerSettings.CreateUpdateFile", ex);
            }

            return settings;
        }

        public PartyManagerSettings Clone()
        {
            try
            {
                //I feel dirty
                PartyManagerSettings clone;
                using (MemoryStream ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, this);

                    ms.Seek(0, SeekOrigin.Begin);
                    clone = Serializer.Deserialize(ms) as PartyManagerSettings;
                }

                return clone;
            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("PartyManagerSettings.Clone", ex);
            }

            return null;
        }

        public static MBBindingList<string> GetSelectableSortOrderStrings()
        {
            var sortTypes = new MBBindingList<string>();
            try
            {

                foreach (var sort in Enum.GetValues(typeof(CustomSortOrder)))
                {
                    if ((int)sort >= 0)
                    {
                        sortTypes.Add(sort.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                GenericHelpers.LogException("PartyManagerSettings.GetSelectableSortOrders", ex);
            }

            return sortTypes;
        }
    }

    public enum SortType
    {
        CustomUpgrades = -3,
        RecruitUpgrade = -2,
        Default = -1,
        TierDesc = 0,
        TierAsc = 1,
        TierDescType = 2,
        TierAscType = 3,
        MountRangeTierDesc = 4,
        MountRangeTierAsc = 5,
        CultureTierDesc = 6,
        CultureTierAsc = 7,
        RangeMountTierDesc = 8,
        RangeMountTierAsc = 9,
        Custom = 10
    }

    public enum CustomSortOrder
    {
        None,

        TierAsc,
        TierDesc,

        MountedAsc,
        MountedDesc,

        MeleeAsc,
        MeleeDesc,

        CultureAsc,
        CultureDesc,

        UnitNameAsc,
        UnitNameDesc
    }
}

