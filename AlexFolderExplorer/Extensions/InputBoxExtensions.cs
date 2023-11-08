using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer.Extensions;

public static class InputBoxExtensions
{
    public static InputViewModel ToViewModel(this InputBox box)
    {
        return new InputViewModel
        (
            box.NameBox.Text,
            box.FolderPathBox.Text,
            box.SavePathBox.Text
        );
    }
}