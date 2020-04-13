using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SortParty
{
    public class SortPartySettings
    {
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
            if (settings == null)
            {
                settings = new SortPartySettings() { Version = version, SortOrder = SortType.TierDesc };
            }
            else
            {
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
            return settings;
        }

        public int? Version { get; set; }

        public SortType SortOrder { get; set; }


    }

    public enum SortType
    {
        TierDesc,
        TierAsc,
        TierDescType,
        TierAscType,
        MountRangeTierDesc,
        MountRangeTierAsc
    }
}

