using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using AlexFolderExplorer.Helpers;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer.Extensions;

public static class FileInfoExtensions
{
    public static FileSystemViewModel ToViewModel(this FileInfo file)
    {
        FileSecurity fs = file.GetAccessControl();
        return new FileSystemViewModel
        {
            Name = file.Name,
            Size = FileInfoProvider.GetFileSize(file),
            CreationDate = file.CreationTime,
            ModifiedDate = file.LastWriteTime,
            LastAccessDate = file.LastAccessTime,
            Attributes = file.Attributes.ToString(),
            Owner = fs.GetOwner(typeof(NTAccount))!.ToString(),
            Rights = FileInfoProvider.GetRights(fs),
            Type = FileSystemElementType.File
        };
    }
}