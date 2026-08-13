using Microsoft.Playwright;

namespace HybridFramework.Dotnet.Pages.Web;

public class InventoryPage : BasePage
{
    private ILocator PageTitle => Page.Locator(".title");
    private ILocator InventoryItems => Page.Locator(".inventory_item");

    public InventoryPage(IPage page) : base(page)
    {
    }

    public async Task<string> GetPageTitleAsync() => await PageTitle.TextContentAsync() ?? "";

    public async Task<int> GetItemCountAsync() => await InventoryItems.CountAsync();
}
