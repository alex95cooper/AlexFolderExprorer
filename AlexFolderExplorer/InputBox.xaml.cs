using System.Windows;
using AlexFolderExplorer.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AlexFolderExplorer;

public partial class InputBox
{
    private readonly InputValidator _validator = new();

    public InputBox()
    {
        InitializeComponent();
    }

    private void FolderPathButton_OnClick(object sender, RoutedEventArgs e)
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            FolderPathBox.Text = dialog.FileName ?? string.Empty;
        }
    }

    private void SavePathButton_OnClick(object sender, RoutedEventArgs e)
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            SavePathBox.Text = dialog.FileName ?? string.Empty;
        }
    }

    private void ExploreButton_OnClick(object sender, RoutedEventArgs e)
    {
        InputViewModel model = BuildInputModel();
        if (_validator.ValidateModel(model))
        {
            DialogResult = true;
        }
        else
        {
            MessageBox.Show(_validator.Error);
        }
    }

    private InputViewModel BuildInputModel()
    {
        return new InputViewModel
        (
            NameBox.Text,
            FolderPathBox.Text,
            SavePathBox.Text
        );
    }
}