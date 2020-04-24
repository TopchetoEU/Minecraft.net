using System;
using System.IO;

namespace Window.GUI
{
    /// <summary>
    /// Writes to console formatted message and dumps tme message into a file
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// The date format the logger is going to use
        /// </summary>
        public string DateFormat { get; set; } = "dd.MM.yyyy, HH:mm:ss";
        /// <summary>
        /// The name of the logger
        /// </summary>
        public string Name { get; set; } = "Default";
        /// <summary>
        /// The dump location of the logger
        /// </summary>
        public string DumpLocation { get; set; }

        private void Log(string message, string type, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"({Name})[{type}] {{{DateTime.Now.ToString(DateFormat)}}}: {message}");
            File.AppendAllText(DumpLocation, $"({Name})[{type}] {{{DateTime.Now.ToString(DateFormat)}}}: {message}\n");
            Console.ResetColor();
        }

        /// <summary>
        /// Inform the user
        /// </summary>
        /// <param name="message">The message that the user will see</param>
        public void Info(string message)
        {
            Log(message, "Info", ConsoleColor.Gray);
        }
        /// <summary>
        /// Warn the user
        /// </summary>
        /// <param name="message">The message that the user will see</param>
        public void Warn(string message)
        {
            Log(message, "Warn", ConsoleColor.Yellow);
        }
        /// <summary>
        /// Inform the user about an error
        /// </summary>
        /// <param name="message">The message that the user will see</param>
        public void Error(string message)
        {
            Log(message, "Error", ConsoleColor.Red);
        }

        /// <summary>
        /// Displays an exception in a nice way
        /// </summary>
        /// <param name="exception">The exception you want to display</param>
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

        /// <summary>
        /// Creates new logger
        /// </summary>
        /// <param name="name">The name of the logger</param>
        public Logger(string name)
        {
            Name = name;
            DumpLocation = Environment.CurrentDirectory + "\\Dump log " + DateTime.Now.ToString(DateFormat.Replace('/', '.').Replace(":", "\\\'")) + ".txt";
        }
    }
}