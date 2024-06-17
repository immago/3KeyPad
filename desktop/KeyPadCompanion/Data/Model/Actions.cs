using System;
using System.Runtime.Serialization;


namespace KeyPadCompanion.Data.Model
{
    // Avaliable actions for buttons
    [Serializable()]
    public enum Actions
    {
        [EnumMember(Value = "None")]
        None,

        [EnumMember(Value = "SwitchAudioInput")]
        SwitchAudioInput,

        [EnumMember(Value = "SwitchAudioOutput")]
        SwitchAudioOutput,

        [EnumMember(Value = "MuteMicrophone")]
        MuteMicrophone,

        [EnumMember(Value = "EmulateKeyboard")]
        EmulateKeyboard
    }
}
