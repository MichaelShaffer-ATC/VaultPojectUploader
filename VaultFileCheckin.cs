using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ACW = Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using Autodesk.DataManagement.Client.Framework.Vault.Services;

using Library = Autodesk.DataManagement.Client.Framework.Vault.Library;
using Folder = Autodesk.Connectivity.WebServices.Folder;
using File = Autodesk.Connectivity.WebServices.File;
using VaultConnection = Autodesk.DataManagement.Client.Framework.Vault.Currency.Connections.Connection;

using VDF = Autodesk.DataManagement.Client.Framework;
using Autodesk.Connectivity.WebServices;

namespace VaultPojectUploader
{
    public class VaultFileCheckin
    {
        public static class UploadStatus
        {
            public static List<string> SuccessUploads { get; } = new List<string>();
            public static List<string> FailedUploads { get; } = new List<string>();
        }

        public void CheckinFile(WebServiceManager vault, string filePath, Folder vaultFolder)
        {
            Byte[] fileContent = System.IO.File.ReadAllBytes(filePath);
            ByteArray uploadTicket = UploadFileResource(vault, filePath, fileContent);
            string fileName = Path.GetFileName(filePath);

            try
            {
                vault.DocumentService.AddUploadedFile(vaultFolder.Id, fileName, "Uploaded via Box Automation", DateTime.Now, null, null, FileClassification.None, false, uploadTicket);
                Console.WriteLine($"File {filePath} checked in successfully.");
                // Append success
                UploadStatus.SuccessUploads.Add(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking in file {filePath}: {ex.Message}");
                // Append failure
                UploadStatus.FailedUploads.Add(fileName);
            }
        }

        public static ByteArray UploadFileResource(WebServiceManager svcmgr, string filename, byte[] fileContents)
        {
            svcmgr.FilestoreService.FileTransferHeaderValue = new FileTransferHeader();
            svcmgr.FilestoreService.FileTransferHeaderValue.Identity = Guid.NewGuid();
            svcmgr.FilestoreService.FileTransferHeaderValue.Extension = Path.GetExtension(filename);
            svcmgr.FilestoreService.FileTransferHeaderValue.Vault = svcmgr.WebServiceCredentials.VaultName;

            ByteArray uploadTicket = new ByteArray();
            int bytesTotal = (fileContents != null ? fileContents.Length : 0);
            int bytesTransferred = 0;
            do
            {
                int bufferSize = (bytesTotal - bytesTransferred); // % MAX_FILE_TRANSFER_SIZE;
                byte[] buffer = null;
                if (bufferSize == bytesTotal)
                {
                    buffer = fileContents;
                }
                else
                {
                    buffer = new byte[bufferSize];
                    Array.Copy(fileContents, (long)bytesTransferred, buffer, 0, (long)bufferSize);
                }

                svcmgr.FilestoreService.FileTransferHeaderValue.Compression = Compression.None;
                svcmgr.FilestoreService.FileTransferHeaderValue.IsComplete = (bytesTransferred + bufferSize) == bytesTotal ? true : false;
                svcmgr.FilestoreService.FileTransferHeaderValue.UncompressedSize = bufferSize;

                using (var fileContentsStream = new MemoryStream(fileContents))
                    uploadTicket.Bytes = svcmgr.FilestoreService.UploadFilePart(fileContentsStream);
                bytesTransferred += bufferSize;

            } while (bytesTransferred < bytesTotal);

            return uploadTicket;
        }
    }
}
