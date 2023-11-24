using System;
using System.IO;
using System.Threading;
using AlexFolderExplorer.Extensions;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer;

public class Explorer : IFileSystemModelCreator
{
    private ManualResetEvent[] _manualResetEvents;
    private readonly InputViewModel _inputVm;

    public Explorer(InputViewModel model,
        ManualResetEvent[] manualResetEvents)
    {
        _inputVm = model;
        _manualResetEvents = manualResetEvents;
    }
    
    public delegate void ExploreTreadFinishedHandler(object sender, EventArgs e);
        
    public event IFileSystemModelCreator.FileSystemVmCreatedHandler FileSystemVmCreated;
    public event ExploreTreadFinishedHandler ExploreTreadFinished;

    public void StartExplore()
    {
        FileSystemViewModel folderVm = new DirectoryInfo(_inputVm.FolderPath).ToViewModel();
        OnFileSystemVmCreated(folderVm, _inputVm.FolderPath);
        ExploreCurrentFolder(_inputVm.FolderPath);
        ExploreTreadFinished?.Invoke(this, null);
    }

    private void ExploreCurrentFolder(string pathRoot)
    {
        DirectoryInfo exploreFolder = new(pathRoot);
        EnumerationOptions options = new EnumerationOptions();
        var subFolders = exploreFolder.GetDirectories(Constants.FileSystem.FolderPattern, options);
        var files = exploreFolder.GetFiles(Constants.FileSystem.FilePattern, options);
        CreateModels(subFolders, files);
        ExploreSubFolders(subFolders);
    }

    private void CreateModels(DirectoryInfo[] subFolders, FileInfo[] files)
    {
        foreach (var folder in subFolders)
        {
            
            FileSystemViewModel model = folder.ToViewModel();
            WaitHandle.WaitAll(_manualResetEvents);
            OnFileSystemVmCreated(model, folder.FullName);
        }

        foreach (var file in files)
        {
            FileSystemViewModel model = file.ToViewModel();
            WaitHandle.WaitAll(_manualResetEvents);
            OnFileSystemVmCreated(model, file.FullName);
        }
    }

    private void ExploreSubFolders(DirectoryInfo[] subFolders)
    {
        foreach (var subFolder in subFolders)
        {
            ExploreCurrentFolder(subFolder.FullName);
        }
    }

    private void OnFileSystemVmCreated(FileSystemViewModel model, string pathRoot)
    {
        FileSystemVmCreated?.Invoke(this, new FileSystemVmCreatedEventArgs(model, pathRoot));
    }
}