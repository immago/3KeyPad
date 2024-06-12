using CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;


namespace KeyPadCompanion
{
    public class AudioIOController
    {
        
        public List<MMDevice> GetInputDevices()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator(Guid.NewGuid());
            return enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
        }
        public List<MMDevice> GetOutputDevices()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator(Guid.NewGuid());
            return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
        }

        public void setDefaultAudioDevice(string id)
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator(Guid.NewGuid());
            MMDevice device = enumerator.GetDevice(id);
            enumerator.SetDefaultAudioEndpoint(device);
        }
    }
}

