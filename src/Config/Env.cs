using System.Text.Json;

namespace HybridFramework.Dotnet.Config;

public sealed class WebConfig
{
    public string BaseUrl { get; set; } = "";
    public bool Headless { get; set; }
}

public sealed class MobileConfig
{
    public string AppiumServerUrl { get; set; } = "";
    public int NewCommandTimeoutSeconds { get; set; } = 120;
}

public sealed class AndroidConfig
{
    public string DeviceName { get; set; } = "";
    public string AppPackage { get; set; } = "";
    public string AppActivity { get; set; } = "";
}

/// <summary>
/// Loads Config/config.json once and applies HYBRID_* environment variable
/// overrides on top, so CI can switch platform/browser without editing the file.
/// </summary>
public static class Env
{
    public static readonly WebConfig Web;
    public static readonly MobileConfig Mobile;
    public static readonly AndroidConfig Android;

    static Env()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Config", "config.json");
        var json = File.ReadAllText(path);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Web = JsonSerializer.Deserialize<WebConfig>(root.GetProperty("Web").GetRawText())!;
        Mobile = JsonSerializer.Deserialize<MobileConfig>(root.GetProperty("Mobile").GetRawText())!;
        Android = JsonSerializer.Deserialize<AndroidConfig>(root.GetProperty("Android").GetRawText())!;

        ApplyOverrides();
    }

    private static void ApplyOverrides()
    {
        Web.BaseUrl = Environment.GetEnvironmentVariable("HYBRID_WEB_BASEURL") ?? Web.BaseUrl;
        Web.Headless = ParseBool(Environment.GetEnvironmentVariable("HYBRID_WEB_HEADLESS"), Web.Headless);
        Mobile.AppiumServerUrl = Environment.GetEnvironmentVariable("HYBRID_MOBILE_APPIUMSERVERURL") ?? Mobile.AppiumServerUrl;
        Android.DeviceName = Environment.GetEnvironmentVariable("HYBRID_ANDROID_DEVICENAME") ?? Android.DeviceName;
    }

    private static bool ParseBool(string? raw, bool fallback) =>
        raw is null ? fallback : raw.Trim().ToLowerInvariant() is "1" or "true" or "yes" or "on";
}
