using HybridFramework.Dotnet.Config;
using HybridFramework.Dotnet.Core;
using HybridFramework.Dotnet.Tests.Context;
using Microsoft.Playwright;
using Reqnroll;

namespace HybridFramework.Dotnet.Tests.Hooks;

/// <summary>
/// Scenario tag decides which driver gets stood up: [web] scenarios get a
/// Playwright page, [mobile] scenarios get an Appium session. Both live on
/// their own context object so step classes only pull in the one they need.
/// </summary>
[Binding]
public class Hooks
{
    private readonly WebContext _webContext;
    private readonly MobileContext _mobileContext;

    public Hooks(WebContext webContext, MobileContext mobileContext)
    {
        _webContext = webContext;
        _mobileContext = mobileContext;
    }

    [BeforeScenario("web")]
    public async Task BeforeWebScenario()
    {
        _webContext.Playwright = await Playwright.CreateAsync();
        _webContext.Browser = await _webContext.Playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions { Headless = Env.Web.Headless });
        _webContext.Page = await _webContext.Browser.NewPageAsync();
    }

    [AfterScenario("web")]
    public async Task AfterWebScenario()
    {
        if (_webContext.Browser is not null)
        {
            await _webContext.Browser.CloseAsync();
        }
        _webContext.Playwright?.Dispose();
    }

    [BeforeScenario("mobile")]
    public void BeforeMobileScenario()
    {
        _mobileContext.Driver = MobileDriverFactory.CreateAndroidDriver();
    }

    [AfterScenario("mobile")]
    public void AfterMobileScenario()
    {
        _mobileContext.Driver?.Quit();
    }
}
