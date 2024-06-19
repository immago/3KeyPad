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
        // https://github.com/kitesurfer1404/WS2812FX
        /*
        #define FX_MODE_STATIC                   0
        #define FX_MODE_BLINK                    1
        #define FX_MODE_BREATH                   2
        #define FX_MODE_COLOR_WIPE               3
        #define FX_MODE_COLOR_WIPE_INV           4 
        #define FX_MODE_COLOR_WIPE_REV           5
        #define FX_MODE_COLOR_WIPE_REV_INV       6
        #define FX_MODE_COLOR_WIPE_RANDOM        7
        #define FX_MODE_RANDOM_COLOR             8
        #define FX_MODE_SINGLE_DYNAMIC           9
        #define FX_MODE_MULTI_DYNAMIC           10
        #define FX_MODE_RAINBOW                 11
        #define FX_MODE_RAINBOW_CYCLE           12
        #define FX_MODE_SCAN                    13
        #define FX_MODE_DUAL_SCAN               14
        #define FX_MODE_FADE                    15
        #define FX_MODE_THEATER_CHASE           16
        #define FX_MODE_THEATER_CHASE_RAINBOW   17
        #define FX_MODE_RUNNING_LIGHTS          18
        #define FX_MODE_TWINKLE                 19
        #define FX_MODE_TWINKLE_RANDOM          20
        #define FX_MODE_TWINKLE_FADE            21
        #define FX_MODE_TWINKLE_FADE_RANDOM     22
        #define FX_MODE_SPARKLE                 23
        #define FX_MODE_FLASH_SPARKLE           24
        #define FX_MODE_HYPER_SPARKLE           25
        #define FX_MODE_STROBE                  26
        #define FX_MODE_STROBE_RAINBOW          27
        #define FX_MODE_MULTI_STROBE            28
        #define FX_MODE_BLINK_RAINBOW           29
        #define FX_MODE_CHASE_WHITE             30
        #define FX_MODE_CHASE_COLOR             31
        #define FX_MODE_CHASE_RANDOM            32
        #define FX_MODE_CHASE_RAINBOW           33
        #define FX_MODE_CHASE_FLASH             34
        #define FX_MODE_CHASE_FLASH_RANDOM      35
        #define FX_MODE_CHASE_RAINBOW_WHITE     36
        #define FX_MODE_CHASE_BLACKOUT          37
        #define FX_MODE_CHASE_BLACKOUT_RAINBOW  38
        #define FX_MODE_COLOR_SWEEP_RANDOM      39
        #define FX_MODE_RUNNING_COLOR           40
        #define FX_MODE_RUNNING_RED_BLUE        41
        #define FX_MODE_RUNNING_RANDOM          42
        #define FX_MODE_LARSON_SCANNER          43
        #define FX_MODE_COMET                   44
        #define FX_MODE_FIREWORKS               45
        #define FX_MODE_FIREWORKS_RANDOM        46
        #define FX_MODE_MERRY_CHRISTMAS         47
        #define FX_MODE_FIRE_FLICKER            48
        #define FX_MODE_FIRE_FLICKER_SOFT       49
        #define FX_MODE_FIRE_FLICKER_INTENSE    50
        #define FX_MODE_CIRCUS_COMBUSTUS        51
        #define FX_MODE_HALLOWEEN               52
        #define FX_MODE_BICOLOR_CHASE           53
        #define FX_MODE_TRICOLOR_CHASE          54
        #define FX_MODE_TWINKLEFOX              55
        #define FX_MODE_RAIN                    56
        #define FX_MODE_BLOCK_DISSOLVE          57
        #define FX_MODE_ICU                     58
        #define FX_MODE_DUAL_LARSON             59
        #define FX_MODE_RUNNING_RANDOM2         60
        #define FX_MODE_FILLER_UP               61
        #define FX_MODE_RAINBOW_LARSON          62
        #define FX_MODE_RAINBOW_FIREWORKS       63
        #define FX_MODE_TRIFADE                 64
        #define FX_MODE_VU_METER                65
        #define FX_MODE_HEARTBEAT               66
        #define FX_MODE_BITS                    67
        #define FX_MODE_MULTI_COMET             68
        #define FX_MODE_FLIPBOOK                69
        #define FX_MODE_POPCORN                 70
        #define FX_MODE_OSCILLATOR              71
        */
        static public Dictionary<int, string> LedModes = new Dictionary<int, string>()
        {
            { 0, "STATIC"},
            { 1, "BLINK"},
            { 2, "BREATH"},
            { 8, "RANDOM_COLOR"},
            { 11, "RAINBOW"}
        };

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
