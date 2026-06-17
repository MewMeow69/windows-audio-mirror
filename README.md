# Windows Audio Mirror

A simple Windows GUI for **[audio-duplicator](https://github.com/Kl1movM/audio-duplicator)** by **Kl1movM** that lets you mirror audio from your default playback device to a second output device.

# Why is this better than Stereo Mix?

Because stereo mix is annoying to use, has more latency and this just bundles everything into a nice little app.

## Credits

All audio duplication is handled by **[audio-duplicator](https://github.com/Kl1movM/audio-duplicator)** by Kl1movM. This project is a GUI wrapper that makes it easier to use.

## Features

- **Mirror any playback device** - route audio from your default output to a second device
- **Smart device detection** - only shows active, physical playback devices (filters out virtual endpoints)
- **One-click start/stop** - simple Start/Stop buttons, no command-line needed
- **Dark-themed WPF** - modern dark UI with system tray support (minimizes to tray)
- **Auto-configures** - selects the first two devices as source/target automatically
- **No permanent changes** - doesn't modify Windows audio settings, just duplicates

## Prerequisites

- **Windows 10/11**
- **[.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)** (x64)

## Quick Start

1. Download the [latest release](https://github.com/MewMeow69/windows-audio-mirror/releases/latest)
2. Extract the zip
3. Run `WindowsAudioMirror.exe`


## Build from Source

```sh
git clone https://github.com/MewMeow69/windows-audio-mirror.git
cd windows-audio-mirror
dotnet build -c Release
```

Then run `WindowsAudioMirror.exe` from the output folder (also needs `audio_duplicator.exe` — see Credits).

## License

MIT — see [LICENSE](./LICENSE)
