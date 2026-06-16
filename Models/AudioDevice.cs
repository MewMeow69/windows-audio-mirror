namespace WindowsAudioMirror.Models;

public class AudioDevice
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public string DeviceHint
    {
        get
        {
            if (Id.Length < 8) return Id;
            return Id.Length > 36 ? $"{Id[..8]}...{Id[^8..]}" : Id;
        }
    }

    public override string ToString() => Name;
}
