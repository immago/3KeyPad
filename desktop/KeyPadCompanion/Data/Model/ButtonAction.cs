using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPadCompanion.Data.Model
{
    // Button actions for different events
    [Serializable()]
    public class ButtonAction
    {
        public Actions SinglePress = Actions.None;
        public Actions DoublePress = Actions.None;
        public Actions LongPress = Actions.None;

        public Actions GetValue(ButtonEventType type)
        {
            switch (type)
            {
                case ButtonEventType.SinglePress:
                    return SinglePress;

                case ButtonEventType.DoublePress:
                    return DoublePress;

                case ButtonEventType.LongPress:
                    return LongPress;

                default:
                    return SinglePress;
            }
        }

        public void SetValue(Actions value, ButtonEventType type)
        {
            switch (type)
            {
                case ButtonEventType.SinglePress:
                    SinglePress = value;
                    break;

                case ButtonEventType.DoublePress:
                    DoublePress = value;
                    break;

                case ButtonEventType.LongPress:
                    LongPress = value;
                    break;
            }
        }
    }

}
