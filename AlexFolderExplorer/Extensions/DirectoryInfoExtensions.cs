using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using AlexFolderExplorer.Helpers;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer.Extensions;

public static class DirectoryInfoExtensions
{
    public static FileSystemViewModel ToViewModel(this DirectoryInfo directory)
    {
        DirectorySecurity ds = directory.GetAccessControl();
        return new FileSystemViewModel
        {
            Name = directory.Name,
            Size = FileInfoProvider.GetFolderSize(directory),
            CreationDate = directory.CreationTime,
            ModifiedDate = directory.LastWriteTime,
            LastAccessDate = directory.LastAccessTime,
            Attributes = directory.Attributes.ToString(),
            Owner = ds.GetOwner(typeof(NTAccount))!.ToString(),
            Rights = FileInfoProvider.GetRights(ds),
            Type = FileSystemElementType.Folder
        };
    }
}