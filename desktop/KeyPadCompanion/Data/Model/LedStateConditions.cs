using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Markup;

namespace KeyPadCompanion.Data.Model
{
    [Serializable()]
    public enum LedStateConditions
    {
        [EnumMember(Value = "Default")]
        Default,

        [EnumMember(Value = "MicprophoneIsMuted")]
        MicprophoneIsMuted,

        [EnumMember(Value = "IsInputSelected")]
        IsInputSelected
    }
}
