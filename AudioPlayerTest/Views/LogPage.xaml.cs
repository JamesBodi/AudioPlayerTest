using System.Collections.ObjectModel;

namespace AudioPlayerTest
{
    public partial class LogPage : ContentPage
    {
        string _logPath;
        public ObservableCollection<string> LogEntries { get; set; } = new ObservableCollection<string>();

        public LogPage(string logPath)
        {
            InitializeComponent();
            BindingContext = this;
            _logPath = logPath;

            if (File.Exists(_logPath))
            {
                foreach (var line in File.ReadLines(_logPath))
                {
                    LogEntries.Add(line);
                }
            }
        }

        public void OnClearLogClicked(object sender, EventArgs e)
        {
            // Clear the ObservableCollection, which should be bound to LogEntries in XAML
            LogEntries.Clear();

            // Clear NLog entries
            if (File.Exists(_logPath))
            {
                File.WriteAllText(_logPath, string.Empty);
            }
        }

    }
}
