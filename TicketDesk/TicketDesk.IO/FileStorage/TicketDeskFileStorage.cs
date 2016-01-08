// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureBlobFileSystem;

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
                    var accout = AzureConnectionHelper.CloudStorageAccount;
                    if (accout != null)
                    {
                        _currentProvider = new AzureBlobStorageProvider(accout);
                    }
                    else
                    {
                        var dir = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "filestore");
                        _currentProvider = new FileSystemStorageProvider(dir);
                    }
                }
                return _currentProvider;
            }
        }

        public static bool MoveFile(string fileName, string oldContainerId, string newContainerId, bool wasPending, bool isPending)
        {
            var oldPath = GetFilePath(fileName, oldContainerId, wasPending);
            var oldFolder = GetFileFolderPath(oldContainerId, wasPending);
            var newPath = GetFilePath(fileName, newContainerId, isPending);
            if (Current.FileExists(oldPath))
            {
                if (!isPending)
                {
                    Current.TryCreateFolder(GetFileFolderPath(newContainerId, false));//don't care about return value
                }
                //hadle case where filename already exists ; add (x) to filename
                var count = 1;
                var fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
                var extension = Path.GetExtension(fileName);
                while (Current.FileExists(newPath))
                {
                    var tempFileName = string.Format("{0}({1}){2}", fileNameOnly, count++, extension);
                    newPath = GetFilePath(tempFileName,newContainerId, isPending);
                }
                
                Current.RenameFile(oldPath,newPath);
               
                return true;
            }
            return false;
        }

        public static Stream GetFile(string fileName,string containerId, bool isPending)
        {
            var f = Current.GetFile(GetFilePath(fileName, containerId, isPending));
            return f.OpenRead();
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

        public static bool FileExists(string fileName, string containerId, bool isPending)
        {
            return Current.FileExists(GetFilePath(fileName, containerId, isPending));
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
