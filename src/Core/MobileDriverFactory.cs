using HybridFramework.Dotnet.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace HybridFramework.Dotnet.Core;

/// <summary>
/// Playwright drives the web side of this framework directly (see
/// tests/Hooks), so the only driver this factory needs to build is the
/// Appium one for native mobile.
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
}
