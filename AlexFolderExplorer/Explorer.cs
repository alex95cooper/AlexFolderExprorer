using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using AlexFolderExplorer.Extensions;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer;

public class Explorer
{
    private const int ThreadsCount = 3;

    private readonly Barrier _barrier;
    private readonly FolderXmlWriter _xmlWriter;
    private readonly TreeViewWriter _treeWriter;
    private readonly InputViewModel _inputVm;
    private readonly Label _progress;

    private Thread _theeViewThread;
    private Thread _xmlFileThread;

    public Explorer(TreeView tree, InputViewModel model, Label progress)
    {
        _inputVm = model;
        _progress = progress;
        _barrier = new Barrier(ThreadsCount);
        _xmlWriter = new FolderXmlWriter(_barrier);
        _treeWriter = new TreeViewWriter(tree, _barrier, _inputVm.FolderPath);
    }

    public void StartExplore()
    {
        FileSystemViewModel folderVm = new DirectoryInfo(_inputVm.FolderPath).ToViewModel();
        XElement rootXElement = _xmlWriter.AddRootXmlFolder(folderVm);
        _treeWriter.AddRootTreeItem(folderVm);
        ExploreCurrentFolder(_inputVm.FolderPath, rootXElement);
        _xmlWriter.SaveFile(_inputVm);
        NotifyCompletion();
    }

    private void ExploreCurrentFolder(string pathRoot, XElement rootXElement)
    {
        DirectoryInfo exploreFolder = new(pathRoot);
        EnumerationOptions options = new EnumerationOptions();
        var subFolders = exploreFolder.GetDirectories(Constants.FileSystem.FolderPattern, options);
        var subFoldersVm = subFolders.ToViewModel();
        var filesVm = exploreFolder.GetFiles(Constants.FileSystem.FilePattern, options).ToViewModel();
        LaunchWriteThreads(subFoldersVm, filesVm, pathRoot, rootXElement);
        ExploreSubFolders(subFolders, rootXElement);
    }

    private void NotifyCompletion()
    {
        Application.Current.Dispatcher.Invoke(() => { _progress.Content = Constants.Notifications.WorkComplete; });
    }

    private void LaunchWriteThreads(List<FileSystemViewModel> subFoldersVm,
        List<FileSystemViewModel> filesVm, string pathRoot, XElement rootXElement)
    {
        _theeViewThread = new Thread(() => _treeWriter.AddToTreeView(subFoldersVm, filesVm, pathRoot));
        _xmlFileThread = new Thread(() => _xmlWriter.WriteToXml(subFoldersVm, filesVm, rootXElement));
        _theeViewThread.Start();
        _xmlFileThread.Start();
        _barrier.SignalAndWait();
    }

    private void ExploreSubFolders(DirectoryInfo[] subFolders, XElement rootXElement)
    {
        foreach (DirectoryInfo subFolder in subFolders)
        {
            XElement subFolderXElement = rootXElement.Descendants(Constants.FileSystem.FolderXName)
                .FirstOrDefault(el => el.Attribute("name")!.Value == subFolder.Name);
            ExploreCurrentFolder(subFolder.FullName, subFolderXElement);
        }
    }
}