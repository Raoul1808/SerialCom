using System;
using System.IO.Ports;

namespace SerialCom
{

    
    public struct CommandData
    {
        public string Command;
        public string Data;

        public static CommandData Incorrect => new CommandData {Command = null};
    }
    
    internal class Program
    {
        const string RAW_HELP_MESSAGE = @"
List of all commands
    help        Shows this message
    port        Port-related commands (connecting, disconnecting and listing)
    send        Send messages through a connected port
    exit        Close this program
";
        public static SerialPort SerialPort;

        public static void Main(string[] args)
        {
            Console.WriteLine("Type help to see all available commands");
            bool looping = true;
            while (looping)
            {
                CommandData command = Prompt();
                switch (command.Command)
                {
                    case null:
                        break;
                    
                    case "help":
                        Console.WriteLine(RAW_HELP_MESSAGE);
                        break;
                    
                    case "port":
                        Console.WriteLine(PortCommand.Process(command.Data));
                        Console.WriteLine();
                        break;
                    
                    case "send":
                        Console.WriteLine(SendCommand.Process(command.Data));
                        Console.WriteLine();
                        break;
                    
                    case "exit":
                        looping = false;
                        break;
                }
            }
        }

        private static CommandData Prompt()
        {
            Console.Write(">");
            string line = Console.ReadLine() ?? "";
            if (line.Length <= 0)
                return CommandData.Incorrect;
            
            int firstSpaceIndex = line.IndexOf(' ');
            if (firstSpaceIndex <= 0)
                return new CommandData
                {
                    Command = line,
                    Data = null,
                };

            return new CommandData
            {
                Command = line.Substring(0, firstSpaceIndex),
                Data = line.Substring(firstSpaceIndex + 1),
            };
        }
    }
}
