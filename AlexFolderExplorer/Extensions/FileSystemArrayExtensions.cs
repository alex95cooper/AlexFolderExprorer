using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer.Extensions;

public static class FileSystemArrayExtensions
{
    public static List<FileSystemViewModel> ToViewModel(this DirectoryInfo[] directories)
    {
        return directories.Select(directory => directory.ToViewModel()).ToList();
    }
    
    public static List<FileSystemViewModel> ToViewModel(this FileInfo[] files)
    {
        return files.Select(file => file.ToViewModel()).ToList();
    }
}

