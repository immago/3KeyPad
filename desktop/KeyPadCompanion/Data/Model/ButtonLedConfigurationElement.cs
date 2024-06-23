using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Markup;

namespace KeyPadCompanion.Data.Model
{
    // Button configruation for specific condition
    [Serializable()]
    class ButtonLedConfigurationElement
    {
       
        

        public Boolean IsEnabled { get; set; }
        public LedStateConditions Condition { get; set; }
        public String HexColor { get; set; }
        public int Mode { get; set; }
        public int Speed { get; set; }
    }
}
