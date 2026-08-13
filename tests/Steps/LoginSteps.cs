using HybridFramework.Dotnet.Config;
using HybridFramework.Dotnet.Pages.Web;
using HybridFramework.Dotnet.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;

namespace HybridFramework.Dotnet.Tests.Steps;

[Binding]
public class LoginSteps
{
    private readonly WebContext _webContext;
    private LoginPage _loginPage = null!;

    public LoginSteps(WebContext webContext)
    {
        _webContext = webContext;
    }

    [Given(@"I open the SauceDemo login page")]
    public async Task GivenIOpenTheSauceDemoLoginPage()
    {
        _loginPage = new LoginPage(_webContext.Page!);
        await _loginPage.OpenAsync(Env.Web.BaseUrl);
    }

    [When(@"I log in as ""(.*)"" with password ""(.*)""")]
    public async Task WhenILogInAsWithPassword(string user, string password)
    {
        await _loginPage.SubmitLoginAsync(user, password);
    }

    [Then(@"I should see the ""(.*)"" inventory page")]
    public async Task ThenIShouldSeeTheInventoryPage(string expectedTitle)
    {
        var inventoryPage = new InventoryPage(_webContext.Page!);
        Assert.AreEqual(expectedTitle, await inventoryPage.GetPageTitleAsync());
        Assert.IsTrue(await inventoryPage.GetItemCountAsync() > 0);
    }

    [Then(@"I should see a ""(.*)"" error message")]
    public async Task ThenIShouldSeeAnErrorMessage(string expectedSubstring)
    {
        var message = await _loginPage.GetErrorMessageAsync();
        StringAssert.Contains(message, expectedSubstring);
    }
}
