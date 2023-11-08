using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer;

public class TreeViewWriter
{
    private readonly Barrier _barrier;
    private readonly TreeView _tree;
    private readonly int _nestingCount;

    private TreeViewItem _rootItem;


    public TreeViewWriter(TreeView tree, Barrier barrier, string rootPath)
    {
        _tree = tree;
        _barrier = barrier;
        _nestingCount = rootPath.Split(Path.DirectorySeparatorChar).Length;
    }

    public void AddRootTreeItem(FileSystemViewModel folderVm)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _rootItem = new TreeViewItem();
            _rootItem.Header = folderVm.Name;
            _tree.Items.Add(_rootItem);
        });
    }

    public void AddToTreeView(List<FileSystemViewModel> subFolders,
        List<FileSystemViewModel> files, string rootPath)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            TreeViewItem rootTreeItem = GetRootItem(rootPath);
            AddFolders(subFolders, rootTreeItem);
            AddFiles(files, rootTreeItem);
        });

        _barrier.SignalAndWait();
    }

    private static void AddFolders(List<FileSystemViewModel> subFolders, TreeViewItem rootTreeItem)
    {
        foreach (FileSystemViewModel subFolder in subFolders)
        {
            TreeViewItem subFolderItem = new TreeViewItem();
            subFolderItem.Header = subFolder.Name;
            rootTreeItem.Items.Add(subFolderItem);
        }
    }

    private static void AddFiles(List<FileSystemViewModel> files, TreeViewItem rootTreeItem)
    {
        foreach (FileSystemViewModel file in files)
        {
            TreeViewItem fileItem = new TreeViewItem();
            fileItem.Header = file.Name;
            rootTreeItem.Items.Add(fileItem);
        }
    }

    private TreeViewItem GetRootItem(string path)
    {
        TreeViewItem treeItem = _rootItem;
        string[] directories = path.Split(Path.DirectorySeparatorChar);
        for (int i = _nestingCount; i < directories.Length; i++)
        {
            treeItem = treeItem!.Items.Cast<TreeViewItem>()
                .FirstOrDefault(item => (string) item.Header == directories[i]);
        }

        return treeItem;
    }
}