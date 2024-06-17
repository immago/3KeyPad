using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace KeyPadCompanion.Data.Model
{

   

   

    
    [Serializable()]
    public class Configuration : ISerializable
    {
        static private string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KeyPadCompanion/Settings.xml");

        // Sngleton instance
        private static Configuration? _instance;
        public static Configuration Instance
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

        // Active input devices id's list
        [XmlArray("ActiveAudioInputDevices")]
        [XmlArrayItem("ActiveAudioInputDevice")]
        public List<string> ActiveAudioInputDevices = new List<string>();

        // Actions for buttons 0..2
        [XmlArray("ButtonActions")]
        [XmlArrayItem("ButtonAction")]
        public List<ButtonAction> ButtonActions = new List<ButtonAction>();



        private Configuration() { }

        static public void Save()
        {

            XmlSerializer ser = new XmlSerializer(typeof(Configuration));
            // write
            using (var stream = File.Create(filePath))
            {
                ser.Serialize(stream, Instance);
            }
        }

        private static Configuration Load()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(Configuration));
                using (var stream = File.OpenRead(filePath))
                {
                    return (Configuration?)ser.Deserialize(stream) ?? new Configuration();
                }
            }
            catch
            {
                return new Configuration();
            }
        }

        //Deserialization constructor.
        public Configuration(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            ComPortName = (string?)info.GetValue("ComPortName", typeof(string));
            ActiveAudioInputDevices = (List<string>?)info.GetValue("ActiveAudioInputDevices", typeof(List<string>)) ?? new List<string>();
            ButtonActions = (List<ButtonAction>?)info.GetValue("ButtonActions", typeof(List<ButtonAction>)) ?? new List<ButtonAction> { new ButtonAction(), new ButtonAction(), new ButtonAction() };
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            info.AddValue("ComPortName", ComPortName);
            info.AddValue("ActiveAudioInputDevices", ActiveAudioInputDevices);
            info.AddValue("ButtonActions", ButtonActions);
        }
    }
}
