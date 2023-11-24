using System;

namespace AlexFolderExplorer.ViewModels;

public class FileSystemViewModel
{
    public string Name { get; set; }
    public string Size { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public DateTime LastAccessDate { get; set; }
    public string Attributes { get; set; }
    public string Owner { get; set; }
    public string Rights { get; set; }
    public FileSystemElementType Type { get; set; }
}