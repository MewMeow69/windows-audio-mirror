# Windows Audio Mirror

A simple Windows GUI for **[audio-duplicator](https://github.com/Kl1movM/audio-duplicator)** by **Kl1movM** — mirror audio from your default playback device to a second output device.

## Credits

All audio duplication is handled by **[audio-duplicator](https://github.com/Kl1movM/audio-duplicator)** by Kl1movM. This project is a GUI wrapper that makes it easier to use.

## Features

- **Mirror any playback device** — route audio from your default output to a second device
- **Smart device detection** — only shows active, physical playback devices (filters out virtual endpoints)
- **One-click start/stop** — simple Start/Stop buttons, no command-line needed
- **Dark-themed WPF** — modern dark UI with system tray support (minimizes to tray)
- **Auto-configures** — selects the first two devices as source/target automatically
- **No permanent changes** — doesn't modify Windows audio settings, just duplicates

## Prerequisites

- **Windows 10/11**
- **[.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)** (x64)
- **[audio-duplicator.exe](https://github.com/Kl1movM/audio-duplicator)** — place it in the same folder as `WindowsAudioMirror.exe`

## Quick Start

1. Download the [latest release](https://github.com/MewMeow69/windows-audio-mirror/releases/latest)
2. Extract the zip
3. Download `audio_duplicator.exe` from [audio-duplicator releases](https://github.com/Kl1movM/audio-duplicator/releases) and place it in the extracted folder
4. Run `WindowsAudioMirror.exe`

## Build from Source

```sh
git clone https://github.com/MewMeow69/windows-audio-mirror.git
cd windows-audio-mirror
dotnet build -c Release
```

Then place `audio_duplicator.exe` in `bin\Release\net8.0-windows\` and run `WindowsAudioMirror.exe`.

## License

MIT — see [LICENSE](./LICENSE)
