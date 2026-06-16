# Windows Audio Mirror

A simple Windows GUI for **[audio-duplicator](https://github.com/Kl1movM/audio-duplicator)** by **Kl1movM** — mirror audio from your default playback device to a second output device.

## Credits

All audio duplication is handled by **[audio-duplicator](https://github.com/Kl1movM/audio-duplicator)** by Kl1movM. This project is a GUI wrapper that makes it easier to use.

## Features

- Dark-themed WPF interface
- Auto-detects active playback devices
- System tray support (minimizes to tray)
- One-click start/stop mirroring

## Prerequisites

- **Windows 10/11**
- **[.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)** (x64)
- **[audio-duplicator.exe](https://github.com/Kl1movM/audio-duplicator)** — place it in the same folder as `AudioControlHub.exe`

## Quick Start

1. Download the [latest release](https://github.com/MewMeow69/windows-audio-mirror/releases/latest)
2. Extract the zip
3. Download `audio_duplicator.exe` from [audio-duplicator releases](https://github.com/Kl1movM/audio-duplicator/releases) and place it in the extracted folder
4. Run `AudioControlHub.exe`

## Build from Source

```sh
git clone https://github.com/MewMeow69/windows-audio-mirror.git
cd windows-audio-mirror
dotnet build -c Release
```

Then place `audio_duplicator.exe` in `bin\Release\net8.0-windows\` and run.

## License

MIT — see [LICENSE](./LICENSE)
