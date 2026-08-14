using HybridFramework.Dotnet.Core;
using OpenQA.Selenium;

namespace HybridFramework.Dotnet.Pages.Mobile;

public abstract class BaseScreen
{
    protected readonly IWebDriver Driver;
    protected virtual TimeSpan DefaultTimeout => TimeSpan.FromSeconds(15);

    protected BaseScreen(IWebDriver driver)
    {
        Driver = driver;
    }

    protected IWebElement Find(By locator) => Waits.Visible(Driver, locator, DefaultTimeout);

    protected void Click(By locator) => Waits.Clickable(Driver, locator, DefaultTimeout).Click();

    protected void Type(By locator, string text)
    {
        var element = Find(locator);
        element.Clear();
        element.SendKeys(text);
    }

    protected IReadOnlyList<IWebElement> FindAll(By locator) => Waits.AnyPresent(Driver, locator, DefaultTimeout);
}
