using System;
using System.IO;

namespace Minecraft.GUI
{
    public class Logger
    {
        public string DateFormat { get; set; } = "dd.MM.yyyy, HH:mm:ss";
        public string Name { get; set; } = "Default";
        public string DumpLocation { get; set; }

        private void Log(string message, string type, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"({Name})[{type}] {{{DateTime.Now.ToString(DateFormat)}}}: {message}");
            File.AppendAllText(DumpLocation, $"({Name})[{type}] {{{DateTime.Now.ToString(DateFormat)}}}: {message}\n");
            Console.ResetColor();
        }

        public void Info(string message)
        {
            Log(message, "Info", ConsoleColor.Gray);
        }
        public void Warn(string message)
        {
            Log(message, "Warn", ConsoleColor.Yellow);
        }
        public void Error(string message)
        {
            Log(message, "Error", ConsoleColor.Red);
        }

        public void DisplayException(Exception exception)
        {
            Error("An unexpected error occured!");
            Error("--------- Crash report: ---------");
            Error("If you don't know what is that, ask in forums or contact someone, who");
            Error("knows tehnology. TopchetoEU will be thankful, if you send him this report");
            Error($"    {exception.GetType()}: {exception.Message}");
            foreach (var call in exception.StackTrace.Split('\n'))
            {
                Error("    " + call);
            }
            Error("Sorry for the inconvenience!");
            Error("---------------------------------");
            Info("For a full log, see the log dump file " + DumpLocation);
        }

        public Logger(string name)
        {
            Name = name;
            DumpLocation = Environment.CurrentDirectory + "\\Dump log " + DateTime.Now.ToString(DateFormat.Replace('/', '.').Replace(":", "\\\'")) + ".txt";
        }
    }
}