using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AlexFolderExplorer.Helpers;

public static class FileInfoProvider
{
    public static string GetFolderSize(DirectoryInfo crrDir)
    {
        long folderSize = CalculateFolderSize(crrDir);
        int measureRatio = folderSize > Constants.SizeMeasures.BytesInGigabytesValue
            ? Constants.SizeMeasures.BytesInGigabytesValue
            : Constants.SizeMeasures.BytesInMegabytesValue;
        return $"{(double) folderSize / measureRatio:0.0} "
               + GetMeasureUnit(folderSize) + $" ({folderSize:#,#} Bytes)";
    }

    public static string GetFileSize(FileInfo crrFile)
    {
        int measureRatio = crrFile.Length > Constants.SizeMeasures.BytesInGigabytesValue
            ? Constants.SizeMeasures.BytesInGigabytesValue
            : Constants.SizeMeasures.BytesInMegabytesValue;
        return $"{(double) crrFile.Length / measureRatio:0.0} "
               + GetMeasureUnit(crrFile.Length) + $" ({crrFile.Length:#,#} Bytes)";
    }

    public static string GetRights(FileSystemSecurity ds)
    {
        var rightList = ds.GetAccessRules(
            true, true, typeof(NTAccount));
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        foreach (FileSystemAccessRule right in rightList)
        {
            var ntAccount = right.IdentityReference as NTAccount;
            if (ntAccount != null && principal.IsInRole(ntAccount.Value))
            {
                return right.FileSystemRights.ToString();
            }
        }

        return Constants.FileSystem.NoRights;
    }

    private static long CalculateFolderSize(DirectoryInfo crrDir)
    {
        EnumerationOptions options = new EnumerationOptions();
        var files = crrDir.GetFiles(Constants.FileSystem.FilePattern, options);
        long size = files.Sum(file => file.Length);
        var folder = crrDir.GetDirectories(Constants.FileSystem.FolderPattern, options);
        size += folder.Sum(CalculateFolderSize);
        return size;
    }

    private static string GetMeasureUnit(long size)
    {
        return size > Constants.SizeMeasures.BytesInGigabytesValue
            ? Constants.FileSystem.Gigabytes
            : Constants.FileSystem.Megabytes;
    }
}