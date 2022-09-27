using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialCom
{
    public static class SendCommand
    {
        const string RAW_HELP_MESSAGE = @"
List of send command arguments
    help                Shows this message
    raws <message>      Send string message
    rawb <sequence>     Send escape sequence command (escape character and left brackets are automatically added)
";
        
        public static string Process(string data)
        {
            if (data == null || data.Length <= 0 || data.StartsWith("help"))
                return RAW_HELP_MESSAGE;
            
            if (!(Program.SerialPort?.IsOpen) ?? true)
                return "No port was opened";

            int firstSpaceIndex = data.IndexOf(' ');
            if (firstSpaceIndex <= 0)
                return RAW_HELP_MESSAGE;

            string arg = data.Substring(0, firstSpaceIndex);
            string msg = data.Substring(firstSpaceIndex + 1);
            msg = string.Join("", msg.ToCharArray().Where(x=> (int)x < 255));

            switch (arg)
            {
                case "raws":
                    return TrySend(Encoding.ASCII.GetBytes(msg));
                
                case "rawb":
                    List<byte> bytes = new List<byte>
                    {
                        0x1B,
                        0x5B,
                    };
                    bytes.AddRange(Encoding.ASCII.GetBytes(msg));
                    return TrySend(bytes.ToArray());
                
                default:
                    return "Invalid arguments";
            }
        }

        private static string TrySend(byte[] data)
        {
            try
            {
                Program.SerialPort.Write(data, 0, data.Length);
                return "Message sent!";
            }
            catch (Exception e)
            {
                return "Failed to send message";
            }
        }
    }
}
