using Microsoft.Playwright;

namespace HybridFramework.Dotnet.Pages.Web;

public class LoginPage : BasePage
{
    private ILocator Username => Page.Locator("#user-name");
    private ILocator Password => Page.Locator("#password");
    private ILocator LoginButton => Page.Locator("#login-button");
    private ILocator ErrorMessage => Page.Locator("[data-test='error']");

    public LoginPage(IPage page) : base(page)
    {
    }

    public async Task SubmitLoginAsync(string user, string password)
    {
        await Username.FillAsync(user);
        await Password.FillAsync(password);
        await LoginButton.ClickAsync();
    }

    public async Task<string> GetErrorMessageAsync() => await ErrorMessage.TextContentAsync() ?? "";
}
