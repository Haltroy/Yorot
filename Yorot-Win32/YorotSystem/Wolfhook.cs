using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HTAlt;

namespace Yorot
{
    /// <summary>
    /// Hooks other instances to itself.
    /// </summary>
    public class Wolfhook
    {
        /// <summary>
        /// Location of Wolves in 
        /// </summary>
        public string WhFolder => YorotGlobal.YorotAppPath + "\\wolfhook\\";

        /// <summary>
        /// Determines the <see cref="Encoding"/> of Wolves.
        /// </summary>
        public Encoding DefaultEncoding { get; set; } = Encoding.Unicode;
        /// <summary>
        /// Determines the timeout between each search.
        /// </summary>
        public int Timeout { get; set; } = 5000;
        /// <summary>
        /// Logs "Working" message when working.
        /// </summary>
        public bool LogWork { get; set; } = false;
        /// <summary>
        /// List of fetched wolves.
        /// </summary>
        public List<string> Wolves { get; set; } = new List<string>();
        public void SendWolf(string message,string id = "")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = HTAlt.Tools.GenerateRandomText(17);
            }
            Output.WriteLine("<WOLFHOOK> Created message=\"" + message + "\" from ID=\"" + id + "\" without error(s).", LogLevel.Info);
            message.WriteToFile(WhFolder + id + ".wh", DefaultEncoding);
        }
        private bool StopTask { get; set; } = false;
        /// <summary>
        /// Starts searching.
        /// </summary>
        public void StartSearch()
        {
            StopTask = false;
            Task.Run(() => SearchForWolves());

        }
        /// <summary>
        /// Stops before starting a new search in next search.
        /// </summary>
        public void StopSearch()
        {
            StopTask = true;
        }
        private async void SearchForWolves()
        {
            if (StopTask)
            {
                return;
            }
            await Task.Run(() =>
            {
                if (LogWork)
                {
                    Output.WriteLine("<WOLFHOOK> Working...", LogLevel.Info);
                }
                string[] whFiles = Directory.GetFiles(WhFolder, "*.wh", SearchOption.TopDirectoryOnly);
                for(int i = 0; i < whFiles.Length;i++)
                {
                    string message = HTAlt.Tools.ReadFile(whFiles[i], DefaultEncoding);
                    string id = Path.GetFileNameWithoutExtension(whFiles[i]);
                    Wolves.Add(message);
                    Output.WriteLine("<WOLFHOOK> Received message=\"" + message + "\" from ID=\"" + id + "\".", LogLevel.Info);
                    File.Delete(whFiles[i]);
                }
                Thread.Sleep(Timeout);
                Task.Run(() => SearchForWolves());
            });
        }
    }
}
