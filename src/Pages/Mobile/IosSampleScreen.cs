using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace HybridFramework.Dotnet.Pages.Mobile;

/// <summary>
/// BrowserStack's public "BStackSampleApp" iOS demo app - the officially
/// documented smoke-test target for App Automate, so no custom IPA build is
/// needed beyond the one-time upload documented in the README.
/// </summary>
public class IosSampleScreen : BaseScreen
{
    // Real BrowserStack devices queue/boot slower than a local emulator, so
    // give iOS elements a longer explicit wait than Android's 15s default.
    protected override TimeSpan DefaultTimeout => TimeSpan.FromSeconds(30);

    private static readonly By TextButton = MobileBy.AccessibilityId("Text Button");
    private static readonly By TextInput = MobileBy.AccessibilityId("Text Input");
    private static readonly By TextOutput = MobileBy.AccessibilityId("Text Output");

    public IosSampleScreen(IWebDriver driver) : base(driver)
    {
    }

    public IosSampleScreen TapTextButton()
    {
        Click(TextButton);
        return this;
    }

    public IosSampleScreen EnterText(string text)
    {
        Type(TextInput, text);
        Find(TextInput).SendKeys(Keys.Return);
        return this;
    }

    public string GetOutputText() => Find(TextOutput).Text;
}
