using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SortParty
{
    public class SortPartySettings
    {
        const string filePath = "..\\..\\Modules\\SortParty\\ModuleData\\SortPartySettings.xml";
        const int version = 3;

        public int? Version { get; set; }
        public bool EnableHotkey { get; set; }
        public bool EnableRecruitUpgradeSortHotkey { get; set; }
        public bool EnableAutoSort { get; set; }

        public SortType SortOrder { get; set; }


        [XmlIgnore]
        public static XmlSerializer Serializer
        {
            get
            {
                if (_serializer == null)
                {
                    _serializer = new XmlSerializer(typeof(SortPartySettings));
                }
                return _serializer;
            }
        }
        static XmlSerializer _serializer;


        private static SortPartySettings _settings;
        [XmlIgnore]
        public static SortPartySettings Settings
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

        public SortPartySettings()
        {
            EnableHotkey = true;
            EnableRecruitUpgradeSortHotkey = true;
            EnableAutoSort = true;
            Version = version;
            SortOrder = SortType.TierDesc;
        }

        private static SortPartySettings LoadSettings()
        {
            if (!File.Exists(filePath))
            {
                return CreateUpdateFile();
            }
            else
            {
                SortPartySettings settings;

                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    settings = Serializer.Deserialize(fs) as SortPartySettings;
                }

                if (settings.Version != version)
                {
                    settings = CreateUpdateFile(settings);
                }
                return settings;
            }
        }

        private static SortPartySettings CreateUpdateFile(SortPartySettings settings = null)
        {
            var newFileGenerated = false;
            if (settings == null)
            {
                settings = new SortPartySettings();
                newFileGenerated = true;
            }
            else
            {
                for(int i= settings.Version.Value; i<version; i++)
                {
                    switch(i)
                    {
                        case 1:
                            settings.EnableHotkey = true;
                            settings.EnableAutoSort = true;
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

            if(newFileGenerated)
            {
                var fullPath = Path.GetFullPath(filePath);
                InformationManager.DisplayMessage(new InformationMessage($"SortParty config generated at {fullPath}", Color.FromUint(4282569842U)));
            }

            return settings;
        }
    }

    public enum SortType
    {
        TierDesc,
        TierAsc,
        TierDescType,
        TierAscType,
        MountRangeTierDesc,
        MountRangeTierAsc,
        CultureTierDesc,
        CultureTierAsc
    }
}

