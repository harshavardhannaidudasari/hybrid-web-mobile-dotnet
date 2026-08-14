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
/// iOS runs against BrowserStack App Automate (a cloud real-device farm)
/// rather than a local simulator, since this machine can't run one. Values
/// are overridable via both this project's HYBRID_* convention and the
/// plain BROWSERSTACK_* env vars BrowserStack's own docs use directly.
/// </summary>
public sealed class IosConfig
{
    public string DeviceName { get; set; } = "iPhone 14";
    public string PlatformVersion { get; set; } = "16";
    public string HubUrl { get; set; } = "https://hub-cloud.browserstack.com/wd/hub";
    public string AppId { get; set; } = "";
    public string BrowserStackUsername { get; set; } = "";
    public string BrowserStackAccessKey { get; set; } = "";
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
    public static readonly IosConfig Ios;

    static Env()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Config", "config.json");
        var json = File.ReadAllText(path);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Web = JsonSerializer.Deserialize<WebConfig>(root.GetProperty("Web").GetRawText())!;
        Mobile = JsonSerializer.Deserialize<MobileConfig>(root.GetProperty("Mobile").GetRawText())!;
        Android = JsonSerializer.Deserialize<AndroidConfig>(root.GetProperty("Android").GetRawText())!;
        Ios = JsonSerializer.Deserialize<IosConfig>(root.GetProperty("Ios").GetRawText())!;

        ApplyOverrides();
    }

    private static void ApplyOverrides()
    {
        Web.BaseUrl = Environment.GetEnvironmentVariable("HYBRID_WEB_BASEURL") ?? Web.BaseUrl;
        Web.Headless = ParseBool(Environment.GetEnvironmentVariable("HYBRID_WEB_HEADLESS"), Web.Headless);
        Mobile.AppiumServerUrl = Environment.GetEnvironmentVariable("HYBRID_MOBILE_APPIUMSERVERURL") ?? Mobile.AppiumServerUrl;
        Android.DeviceName = Environment.GetEnvironmentVariable("HYBRID_ANDROID_DEVICENAME") ?? Android.DeviceName;

        Ios.DeviceName = Environment.GetEnvironmentVariable("HYBRID_IOS_DEVICENAME") ?? Ios.DeviceName;
        Ios.PlatformVersion = Environment.GetEnvironmentVariable("HYBRID_IOS_PLATFORMVERSION") ?? Ios.PlatformVersion;
        // Plain BROWSERSTACK_* names are what BrowserStack's own docs use, so
        // support them directly (checked first) in addition to HYBRID_IOS_*.
        Ios.HubUrl = Environment.GetEnvironmentVariable("BROWSERSTACK_HUB_URL")
            ?? Environment.GetEnvironmentVariable("HYBRID_IOS_HUBURL")
            ?? Ios.HubUrl;
        Ios.AppId = Environment.GetEnvironmentVariable("BROWSERSTACK_APP_ID") ?? Ios.AppId;
        Ios.BrowserStackUsername = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME") ?? Ios.BrowserStackUsername;
        Ios.BrowserStackAccessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY") ?? Ios.BrowserStackAccessKey;
    }

    private static bool ParseBool(string? raw, bool fallback) =>
        raw is null ? fallback : raw.Trim().ToLowerInvariant() is "1" or "true" or "yes" or "on";
}
