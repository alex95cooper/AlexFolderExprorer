namespace AlexFolderExplorer;

public interface IFileSystemModelCreator
{
    public delegate void FileSystemVmCreatedHandler(object sender, FileSystemVmCreatedEventArgs e);
        
    public event FileSystemVmCreatedHandler FileSystemVmCreated;
}