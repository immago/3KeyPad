using KeyPadCompanion.Data.Model;
using System.Diagnostics;

namespace KeyPadCompanion.Data.Controllers
{
    // Performs actions based on stored settings
    public class ActionsController
    {

        private AudioIOController audioIOController = new AudioIOController();

        public void PerformAction(ButtonEventType buttonType, int buttonIndex)
        {
            var action = Configuration.Instance.ButtonActions[buttonIndex].GetValue(buttonType);
            switch (action)
            {
                case Actions.None:
                    break;

                case Actions.SwitchAudioInput:
                    SwitchAudioInput();
                    break;

                case Actions.SwitchAudioOutput:
                    SwitchAudioOutput();
                    break;

                case Actions.MuteMicrophone:
                    MuteMicrophone();
                    break;

                case Actions.EmulateKeyboard:
                    EmulateKeyboard();
                    break;
            }
        }

        private void SwitchAudioInput()
        {
            var devicesList = Configuration.Instance.ActiveAudioInputDevices;
            if (devicesList.Count == 0) { return; }

            var currentInputDevice = audioIOController.GetCurrentInputDevice();
            int index = devicesList.IndexOf(currentInputDevice.ID);
            if (index >= 0) {
                // Device is in list
                if (index < devicesList.Count - 1)
                {
                    // Select next device
                    audioIOController.setDefaultAudioDevice(devicesList[index+1]);
                }else
                {
                    // This is a last device. Select first device
                    audioIOController.setDefaultAudioDevice(devicesList[0]);
                }

            }else
            {
                // Device is not in list. Just selet first in list
                audioIOController.setDefaultAudioDevice(devicesList[0]);
            }
        }
        private void SwitchAudioOutput()
        {
            Debug.WriteLine("TODO: Not implemented");
        }

        private void MuteMicrophone()
        {
            audioIOController.GetCurrentInputDevice().ToggleMute();
        }

        private void EmulateKeyboard()
        {
            Debug.WriteLine("TODO: Not implemented");
        }
    }
}