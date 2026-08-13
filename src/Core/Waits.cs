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
}
