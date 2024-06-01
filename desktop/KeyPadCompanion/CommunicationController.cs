using System;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace KeyPadCompanion
{
    class CommunicationController
    {
        // Version
        public delegate void VersionResponseHandler(string version);
        public event VersionResponseHandler? OnVersionResponse;

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
            port.Open();

            // Wait for connection
            Thread.Sleep(200);

            // Read some USBtoSerial junk
            port.ReadExisting();
            Thread.Sleep(200);

            // Only after this subscribe to data
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
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


        // - Private
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (port == null) { return; }

            string text = port.ReadLine();
            string[] parts = text.Split(' ');

            Debug.Write(text);

            if (parts.Length == 0) { return; }
            string command = parts[0];

            // Version
            if (command == "ver")
            {
                string version = parts[1];
                OnVersionResponse?.Invoke(version);
            }

            
        }
    }
}
