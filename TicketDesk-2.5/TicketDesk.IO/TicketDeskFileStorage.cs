using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureBlobFileSystem;
using Microsoft.WindowsAzure.Storage;

namespace TicketDesk.IO
{
    public static class TicketDeskFileStore
    {
        private static IStorageProvider _currentProvider;
        internal static IStorageProvider Current
        {
            get
            {
                if (_currentProvider == null)
                {
                    var connectionString = AzureConnectionHelper.CloudConfigConnString ??
                                           AzureConnectionHelper.ConfigManagerConnString;
                    CloudStorageAccount cloudStorageAccount;
                    if (CloudStorageAccount.TryParse(connectionString, out cloudStorageAccount))
                    {
                        return new AzureBlobStorageProvider(cloudStorageAccount);
                    }

                    var dir = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "filestore");
                    _currentProvider = new FileSystemStorageProvider(dir);
                }
                return _currentProvider;
            }
        }

        public static IEnumerable<TicketDeskFileInfo> ListAttachmentInfo(string containerId, bool isPending)
        {
            var path = GetFileFolderPath(containerId, isPending);
            var files = Current.ListFiles(path);
            if (isPending)
            {
                files = files.Where(f => f.GetName().EndsWith(containerId));
            }
            return files.Select(f => new TicketDeskFileInfo
            {
                Name = TrimIdFromName(f.GetName(),containerId),
                Size = f.GetSize()
            });
        }

        public static async Task<bool> SaveAttachmentAsync(Stream inputStream, string fileName, string containerId, bool isPending)
        {
            var path = GetFilePath(fileName, containerId, isPending);
            using (var writer = new StreamWriter(Current.CreateFile(path).OpenWrite()))
            {
                await inputStream.CopyToAsync(writer.BaseStream);
            }
            return true;
        }

        public static bool DeleteAttachment(string fileName, string containerId, bool isPending)
        {
            var path = GetFilePath(fileName, containerId, isPending);

            if (Current.FileExists(path))
            {
                Current.DeleteFile(path);
            }
            return true;
        }

        private static string GetFilePath(string fileName, string containerId, bool isPending)
        {
            if (isPending)
            {
                fileName = string.Format("{0}.{1}", fileName, containerId);
            }
            var path = GetFileFolderPath(containerId, isPending);
            return Current.Combine(path, fileName);
        }

        private static string TrimIdFromName(string fileName, string containerId)
        {
            return fileName.EndsWith(containerId) ? fileName.Substring(0, fileName.Length - (containerId.Length + 1)) : fileName;
        }

        private static string GetFileFolderPath(string containerId, bool isPending)
        {
            var path = Current.Combine("ticketdesk-attachments", isPending ? "pending" : containerId);
            return path;
        }
    }
}
