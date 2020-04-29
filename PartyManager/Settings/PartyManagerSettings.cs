using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PartyManager.Settings;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace PartyManager
{
    public class PartyManagerSettings
    {
        const string filePath = "..\\..\\Modules\\PartyManager\\ModuleData\\PartyManager.xml";
        const int version = 5;

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
        public bool DisableUpdatedTroopLabel { get; set; }
        public bool DisablePartyCompositionIcon { get; set; }
        public bool UseAdvancedPartyComposition { get; set; }
        public int StickySlots { get; set; }



        public bool Debug { get; set; }

        public CustomSortOrder CustomSortOrderField1 { get; set; }
        public CustomSortOrder CustomSortOrderField2 { get; set; }
        public CustomSortOrder CustomSortOrderField3 { get; set; }
        public CustomSortOrder CustomSortOrderField4 { get; set; }
        public CustomSortOrder CustomSortOrderField5 { get; set; }

        private List<SavedFormation> _formationSettings;

        public List<SavedFormation> FormationSettingsList
        {
            get
            {
                if (_formationSettings == null)
                {
                    _formationSettings = new List<SavedFormation>();
                }
                return _formationSettings;
            }
            set => _formationSettings = value;
        }

        private List<string> _ransomPrisonerBlackWhiteList;
        public List<string> RansomPrisonerBlackWhiteList
        {
            get
            {
                if (_ransomPrisonerBlackWhiteList == null)
                {
                    _ransomPrisonerBlackWhiteList= new List<string>();
                }

                return _ransomPrisonerBlackWhiteList;
            }
            set => _ransomPrisonerBlackWhiteList = value;
        }
        public bool RansomPrisonersUseWhitelist { get; set; }
        

        private List<string> _transferPrisonerBlackWhiteList;
        public List<string> TransferPrisonerBlackWhiteList
        {
            get
            {
                if (_transferPrisonerBlackWhiteList == null)
                {
                    _transferPrisonerBlackWhiteList = new List<string>();
                }

                return _transferPrisonerBlackWhiteList;
            }
            set => _transferPrisonerBlackWhiteList = value;
        }

        public bool TransferPrisonersUseWhitelist { get; set; }


        private List<string> _recruitPrisonerBlackWhiteList;
        public List<string> RecruitPrisonerBlackWhiteList
        {
            get
            {
                if (_recruitPrisonerBlackWhiteList == null)
                {
                    _recruitPrisonerBlackWhiteList = new List<string>();
                }

                return _recruitPrisonerBlackWhiteList;
            }
            set => _recruitPrisonerBlackWhiteList = value;
        }
        public bool RecruitPrisonersUseWhitelist { get; set; }


        private List<string> _upgradeTroopsBlackWhiteList;
        public List<string> UpgradeTroopsBlackWhiteList
        {
            get
            {
                if (_upgradeTroopsBlackWhiteList == null)
                {
                    _upgradeTroopsBlackWhiteList = new List<string>();
                }

                return _upgradeTroopsBlackWhiteList;
            }
            set => _upgradeTroopsBlackWhiteList = value;
        }
        public bool UpgradeTroopsUseWhitelist { get; set; }

        private Dictionary<string, SavedFormation> _savedFormations;
        [XmlIgnore]
        public Dictionary<string, SavedFormation> SavedFormations
        {
            get
            {
                if (_savedFormations == null)
                {
                    _savedFormations= new Dictionary<string, SavedFormation>();
                }

                return _savedFormations;
            }
            set => _savedFormations = value;
        }


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
            FormationSettingsList = SavedFormations.Values.ToList();
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
                        settings?.LoadFormationDictionary();
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

        public void LoadFormationDictionary()
        {
            _savedFormations = new Dictionary<string, SavedFormation>();

            foreach (var formationSetting in FormationSettingsList)
            {
                _savedFormations[formationSetting.TroopName] = formationSetting;
            }
        }
    }

}

