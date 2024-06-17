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

        public Actions GetValue(ButtonType type)
        {
            switch (type)
            {
                case ButtonType.SinglePress:
                    return SinglePress;

                case ButtonType.DoublePress:
                    return DoublePress;

                case ButtonType.LongPress:
                    return LongPress;

                default:
                    return SinglePress;
            }
        }

        public void SetValue(Actions value, ButtonType type)
        {
            switch (type)
            {
                case ButtonType.SinglePress:
                    SinglePress = value;
                    break;

                case ButtonType.DoublePress:
                    DoublePress = value;
                    break;

                case ButtonType.LongPress:
                    LongPress = value;
                    break;
            }
        }
    }

}
