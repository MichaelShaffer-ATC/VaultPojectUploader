using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

// NEED ACCESS TO AN EXPLORER FOLDER

namespace VaultPojectUploader
{
    public class FolderGetterExplorer
    {
        //"C:\Users\Michael.Shaffer\OneDrive - American Tower\Desktop\TO-DO LIST\Box.com\VAULT THESE"
        public string[] GetExplorerFiles(string folderPath)
        {
            try
            {
                Console.WriteLine($"Getting explorer folder {folderPath}.");
                string[] files = Directory.GetFiles(folderPath);
                return files;
            }
            catch (Exception ex)
            {
                Program.ErrorDisplay(ex, $"Explorer directory {folderPath} could not be found.");
                return null;
            }
        }
    }
}
