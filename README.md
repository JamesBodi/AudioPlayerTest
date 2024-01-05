The project was created to demonstrate a bug or at least an incompatibility between MediaElement and Plugin.Maui.Audio.

BACKGROUND
I am working on a mobile app that needs to record and playback audio. I decided to use Plugin.Maui.Audio. This all worked great until I got to testing the app on IOS. 
PLayback fails on all IOS devices. PLayback does work on IOS simulators. I couldn't ever find a way to get the Plugin to work on IOS so I switched to MediaElement for playback.
And the cycle repeated itself - everything worked great on Android (of course), it worked great in IOS simulators, but failed everytime on IOS devices. 
I would prefer to get the Plugin working rather than reference 2 libraries.

TESTING
I created a new .Net Maui app from scratch and started adding dependencies. I added MediaElement first and to my surprise it worked - even on IOS devces! 
OK, so playback was working, now on to recording audio. So I added Plugin.Maui.Audio dependency to the project and started to wire it up. Everything (i.e. playback)
continued working - until I referenced a plugin object:

  public MainPage(IAudioManager audioManager)
	{
		InitializeComponent();

        // TODO: MediaElement works until you uncomment the next 2 lines!!
        //_audioManager = audioManager;
        //_audioRecorder = _audioManager.CreateRecorder();
    }

I would have thought that injecting the AudioManager would cause it to fail but its not until I store the parameter in a module level var that it fails. I dont know when exactly  
the AudioManager gets created (e.g. before its injected, only when it is referenced) but MediaPlayer no longers works if the 2 lines above are uncommented. There appears to be a
conflict between these 2 libraries.
