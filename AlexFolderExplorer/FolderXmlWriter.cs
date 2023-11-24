using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using AlexFolderExplorer.Extensions;

namespace AlexFolderExplorer;

public class FolderXmlWriter
{
    private readonly ManualResetEvent _manualResetEvent;
    private readonly XDocument _document;
    private readonly string _rootFolderPath;
    private readonly int _nestingCount;

    private XElement _rootElement;

    public FolderXmlWriter(XDocument document, string rootPath, WaitHandle manualResetEvent)
    {
        _document = document;
        _manualResetEvent = (ManualResetEvent) manualResetEvent;
        _rootFolderPath = rootPath;
        _nestingCount = rootPath.Split(Path.DirectorySeparatorChar).Length;
    }
    
    public void ModelHandler(object sender, FileSystemVmCreatedEventArgs e)
    {
        XElement rootElement = GetRootItem(e.PathRoot);
        XElement fileSystemElement = e.Model.ToXElement();
        AddFileSystemItem(rootElement, fileSystemElement, e);
        _manualResetEvent.Set();
    }

    private void AddFileSystemItem(XElement rootElement,
        XElement fileSystemElement, FileSystemVmCreatedEventArgs e)
    {
        if (e.PathRoot == _rootFolderPath)
        {
            _rootElement = fileSystemElement;
            _document.Add(_rootElement);
        }
        else
        {
            rootElement.Add(fileSystemElement);
        }
    }

    private XElement GetRootItem(string path)
    {
        XElement xElement = _rootElement;
        string[] directories = path.Split(Path.DirectorySeparatorChar);
        for (int i = _nestingCount; i < directories.Length - 1; i++)
        {
            xElement = xElement!.Descendants(Constants.FileSystem.FolderXName)
                .FirstOrDefault(el => el.Attribute("name")!.Value == directories[i]);
        }

        return xElement;
    }
}