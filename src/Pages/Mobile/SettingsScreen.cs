using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace HybridFramework.Dotnet.Pages.Mobile;

/// <summary>Android Settings app search screen - no custom APK required to run this sample.</summary>
public class SettingsScreen : BaseScreen
{
    private static readonly By SearchIcon = MobileBy.AndroidUIAutomator(
        "new UiSelector().resourceId(\"com.android.settings:id/search_action_bar\")");
    // Tapping the search bar hands off to the separate Settings Suggestions
    // (search) app, whose edit text lives under its own package id.
    private static readonly By SearchBox = MobileBy.AndroidUIAutomator(
        "new UiSelector().resourceId(\"com.google.android.settings.intelligence:id/open_search_view_edit_text\")");
    private static readonly By ResultTitles = MobileBy.AndroidUIAutomator(
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

    public IReadOnlyList<IWebElement> GetResults() => FindAll(ResultTitles);
}
