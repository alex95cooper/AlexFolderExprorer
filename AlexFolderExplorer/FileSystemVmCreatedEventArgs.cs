using System;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer;

public class FileSystemVmCreatedEventArgs : EventArgs
{
    public FileSystemVmCreatedEventArgs(FileSystemViewModel model, string pathRoot)
    {
        Model = model;
        PathRoot = pathRoot;
    }
    
    public FileSystemViewModel Model { get; set; }
    public string PathRoot { get; set; }
}