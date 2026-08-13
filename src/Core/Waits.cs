using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace HybridFramework.Dotnet.Core;

public static class Waits
{
    public static IWebElement Visible(IWebDriver driver, By locator, TimeSpan timeout) =>
        new WebDriverWait(driver, timeout).Until(ExpectedConditions.ElementIsVisible(locator));

    public static IWebElement Clickable(IWebDriver driver, By locator, TimeSpan timeout) =>
        new WebDriverWait(driver, timeout).Until(ExpectedConditions.ElementToBeClickable(locator));

    public static IReadOnlyList<IWebElement> AnyPresent(IWebDriver driver, By locator, TimeSpan timeout)
    {
        var wait = new WebDriverWait(driver, timeout);
        return wait.Until<IReadOnlyList<IWebElement>?>(d =>
        {
            var elements = d.FindElements(locator);
            return elements.Count > 0 ? elements : null;
        })!;
    }
}
