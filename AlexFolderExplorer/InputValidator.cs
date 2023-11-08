using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer;

public class InputValidator
{
    private const string PatternName = @"^(AUX|CON|NUL|PRN|COM\d|LPT\d|)$";

    private readonly string[] _unacceptableSymbols = {"\\", "/", ":", "*", "?", "\"", "<", ">", "|"};

    public string Error { get; private set; }

    public bool ValidateModel(InputViewModel model)
    {
        Error = string.Empty;
        return CheckIfModelFull(model)
               && CheckIfNameIsValid(model.Name)
               && CheckIfFolderPathIsValid(model.FolderPath)
               && CheckIfSavePathIsValid(model);
    }

    private bool CheckIfModelFull(InputViewModel model)
    {
        if (model.Name != string.Empty
            && model.FolderPath != string.Empty
            && model.SavePath != string.Empty)
        {
            return true;
        }

        Error = Constants.Errors.FormNotFull;
        return false;
    }

    private bool CheckIfNameIsValid(string name)
    {
        Regex regex = new(PatternName, RegexOptions.IgnoreCase);
        if (regex.IsMatch(name) || _unacceptableSymbols.Any(name.Contains))
        {
            Error = regex.IsMatch(name)
                ? string.Format(Constants.Errors.NameReserved, name)
                : Constants.Errors.InvalidSymbols;
            return false;
        }

        return true;
    }

    private bool CheckIfFolderPathIsValid(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Error = Constants.Errors.InvalidFolderPath;
            return false;
        }

        return true;
    }

    private bool CheckIfSavePathIsValid(InputViewModel model)
    {
        string fullPath = Path.Combine(model.SavePath, model.Name + ".xml");
        return CheckIfSavePathExists(model.SavePath) &&
               CheckIfSaveFileNotExists(fullPath) &&
               CheckIfSaveFileNotExplore(model);
    }

    private bool CheckIfSavePathExists(string savePath)
    {
        if (!Directory.Exists(savePath))
        {
            Error = Constants.Errors.InvalidFilePath;
            return false;
        }

        return true;
    }

    private bool CheckIfSaveFileNotExists(string fullPath)
    {
        if (File.Exists(fullPath))
        {
            Error = Constants.Errors.FileAlreadyExists;
            return false;
        }

        return true;
    }

    private bool CheckIfSaveFileNotExplore(InputViewModel model)
    {
        if (model.SavePath.Contains(model.FolderPath))
        {
            Error = Constants.Errors.FileInFolderToExplore;
            return false;
        }

        return true;
    }
}