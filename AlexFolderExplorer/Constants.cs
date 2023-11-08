namespace AlexFolderExplorer;

public static class Constants
{
    public static class FileSystem
    {
        public const string FolderPattern = "*";
        public const string FilePattern = "*.*";
        public const string NoRights = "No rights";
        public const string Megabytes = "Mb";
        public const string Gigabytes = "Gb";
        public const string FolderXName = "Folder";
        public const string FileXName = "File";
    }
    
    public static class SizeMeasures
    {
        public const int BytesInGigabytesValue = 1_073_741_824;
        public const int BytesInMegabytesValue = 1_048_576;
    }
    
    public static class Notifications
    {
        public const string WorkComplete = "File is complete";
        public const string InProcess = "In process...";
    }
    
    public static class Errors
    {
        public const string FormNotFull = "Not all fields are filled";
        public const string NameReserved = "Name \"{0}\" reserved by Windows";
        public const string InvalidSymbols = "The name must not contain the following characters: \\/:*?\"<>|";
        public const string InvalidFolderPath = "You entered incorrect path to explore!";
        public const string InvalidFilePath = "You entered incorrect path to save file!";
        public const string FileAlreadyExists = "The file with same name already exists by this path";
        public const string FileInFolderToExplore = "I can't create a file in the folder to be researched";
    }
}