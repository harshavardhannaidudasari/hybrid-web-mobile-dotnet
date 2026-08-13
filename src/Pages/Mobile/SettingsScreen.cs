using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace HybridFramework.Dotnet.Pages.Mobile;

/// <summary>Android Settings app search screen - no custom APK required to run this sample.</summary>
public class SettingsScreen : BaseScreen
{
    private static readonly By SearchIcon = AppiumBy.AccessibilityId("Search settings");
    private static readonly By SearchBox = AppiumBy.AndroidUIAutomator(
        "new UiSelector().resourceId(\"android:id/search_src_text\")");
    private static readonly By ResultTitles = AppiumBy.AndroidUIAutomator(
        "new UiSelector().resourceId(\"android:id/title\")");

    public SettingsScreen(IWebDriver driver) : base(driver)
    {
    }

    public SettingsScreen OpenSearch()
    {
        Click(SearchIcon);
        return this;
    }

    public SettingsScreen SearchFor(string query)
    {
        Type(SearchBox, query);
        return this;
    }

    public IReadOnlyList<IWebElement> GetResults() => Driver.FindElements(ResultTitles);
}
