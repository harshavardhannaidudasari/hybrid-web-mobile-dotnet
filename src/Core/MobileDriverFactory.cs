using HybridFramework.Dotnet.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;

namespace HybridFramework.Dotnet.Core;

/// <summary>
/// Playwright drives the web side of this framework directly (see
/// tests/Hooks), so the only drivers this factory needs to build are the
/// Appium ones for native mobile: Android against a local emulator, iOS
/// against BrowserStack App Automate (no local Mac/simulator on this box).
/// </summary>
public static class MobileDriverFactory
{
    public static IWebDriver CreateAndroidDriver()
    {
        var options = new AppiumOptions
        {
            AutomationName = "UiAutomator2",
            PlatformName = "Android",
            DeviceName = Env.Android.DeviceName
        };
        options.AddAdditionalAppiumOption("appPackage", Env.Android.AppPackage);
        options.AddAdditionalAppiumOption("appActivity", Env.Android.AppActivity);
        options.AddAdditionalAppiumOption("noReset", true);
        options.AddAdditionalAppiumOption("newCommandTimeout", Env.Mobile.NewCommandTimeoutSeconds);

        var driver = new AndroidDriver(new Uri(Env.Mobile.AppiumServerUrl), options);
        // appium:appPackage/appActivity alone don't reliably foreground a
        // pre-installed system app like Settings on every UiAutomator2
        // version - activate it explicitly so tests don't start on the
        // home screen.
        driver.ExecuteScript("mobile: startActivity", new Dictionary<string, object>
        {
            ["intent"] = $"{Env.Android.AppPackage}/{Env.Android.AppActivity}"
        });
        return driver;
    }

    /// <summary>
    /// Builds an iOS session against BrowserStack App Automate using modern
    /// W3C capabilities (nested "bstack:options"), targeting BrowserStack's
    /// public "BStackSampleApp" demo app. Credentials/app id come from
    /// Env.Ios, which reads the plain BROWSERSTACK_* env vars BrowserStack's
    /// own docs use, in addition to this project's HYBRID_* override convention.
    /// </summary>
    public static IWebDriver CreateIosDriver()
    {
        var options = new AppiumOptions
        {
            PlatformName = "iOS",
            DeviceName = Env.Ios.DeviceName,
            PlatformVersion = Env.Ios.PlatformVersion,
            App = Env.Ios.AppId
        };
        options.AddAdditionalAppiumOption("bstack:options", new Dictionary<string, object>
        {
            ["userName"] = Env.Ios.BrowserStackUsername,
            ["accessKey"] = Env.Ios.BrowserStackAccessKey,
            ["projectName"] = "Hybrid Web+Mobile Dotnet",
            ["buildName"] = "iOS BrowserStack",
            ["sessionName"] = "iOS BrowserStack sample text roundtrip",
            ["debug"] = true
        });

        return new IOSDriver(new Uri(Env.Ios.HubUrl), options);
    }
}
