using System.Collections.ObjectModel;
using Plugin.Maui.Audio;

namespace AudioPlayerTest;

public partial class MainPage : ContentPage
{
    public ObservableCollection<string> LogFiles { get; set; } = new ObservableCollection<string>();

    private readonly IAudioManager _audioManager;
    private readonly IAudioRecorder _audioRecorder;
    private IAudioPlayer? _audioPlayer = null;
    private IAudioSource _audio;

    string _localDataFolder = FileSystem.AppDataDirectory;
    string _audioFileName = "audio_data_00.wav";
    string _audioFilePath;

    public MainPage(IAudioManager audioManager)
	{
		InitializeComponent();

        // TODO: MediaElement works until you uncomment the next 2 lines!!
        //_audioManager = audioManager;
        //_audioRecorder = _audioManager.CreateRecorder();

        _audioFilePath = Path.Combine(_localDataFolder, _audioFileName);
    }

    private async void RecordAudio(object sender, EventArgs e)
    {
        try
        {
            var granted = await CheckPermissions();
            if (!granted) return;

            //await _audioRecorder.StartAsync(_audioFilePath);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void StopAudio(object sender, EventArgs e)
    {
        try
        {
            //_audio = await _audioRecorder.StopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void PlayAudio(object sender, EventArgs e)
    {
        try
        {
            var granted = await CheckReadWritePermission();
            if (!granted) return;

            //_audioPlayer = _audioManager.CreatePlayer(_audio.GetAudioStream());

            //_audioPlayer.PlaybackEnded += PlaybackEnded;
            //_audioPlayer.Play();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void ShowFiles(object sender, EventArgs e)
    {
        try
        {
            LogFilesList.ItemsSource = LogFiles;

            if (Directory.Exists(_localDataFolder))
            {
                var files = Directory.GetFiles(_localDataFolder, "*.*");

                // Sort file data by created date descending
                if (files.Length > 1)
                    files = files.OrderByDescending(f => new FileInfo(f).CreationTime).ToArray();

                foreach (var filePath in files)
                {
                    LogFiles.Add(Path.GetFileName(filePath));
                }
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void ShowLogFiles(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LogFilesPage());
    }

    private void PlaybackEnded(object? sender, EventArgs e)
    {
        DisplayAlert("Notice", "Playback of the audio ended.", "OK");
    }

    private void OnPlaybackFailed(object sender, EventArgs e)
    {
        DisplayAlert("Notice", "Playback of the audio failed.", "OK");
    }

    private async Task<bool> CheckPermissions()
    {
        var microphoneStatus = await Permissions.CheckStatusAsync<Permissions.Microphone>();

        if (microphoneStatus != PermissionStatus.Granted)
        {
            microphoneStatus = await Permissions.RequestAsync<Permissions.Microphone>();
        }

        if (microphoneStatus == PermissionStatus.Denied)
        {
            await DisplayAlert("Notice", "In order to record audio, microphone access is required.", "OK");
            return false;
        }

        return true;
    }

    private async Task<bool> CheckReadWritePermission()
    {
        try
        {
            var readStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (readStatus != PermissionStatus.Granted)
            {
                readStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            if (readStatus == PermissionStatus.Denied)
            {
                await DisplayAlert("Notice", "In order to export the transcript, file read access is required.", "OK");
                return false;
            }

            var writeStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (writeStatus != PermissionStatus.Granted)
            {
                writeStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }

            if (writeStatus == PermissionStatus.Granted)
            {
                return true;
            }
            else
            {
                await DisplayAlert("Notice", "In order to export the transcript, file write access is required.", "OK");
                return false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            return false;
        }
    }

}


