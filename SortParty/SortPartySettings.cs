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
        public int? Version { get; set; }
        public bool EnableHotkey { get; set; }
        public bool EnableAutoSort { get; set; }

        public SortType SortOrder { get; set; }


        const string filePath = "..\\..\\Modules\\SortParty\\ModuleData\\SortPartySettings.xml";
        const int version = 2;

        private static SortPartySettings _settings;
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
                var serializer = new XmlSerializer(typeof(SortPartySettings));
                SortPartySettings settings;

                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    settings = serializer.Deserialize(fs) as SortPartySettings;
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

            var serializer = new XmlSerializer(typeof(SortPartySettings));

            var saveDirectory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }


            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, settings);
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

