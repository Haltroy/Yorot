/*

Copyright © 2020 Eren "Haltroy" Kanat

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/Korot/blob/master/LICENSE

*/

using System;
using System.IO;

namespace Korot_Win32
{
    public class Output
    {
        private readonly string LogDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\korot.d\\log\\";

        private static Output _outputSingleton;

        private static Output OutputSingleton
        {
            get
            {
                if (_outputSingleton == null)
                {
                    _outputSingleton = new Output();
                }
                return _outputSingleton;
            }
        }

        public StreamWriter SW { get; set; }

        public Output()
        {
            EnsureLogDirectoryExists();
            InstantiateStreamWriter();
        }

        ~Output()
        {
            if (SW != null)
            {
                try
                {
                    SW.Dispose();
                }
                catch (ObjectDisposedException) { } // object already disposed - ignore exception
            }
        }

        public static void WriteLine(string str, LogLevel level = LogLevel.None)
        {
            Console.WriteLine(str);
            switch (level)
            {
                case LogLevel.Hidden: break;
                case LogLevel.None: str = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:FFFFFFF") + ") " + str;break;
                case LogLevel.Info: str = " [I] (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:FFFFFFF") + ") " + str;break;
                case LogLevel.Warning: str = " [W] (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:FFFFFFF") + ") " + str;break;
                case LogLevel.Error: str = "[E] (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:FFFFFFF") + ") " + str;break;
                case LogLevel.Critical: str = "[C] (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:FFFFFFF") + ") " + str;break;
            }
            OutputSingleton.SW.WriteLine(str);
        }

        public static void Write(string str)
        {
            Console.Write(str);
            OutputSingleton.SW.Write(str);
        }

        private void InstantiateStreamWriter()
        {
            string filePath = Path.Combine(LogDirPath, "korot." + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-FFFFFFF")) + ".txt";
            try
            {
                SW = new StreamWriter(filePath)
                {
                    AutoFlush = true,
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new ApplicationException(string.Format("Access denied. Could not instantiate StreamWriter using path: {0}.", filePath), ex);
            }
        }

        private void EnsureLogDirectoryExists()
        {
            if (!Directory.Exists(LogDirPath))
            {
                try
                {
                    Directory.CreateDirectory(LogDirPath);
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw new ApplicationException(string.Format("Access denied. Could not create log directory using path: {0}.", LogDirPath), ex);
                }
            }
        }
    }
    /// <summary>
    /// The log level.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Hides date and log level marks.
        /// </summary>
        Hidden,
        /// <summary>
        /// No log level.
        /// </summary>
        None,
        /// <summary>
        /// Information
        /// </summary>
        Info,
        /// <summary>
        /// An exception or problem occurred but ignored.
        /// </summary>
        Warning,
        /// <summary>
        /// An exception or problem occurred (not ignored):
        /// </summary>
        Error,
        /// <summary>
        /// Ciritical exception occurred.
        /// </summary>
        Critical
    }
}