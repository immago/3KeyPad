using CoreAudio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace KeyPadCompanion.Data.Controllers
{
    public class AudioIOController
    {
        private MMDeviceEnumerator enumerator = new MMDeviceEnumerator(Guid.NewGuid());

        public List<MMDevice> GetInputDevices()
        {
            return enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
        }
        public List<MMDevice> GetOutputDevices()
        {
            return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
        }

        public void setDefaultAudioDevice(string id)
        {
            try
            {
                MMDevice device = enumerator.GetDevice(id);
                enumerator.SetDefaultAudioEndpoint(device);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public MMDevice GetCurrentInputDevice()
        {
            return enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
        }

        public MMDevice GetCurrentOutputDevice(Role role)
        {
            return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, role);
        }
    }

    public static class AudioIOControllerExtensions
    {
        public static void Mute(this MMDevice device)
        {
            device.AudioEndpointVolume!.Mute = true;
        }

        public static void UnMute(this MMDevice device)
        {
            device.AudioEndpointVolume!.Mute = false;
        }
        public static void ToggleMute(this MMDevice device)
        {
            device.AudioEndpointVolume!.Mute = !device.AudioEndpointVolume!.Mute;
        }
    }
}

