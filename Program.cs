// INFO:
// https://help.autodesk.com/view/VAULT/2023/ENU/?guid=GUID-F21E7DD6-39E9-473C-84BB-3446BCAFCCC0
// https://blogs.autodesk.com/vault/2024/03/autodesk-vault-sdk-getting-started-1-installing-the-sdk/
// DOCS LOCATION:
// C:\Program Files\Autodesk\Autodesk Vault 2022 SDK\docs
// DLL LOCATION:
// C:\Program Files\Autodesk\Autodesk Vault 2022 SDK\bin\x64

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;

using Library = Autodesk.DataManagement.Client.Framework.Vault.Library;
using Folder = Autodesk.Connectivity.WebServices.Folder;
using File = Autodesk.Connectivity.WebServices.File;
using VaultConnection = Autodesk.DataManagement.Client.Framework.Vault.Currency.Connections.Connection;

namespace VaultPojectUploader
{
    public class Program
    {
        public static void ErrorDisplay(Exception ex, string message)
        {
            Console.WriteLine($"An error occurred: {ex.Message}\nStack Trace: {ex.StackTrace}\n{message} Press any key to continue.");
        }

        // Move UploadStatus to VaultFileCheckin (makes more sense there)

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Function takes exactly one passed argument for directory path. Function cancelled.");
            }
            else
            {
                #region ConnectToVault
                // Log in with creds (Vault Authentication):
                //ServerIdentities serverID = new ServerIdentities();
                //serverID.DataServer = "US5DEVAVLT1";  //// replace value by command line args[i]
                //serverID.FileServer = "US5DEVAVLT1";
                //string vaultName = "ATC";
                //string userName = "login name";
                //string userPassword = "password";
                //LicensingAgent licenseAgent = LicensingAgent.Client;
                //UserPasswordCredentials credentials = null;
                //WebServiceManager vault = null;

                // Log in without creds (AutoDesk ID):
                ServerIdentities serverId = new ServerIdentities();
                serverId.DataServer = "US5DEVAVLT1";
                serverId.FileServer = "US5DEVAVLT1";
                string vaultName = "ATC";

                IAutodeskAccount autodeskAccount = AutodeskAccount.Login(hwnd: IntPtr.Zero);
                String token = autodeskAccount.GetAccessToken();
                Autodesk.Connectivity.WebServicesTools.AutodeskAuthCredentials autodeskAuthCredentials = new AutodeskAuthCredentials(serverId, vaultName, autodeskAccount, token);

                WebServiceManager vault = null;
                FolderGetterVault serveyFolder = new FolderGetterVault();
                FolderGetterExplorer explorerFiles = new FolderGetterExplorer();
                VaultFileCheckin vaultFileCheckin = new VaultFileCheckin();
                LogFile logFile = new LogFile();

                try
                {
                    //(Vault Authentication):
                    //credentials = new UserPasswordCredentials(serverID, vaultName, userName, userPassword, licenseAgent);
                    //vault = new WebServiceManager(credentials);

                    //(AutoDesk ID):
                    vault = new WebServiceManager(autodeskAuthCredentials);

                    try
                    {
                        //query data, create files, folders, items... etc. here
                        Console.WriteLine("Login Success!");

                        //get "vault folder" and "explorer folder"
                        Folder vaultFolder = serveyFolder.GetFolder(vault);
                        string[] files = explorerFiles.GetExplorerFiles(args[0]);

                        if (vaultFolder == null)
                        {
                            Console.WriteLine("Vault folder could not be gathered.");
                        }
                        else if (files == null | files.Length == 0)
                        {
                            Console.WriteLine("No files were extracted.");
                        }
                        else
                        {
                            foreach (string file in files)
                            {
                                // upload files to Vault from "user folder"
                                Console.WriteLine($"{file}");
                                vaultFileCheckin.CheckinFile(vault, file, vaultFolder);
                            }
                            // Log success and failure
                            logFile.CreateLogFile();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorDisplay(ex, "Automation failed.");
                    }
                    //never forget to release the license, especially if pulled from Server
                    vault.Dispose();
                }
                catch (Exception ex)
                {
                    ErrorDisplay(ex, "Vault connection failed.");
                }
            }
            #endregion connect to Vault
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
