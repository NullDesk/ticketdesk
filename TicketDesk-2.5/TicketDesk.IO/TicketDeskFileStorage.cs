using System;
using System.IO;
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

        public static async Task<bool> SaveAttachmentAsync(Stream inputStream, string fileName, string id, bool isPending)
        {
            var path = GetFilePath(fileName, id, isPending);
            using (var writer = new StreamWriter(Current.CreateFile(path).OpenWrite()))
            {
                await inputStream.CopyToAsync(writer.BaseStream);
            }
            return true;
        }

        public static bool DeleteAttachment(string fileName , string id, bool isPending)
        {
            var path = GetFilePath(fileName, id, isPending);

            if (Current.FileExists(path))
            {
                Current.DeleteFile(path);
            }
            return true;
        }

        private static string GetFilePath(string fileName, string id, bool isPending)
        {
            if (isPending)
            {
                fileName = string.Format("{0}.{1}", fileName, id);
            }
            var path = Current.Combine("ticketdesk-attachments", isPending ? "pending" : id);
            return Current.Combine(path, fileName);
        }
    }
}
