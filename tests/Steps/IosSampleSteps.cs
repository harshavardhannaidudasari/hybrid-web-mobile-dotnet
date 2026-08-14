using HybridFramework.Dotnet.Pages.Mobile;
using HybridFramework.Dotnet.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;

namespace HybridFramework.Dotnet.Tests.Steps;

[Binding]
public class IosSampleSteps
{
    private readonly MobileContext _mobileContext;
    private IosSampleScreen _screen = null!;

    public IosSampleSteps(MobileContext mobileContext)
    {
        _mobileContext = mobileContext;
    }

    [Given(@"the iOS BrowserStack session is started")]
    public void GivenTheIosBrowserStackSessionIsStarted()
    {
        _screen = new IosSampleScreen(_mobileContext.Driver!);
    }

    [When(@"the user taps ""Text Button""")]
    public void WhenTheUserTapsTextButton()
    {
        _screen.TapTextButton();
    }

    [When(@"the user enters ""(.*)"" into ""Text Input""")]
    public void WhenTheUserEntersIntoTextInput(string text)
    {
        _screen.EnterText(text);
    }

    [Then(@"""Text Output"" should display ""(.*)""")]
    public void ThenTextOutputShouldDisplay(string expected)
    {
        Assert.AreEqual(expected, _screen.GetOutputText());
    }
}
