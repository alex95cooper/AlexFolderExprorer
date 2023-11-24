using System.IO;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using AlexFolderExplorer.Extensions;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer
{
    public partial class MainWindow
    {
        private static readonly SynchronizationContext MainThreadContext = SynchronizationContext.Current;
        
        private Explorer _explorer;
        private FolderXmlWriter _xmlWriter;
        private TreeViewWriter _treeWriter;
        private XDocument _document;
        private InputViewModel _inputModel;

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
                _document = new XDocument();
                ManualResetEvent[] manualResetEvents = CreateMreArray();
                _inputModel = inputBox.ToViewModel();
                InitAssistants(manualResetEvents);
                SubscribeToExploreTreadFinish();
                LaunchBackgroundThreads();
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

        private void InitAssistants(ManualResetEvent[] manualResetEvents)
        {
            _explorer = new Explorer(_inputModel, manualResetEvents);
            _xmlWriter = new FolderXmlWriter(_document, _inputModel.FolderPath, manualResetEvents[0]);
            _treeWriter = new TreeViewWriter(TreeViewBar, _inputModel.FolderPath, manualResetEvents[1]);
        }

        private void SubscribeToExploreTreadFinish()
        {
            _explorer.ExploreTreadFinished += (_, _) =>
            {
                MainThreadContext.Post(_ => HandleExploreTreadFinished(), null);
            };
        }

        private void LaunchBackgroundThreads()
        {
            LaunchExploreThread();
            LaunchXmlWriteThread();
            LaunchTreeViewWriteThread();
        }

        private void LaunchExploreThread()
        {
            Thread exploreThread = new Thread(_explorer.StartExplore);
            exploreThread.IsBackground = true;
            exploreThread.Start();
        }
        
        private void LaunchXmlWriteThread()
        {
            Thread xmlFileThread = new Thread(() =>
            {
                _explorer.FileSystemVmCreated += _xmlWriter.ModelHandler;
            });
            
            xmlFileThread.IsBackground = true;
            xmlFileThread.Start();
        }
        
        private void LaunchTreeViewWriteThread()
        {
            Thread theeViewThread = new Thread(() =>
            {
                _explorer.FileSystemVmCreated += _treeWriter.ModelHandler;
            });
            
            theeViewThread.IsBackground = true;
            theeViewThread.Start();
        }

        private ManualResetEvent[] CreateMreArray()
        {
            return new[]
            {
                new ManualResetEvent(false),
                new ManualResetEvent(false)
            };
        }

        private void HandleExploreTreadFinished()
        {
            _document.Save(Path.Combine(_inputModel.SavePath, _inputModel.Name + ".xml"));
            ProcessInfoLabel.Content = Constants.Notifications.WorkComplete;
        }
    }
}