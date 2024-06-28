using System;
using System.Collections.Generic;
using System.Timers;
using KeyPadCompanion.Data.Model;

namespace KeyPadCompanion.Data.Controllers
{
    public class LEDController
    {
        // Version
        public delegate void StateChangedHandler(int index, ButtonLedConfigurationElement state);
        public event StateChangedHandler? OnStateChangedHandler;

        private int timerDelay = 300;
        private Timer timer;
        private AudioIOController audioIOController = new AudioIOController();
        private List<ButtonLedConfigurationElement> currentLedStates; // Current led states in memory cache

        public LEDController() {

            // Set default states
            currentLedStates = new List<ButtonLedConfigurationElement>();
            foreach (List<ButtonLedConfigurationElement> led in Configuration.Instance.ButtonLedConfiguration)
            {
                currentLedStates.Add(led[0]);
            }

            // Create timer
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(TickEvent);
            timer.Interval = timerDelay;
            TickEvent(null, null);
        }

        public void Start() {
            timer.Enabled = true;
        }

        public void Stop() {
            timer.Enabled = false;
        }

        public ButtonLedConfigurationElement StateFor(int index)
        {
            return currentLedStates[index]; 
        }

        /*
        public void Reset() {

            // Set default states
            currentLedStates = new List<ButtonLedConfigurationElement>();
            foreach (List<ButtonLedConfigurationElement> led in Configuration.Instance.ButtonLedConfiguration)
            {
                currentLedStates.Add(led[0]);
            }
        }*/

        private void TickEvent(object? source, ElapsedEventArgs? e) {

            var allConf = Configuration.Instance.ButtonLedConfiguration;
            for (int ledNumber = 0; ledNumber < allConf.Count; ledNumber++)
            {
                var confs = allConf[ledNumber];
                foreach (var conf in confs)
                {
                    if (IsConditionFulfilled(conf))
                    {
                        bool isValueChanged = !currentLedStates[ledNumber].Equals(conf);
                        currentLedStates[ledNumber] = conf.Clone();
                        if (isValueChanged)
                        {
                            OnStateChangedHandler?.Invoke(ledNumber, conf);
                        }
                        break;
                    }
                }
            }

        }

        private bool IsConditionFulfilled(ButtonLedConfigurationElement led)
        {
            // Condition is disabled?
            if (!led.IsEnabled)
            {
                return false;
            }

            // Conditions is default?
            if (led.Condition == LedStateConditions.Default)
            {
                return true;
            }

            // Is mic muted?
            if (led.Condition == LedStateConditions.MicprophoneIsMuted)
            {
                var mic = audioIOController.GetCurrentInputDevice();
                return mic.AudioEndpointVolume?.Mute ?? false;
            }

            if (led.Condition == LedStateConditions.IsInputSelected)
            {
                var mic = audioIOController.GetCurrentInputDevice();
                return mic.ID == led.InputDeviceId;
            }

            return false; 
        }
    }
}
