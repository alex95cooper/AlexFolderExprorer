using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using AlexFolderExplorer.Extensions;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer;

public class FolderXmlWriter
{
    private readonly Barrier _barrier;

    private XDocument _document = new();

    public FolderXmlWriter(Barrier barrier)
    {
        _barrier = barrier;
    }

    public XElement AddRootXmlFolder(FileSystemViewModel model)
    {
        _document = new XDocument();
        XElement root = model.ToXElement(Constants.FileSystem.FolderXName);
        _document.Add(root);
        return root;
    }

    public void WriteToXml(List<FileSystemViewModel> subFolders,
        List<FileSystemViewModel> files, XElement rootFolder)
    {
        WriteFoldersToXml(subFolders, rootFolder);
        WriteFilesToXml(files, rootFolder);
        _barrier.SignalAndWait();
    }

    public void SaveFile(InputViewModel model)
    {
        _document.Save(Path.Combine(model.SavePath, model.Name + ".xml"));
    }

    private static void WriteFoldersToXml(List<FileSystemViewModel> subFolders, XElement rootFolder)
    {
        foreach (FileSystemViewModel subFolder in subFolders)
        {
            rootFolder.Add(subFolder.ToXElement(Constants.FileSystem.FolderXName));
        }
    }

    private static void WriteFilesToXml(List<FileSystemViewModel> files, XElement rootFolder)
    {
        foreach (FileSystemViewModel file in files)
        {
            rootFolder.Add(file.ToXElement(Constants.FileSystem.FileXName));
        }
    }
}