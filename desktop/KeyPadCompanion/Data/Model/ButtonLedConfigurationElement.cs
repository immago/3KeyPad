using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Markup;

namespace KeyPadCompanion.Data.Model
{
    // Button configruation for specific condition
    [Serializable()]
    public class ButtonLedConfigurationElement
    {
        public bool IsEnabled { get; set; }
        public LedStateConditions Condition { get; set; }
        public string HexColor { get; set; }
        public int Mode { get; set; }
        public int Speed { get; set; }
        public string? InputDeviceId { get; set; } // For Condition == .IsInputSelected
        public bool HasParameters { get {  return Condition == LedStateConditions.IsInputSelected; } }

        public ButtonLedConfigurationElement()
        {
            IsEnabled = true;
            Condition = LedStateConditions.Default;
            HexColor = "#000000";
            Mode = 0;
            Speed = 1000;
            InputDeviceId = null;
        }

        public ButtonLedConfigurationElement Clone()
        {
            return new ButtonLedConfigurationElement() { IsEnabled = IsEnabled, Condition = Condition, HexColor = HexColor, Mode = Mode, Speed = Speed, InputDeviceId = InputDeviceId };
        }

        public override bool Equals(object? obj)
        {
            if (null == obj)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            var o = obj as ButtonLedConfigurationElement;

            return  this.IsEnabled == o.IsEnabled &&
                    this.Condition == o.Condition &&
                    this.HexColor == o.HexColor &&
                    this.Mode == o.Mode &&
                    this.Speed == o.Speed &&
                    this.InputDeviceId == o.InputDeviceId;
        }

        public override int GetHashCode()
        {   
            var summHash = this.IsEnabled.GetHashCode() + this.Condition.GetHashCode() + this.HexColor.GetHashCode() + this.Mode.GetHashCode() + this.Speed.GetHashCode() + (this.InputDeviceId?.GetHashCode() ?? 0);
            return summHash.GetHashCode();
        }
    }
}
