using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace KeyPadCompanion
{
    [Serializable()]
    public class Configuration: ISerializable
    {
        static private string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KeyPadCompanion/Settings.xml");

        private static Configuration? _instance;
        public static Configuration instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Load();
                }

                return _instance;
            }
        }

        // Data
        public string? ComPortName;

        private Configuration() { }

        static public void Save()
        {
            XmlSerializer ser = new XmlSerializer(typeof(Configuration));
            // write
            using (var stream = File.Create(Configuration.filePath))
            {
                ser.Serialize(stream, Configuration.instance);
            }
        }

        private static Configuration Load()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(Configuration));
                using (var stream = File.OpenRead(Configuration.filePath))
                {
                    return ((Configuration?)ser.Deserialize(stream)) ?? new Configuration();
                }
            }catch
            {
                return new Configuration();
            }
        }

        //Deserialization constructor.
        public Configuration(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            ComPortName = (string?)info.GetValue("ComPortName", typeof(string));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            info.AddValue("ComPortName", ComPortName);
        }
    }
}
