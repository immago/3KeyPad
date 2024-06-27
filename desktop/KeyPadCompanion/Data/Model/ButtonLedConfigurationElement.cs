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
        public Boolean IsEnabled { get; set; }
        public LedStateConditions Condition { get; set; }
        public String HexColor { get; set; }
        public int Mode { get; set; }
        public int Speed { get; set; }

        public ButtonLedConfigurationElement()
        {
            IsEnabled = true;
            Condition = LedStateConditions.Default;
            HexColor = "#000000";
            Mode = 0;
            Speed = 1000;
        }

        public ButtonLedConfigurationElement Clone()
        {
            return new ButtonLedConfigurationElement() { IsEnabled = IsEnabled, Condition = Condition, HexColor = HexColor, Mode = Mode, Speed = Speed };
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
                    this.Speed == o.Speed;
        }

        public override int GetHashCode()
        {   
            var summHash = this.IsEnabled.GetHashCode() + this.Condition.GetHashCode() + this.HexColor.GetHashCode() + this.Mode.GetHashCode() + this.Speed.GetHashCode();
            return summHash.GetHashCode();
        }
    }
}
