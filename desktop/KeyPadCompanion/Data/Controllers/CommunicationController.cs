using System;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace KeyPadCompanion.Data.Controllers
{
    class CommunicationController
    {
        public enum ButtonClickType
        {
            Single,
            Double,
            Long
        }

        // Version
        public delegate void VersionResponseHandler(string version);
        public event VersionResponseHandler? OnVersionResponse;

        // Led state
        public delegate void LedResponseHandler(int index, int mode, byte r, byte g, byte b, int speed);
        public event LedResponseHandler? OnLedResponse;

        // Button
        public delegate void ButtonClickHandler(ButtonClickType type, int index);
        public event ButtonClickHandler? OnButtonClick;

        private SerialPort? port;

        // - Public 
        public CommunicationController(string portName)
        {
            port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
            port.NewLine = "\n";
            port.DtrEnable = true;    // Data-terminal-ready
            port.RtsEnable = true;    // Request-to-send

        }

        public void Start()
        {
            if (port == null) { return; }

            // ToDo: Try Catch
            try
            {
                port.Open();

                // Wait for connection
                Thread.Sleep(200);

                // Read some USBtoSerial junk
                port.ReadExisting();
                Thread.Sleep(200);

                // Only after this subscribe to data
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void Stop()
        {
            port?.Close();
            port = null;
        }

        public static string[] GetPortNames()
        {
            string[] ports = SerialPort.GetPortNames();
            return ports;
        }

        public void GetVersion()
        {
            if (port == null) { return; }
            if (!port.IsOpen) { return; }
            port?.WriteLine("v");
        }

        public void GetLed(int index)
        {
            if (port == null) { return; }
            if (!port.IsOpen) { return; }
            port?.WriteLine($"getled {index}");
        }

        public void SetLed(int index, int mode, byte r, byte g, byte b, int speed)
        {
            if (port == null) { return; }
            if (!port.IsOpen) { return; }
            port?.WriteLine($"setled {index} {mode} {r} {g} {b} {speed}");
        }

        // - Private
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (port == null) { return; }

            string text = port.ReadLine();
            string[] parts = text.Split(' ');

            //Debug.WriteLine(text);

            if (parts.Length == 0) { return; }
            string command = parts[0];

            // Version
            if (command == "ver" && parts.Length >= 2)
            {
                string version = parts[1];
                OnVersionResponse?.Invoke(version);
            }

            // Led
            if (command == "led" && parts.Length >= 7)
            {
                // led (N) (mode) (r) (g) (b) (speed)
                // led 1 1 255 255 255 255

                int index = int.Parse(parts[1]);
                int mode = int.Parse(parts[2]);
                byte r = byte.Parse(parts[3]);
                byte g = byte.Parse(parts[4]);
                byte b = byte.Parse(parts[5]);
                int speed = int.Parse(parts[6]);
                OnLedResponse?.Invoke(index, mode, r, g, b, speed);
            }

            // Single click
            if (command == "sclick" && parts.Length >= 2)
            {
                int index = int.Parse(parts[1]);
                OnButtonClick?.Invoke(ButtonClickType.Single, index);
            }

            // Double click
            if (command == "dclick" && parts.Length >= 2)
            {
                int index = int.Parse(parts[1]);
                OnButtonClick?.Invoke(ButtonClickType.Double, index);
            }

            // Long click
            if (command == "lclick" && parts.Length >= 2)
            {
                int index = int.Parse(parts[1]);
                OnButtonClick?.Invoke(ButtonClickType.Long, index);
            }
        }
    }
}
