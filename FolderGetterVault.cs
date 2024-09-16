using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;

using Library = Autodesk.DataManagement.Client.Framework.Vault.Library;
using Folder = Autodesk.Connectivity.WebServices.Folder;
using File = Autodesk.Connectivity.WebServices.File;

namespace VaultPojectUploader
{
    public class FolderGetterVault
    {
        string SURVEY_PATH = @"$/ATC/Domestic/SURVEY";
        public Folder GetFolder(WebServiceManager vault)
        {
            //get directory location and upload each file from directory
            Console.WriteLine("Getting Vault folder!");

            try
            {
                var vaultFolder = new Folder();
                vaultFolder = vault.DocumentService.GetFolderByPath(SURVEY_PATH);
                Console.WriteLine($"Folder ID: {vaultFolder.Id}");
                Console.WriteLine($"Folder Name: {vaultFolder.Name}");

                return vaultFolder;
            }
            catch (Exception ex )
            {
                Program.ErrorDisplay(ex, $"Vault directory {SURVEY_PATH} could not be reached.");
                return null;
            }
        }
    }
}
