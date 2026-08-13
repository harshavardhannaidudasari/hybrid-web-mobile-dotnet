using Microsoft.Playwright;

namespace HybridFramework.Dotnet.Pages.Web;

public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page)
    {
        Page = page;
    }

    public Task OpenAsync(string url) => Page.GotoAsync(url);
}
