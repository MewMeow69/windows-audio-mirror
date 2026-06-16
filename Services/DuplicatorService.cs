using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AudioControlHub.Services;

public class DuplicatorService : IDisposable
{
    private Process? _process;
    private readonly string _exePath;
    private volatile bool _stopped;

    public bool IsRunning => _process is { HasExited: false };

    public DuplicatorService()
    {
        var dir = AppContext.BaseDirectory;
        _exePath = Path.Combine(dir, "audio_duplicator.exe");
        if (!File.Exists(_exePath))
            _exePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads", "audio mirror", "audio_duplicator.exe");
    }

    public void Start(string targetDeviceName)
    {
        if (IsRunning) return;

        if (!File.Exists(_exePath))
        {
            var dir = AppContext.BaseDirectory;
            throw new FileNotFoundException(
                $"audio_duplicator.exe not found. Place it in: {dir}");
        }

        _stopped = false;
        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _exePath,
                Arguments = $"\"{targetDeviceName}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
            },
            EnableRaisingEvents = true
        };

        _process.Exited += (_, _) => FireStoppedOnce();

        _process.Start();
    }

    public void Stop()
    {
        if (!IsRunning) return;

        try
        {
            if (!_process!.HasExited)
                _process.Kill(entireProcessTree: true);
        }
        catch { }

        var dead = _process;
        _process = null;

        _ = Task.Run(() =>
        {
            try { dead?.WaitForExit(3000); } catch { }
            dead?.Dispose();
        });

        FireStoppedOnce();
    }

    private void FireStoppedOnce()
    {
        if (_stopped) return;
        _stopped = true;
        OnStopped?.Invoke();
    }

    public event Action? OnStopped;

    public void Dispose()
    {
        Stop();
    }
}
