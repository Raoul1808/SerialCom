using System;
using System.IO.Ports;

namespace SerialCom
{
    public static class PortCommand
    {
        const string RAW_HELP_MESSAGE = @"
List of port command arguments
    help                Shows this message
    connect <name>      Connect to the specified port
    disconnect          Disconnect from the currently connected port
    list                List all available ports
";
        
        public static string Process(string data)
        {
            if (data == null || data.Length <= 0 || data.StartsWith("help"))
                return RAW_HELP_MESSAGE;

            var args = data.Split(' ');
            if (args.Length <= 0)
                return RAW_HELP_MESSAGE;
            
            string command = args[0];

            switch (command)
            {
                case "connect" when args.Length >= 2:
                    try
                    {
                        if (Program.SerialPort != null)
                        {
                            return "Port already operational";
                        }

                        string port = args[1];
                        Program.SerialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
                        Program.SerialPort.Open();
                        return "Successfully opened port";
                    }
                    catch (Exception e)
                    {
                        return "Failed to open specified port";
                    }
                
                case "disconnect":
                    if (Program.SerialPort == null)
                    {
                        return "There is no port to close";
                    }

                    string portName = Program.SerialPort.PortName;
                    Program.SerialPort.Close();
                    Program.SerialPort = null;
                    return $"Successfully closed port {portName}";
                
                case "list":
                    var ports = SerialPort.GetPortNames();
                    if (ports.Length <= 0)
                        return "No ports available";
                    string ret = "Available ports:";
                    foreach (string port in ports)
                    {
                        ret += "\n" + port;
                    }
                    return ret;
                
                default:
                    return "Invalid arguments";
            }
        }
    }
}
