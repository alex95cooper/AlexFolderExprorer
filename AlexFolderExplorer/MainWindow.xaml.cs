using System.Threading;
using System.Windows;
using AlexFolderExplorer.Extensions;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer
{
    public partial class MainWindow
    {
        private Explorer _explorer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_OnClick(object sender, RoutedEventArgs e)
        {
            ProcessInfoLabel.Content = string.Empty;
            OpenInputBox();
        }

        private void OpenInputBox()
        {
            InputBox inputBox = SetInputBox();
            if (inputBox.ShowDialog() == true)
            {
                ProcessInfoLabel.Content = Constants.Notifications.InProcess;
                TreeViewBar.Items.Clear();
                InputViewModel model = inputBox.ToViewModel();
                _explorer = new Explorer(TreeViewBar, model, ProcessInfoLabel);
                LaunchExploreThread();
            }
        }

        private InputBox SetInputBox()
        {
            return new InputBox
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
        }

        private void LaunchExploreThread()
        {
            Thread thread = new Thread(_explorer.StartExplore);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}