using Microsoft.Win32;
using WindowsAudioMirror.Models;

namespace WindowsAudioMirror.Services;

public static class AudioDeviceService
{
    private static readonly string RenderKey =
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\MMDevices\Audio\Render";

    private static readonly string[] ExcludedNames =
        ["VBMatrix In", "SteamLink", "Internal AUX", "Speakers", "Headphones", "Headset", "Line"];

    public static List<AudioDevice> GetRenderDevices()
    {
        var result = new List<AudioDevice>();
        var seenNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        using var key = Registry.LocalMachine.OpenSubKey(RenderKey);
        if (key == null) return result;

        foreach (var subKeyName in key.GetSubKeyNames())
        {
            using var deviceKey = key.OpenSubKey(subKeyName);
            if (deviceKey == null) continue;

            var deviceState = deviceKey.GetValue("DeviceState") as int?;
            if (deviceState == null || (deviceState.Value & 0xF) != 1)
                continue;

            using var propsKey = deviceKey.OpenSubKey("Properties");
            if (propsKey == null) continue;

            var name = propsKey.GetValue(
                "{a45c254e-df1c-4efd-8020-67d146a850e0},2") as string;

            if (string.IsNullOrEmpty(name)) continue;
            if (ExcludedNames.Any(p => name.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                continue;
            if (!seenNames.Add(name)) continue;

            var desc = propsKey.GetValue(
                "{a45c254e-df1c-4efd-8020-67d146a850e0},4") as string ?? "";

            var fmt = ParseFormat(propsKey.GetValue(
                "{f19f064d-086c-4e44-9f25-d2a68e7c3537},0") as byte[]);

            result.Add(new AudioDevice
            {
                Id = subKeyName,
                Name = name,
                DeviceDescription = desc,
                FormatSummary = fmt
            });
        }

        return result.OrderBy(d => d.Name).ToList();
    }

    private static string ParseFormat(byte[]? data)
    {
        if (data == null || data.Length < 18) return "";
        try
        {
            var ch = BitConverter.ToUInt16(data, 2);
            var sr = BitConverter.ToUInt32(data, 4);
            var bps = BitConverter.ToUInt16(data, 14);
            return $"{sr} Hz, {bps} bit, {ch} ch";
        }
        catch { return ""; }
    }

    public static string? FindDeviceId(string nameSubstr)
    {
        using var key = Registry.LocalMachine.OpenSubKey(RenderKey);
        if (key == null) return null;

        foreach (var subKeyName in key.GetSubKeyNames())
        {
            using var deviceKey = key.OpenSubKey(subKeyName);
            if (deviceKey == null) continue;
            using var propsKey = deviceKey.OpenSubKey("Properties");
            if (propsKey == null) continue;

            var name = propsKey.GetValue(
                "{a45c254e-df1c-4efd-8020-67d146a850e0},2") as string;

            if (!string.IsNullOrEmpty(name) &&
                name.Contains(nameSubstr, StringComparison.OrdinalIgnoreCase))
                return subKeyName;
        }
        return null;
    }

    public static string? GetDefaultDeviceName()
    {
        return GetRenderDevices().FirstOrDefault()?.Name;
    }
}
