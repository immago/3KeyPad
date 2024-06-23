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
        MicprophoneIsMuted
    }

    /*
    public class EnumToItemsSource : MarkupExtension
    {
        private readonly Type _type;

        public EnumToItemsSource(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type)
                .Cast<object>()
                .Select(e => new { Value = (int)e, DisplayName = e.ToString() });
        }
    }*/
}
