using System.Collections.ObjectModel;

namespace AudioPlayerTest
{
    public partial class LogFilesPage : ContentPage
    {
        public ObservableCollection<string> LogFiles { get; set; } = new ObservableCollection<string>();
        private readonly string _logsFolder = Path.Combine(FileSystem.AppDataDirectory, "Logs");

        public LogFilesPage()
        {
            InitializeComponent();
            BindingContext = this;

            LogFilesList.ItemsSource = LogFiles;

            if (Directory.Exists(_logsFolder))
            {
                var files = Directory.GetFiles(_logsFolder, "*.*");

                // Sort file data by created date descending
                if (files.Length > 1)
                    files = files.OrderByDescending(f => new FileInfo(f).CreationTime).ToArray();

                foreach (var filePath in files)
                {
                    LogFiles.Add(Path.GetFileName(filePath));
                }
            }
        }

        public void OnLogFileSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var logFileName = e.SelectedItem.ToString();
                var logPath = Path.Combine(_logsFolder, logFileName);
                Navigation.PushAsync(new LogPage(logPath));
                ((ListView)sender).SelectedItem = null;
            }
        }

        public async void OnClearLogsClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirm", "Are you sure you want to delete all log files?", "Yes", "No");
            if (answer)
            {
                // Clear the ObservableCollection
                LogFiles.Clear();

                // Delete all log files
                if (Directory.Exists(_logsFolder))
                {
                    var logFiles = Directory.GetFiles(_logsFolder);
                    foreach (var logFile in logFiles)
                    {
                        File.Delete(logFile);
                    }
                }
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}
