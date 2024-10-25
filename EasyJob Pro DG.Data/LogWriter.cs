using System;
using System.Diagnostics;
using System.IO;

namespace EasyJob_ProDG.Data
{
    public class LogWriter
    {
        private static bool isLogStarted = false;
        private static string logFileName = ProgramDefaultSettingValues.ProgramDirectory + "\\" + "log.txt";
        public const string datePattern = @"yyyy/MM/dd hh:mm:ss.fff";

        /// <summary>
        /// Writes the message to the log
        /// </summary>
        /// <param name="message"></param>
        public static void Write(string message)
        {
            if (!isLogStarted) return;

#if DEBUG
            Debug.WriteLine("------ > " + message);
#endif

            try
            {
                using (StreamWriter sw = File.AppendText(logFileName))
                    AppendLog(message, sw);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"!!!!! Log Write method thrown an exception - {ex.Message} - while writing message \"{message}\"");
            }
        }

        /// <summary>
        /// Creates log file if not exists and makes an initial entry
        /// </summary>
        public static void StartLog()
        {
            if (isLogStarted) return;

            if (!File.Exists(logFileName))
                File.Create(logFileName).Close();
            else if (new FileInfo(logFileName).Length > 50000)
            {
                //Removes the oldest record
                var text = File.ReadAllText(logFileName);
                var newText = text.Substring(text.IndexOf(">>>>> Log connected", 1));
                File.WriteAllText(logFileName, newText);
            }

            //Opening log record
            using (TextWriter sw = File.AppendText(logFileName))
            {
                sw.WriteLine(">>>>> Log connected");
                sw.WriteLine(logFileName);
                sw.WriteLine(DateTime.Now.ToString(datePattern));
            }

            isLogStarted = true;

#if DEBUG
            Debug.WriteLine("------ > Start Application_Startup");
#endif
        }


        /// <summary>
        /// Method used to set the final lines on the log session
        /// </summary>
        public static void CloseLog()
        {
            using (TextWriter sw = File.AppendText(logFileName))
            {
                sw.WriteLine("<<<<< Log disconnected <<<<<");
                sw.WriteLine(DateTime.Now.ToString(datePattern));
                sw.WriteLine();
            }
        }


        /// <summary>
        /// Adds a new entry line along with the time
        /// </summary>
        /// <param name="message"></param>
        /// <param name="txtWriter"></param>
        private static void AppendLog(string message, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine($"{DateTime.Now.ToString(datePattern)} : {message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"!!!!! Writing message \"{message}\" to Log file caused an exception {ex.Message}");
            }
        }
    }
}
