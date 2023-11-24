using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace AlexFolderExplorer;

public class TreeViewWriter
{
    private readonly ManualResetEvent _manualResetEvent;
    private readonly TreeView _tree;
    private readonly string _rootFolderPath;
    private readonly int _nestingCount;

    private TreeViewItem _rootItem;

    public TreeViewWriter(TreeView tree, string rootPath, ManualResetEvent manualResetEvent)
    {
        _tree = tree;
        _manualResetEvent = manualResetEvent;
        _rootFolderPath = rootPath;
        _nestingCount = rootPath.Split(Path.DirectorySeparatorChar).Length;
    }
    
    public void ModelHandler(object sender, FileSystemVmCreatedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            TreeViewItem rootTreeItem = GetRootItem(e.PathRoot);
            TreeViewItem fileSystemItem = new TreeViewItem();
            fileSystemItem.Header = e.Model.Name;
            AddFileSystemItem(rootTreeItem, fileSystemItem, e);
        });

        _manualResetEvent.Set();
    }

    private void AddFileSystemItem(TreeViewItem rootTreeItem,
        TreeViewItem fileSystemItem, FileSystemVmCreatedEventArgs e)
    {
        if (e.PathRoot == _rootFolderPath)
        {
            _rootItem = fileSystemItem;
            _tree.Items.Add(_rootItem);
        }
        else
        {
            rootTreeItem.Items.Add(fileSystemItem);
        }
    }

    private TreeViewItem GetRootItem(string path)
    {
        TreeViewItem treeItem = _rootItem;
        string[] directories = path.Split(Path.DirectorySeparatorChar);
        for (int i = _nestingCount; i < directories.Length - 1; i++)
        {
            treeItem = treeItem!.Items.Cast<TreeViewItem>()
                .FirstOrDefault(item => (string) item.Header == directories[i]);
        }

        return treeItem;
    }
}