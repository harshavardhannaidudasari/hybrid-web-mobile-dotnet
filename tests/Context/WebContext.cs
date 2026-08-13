using Microsoft.Playwright;

namespace HybridFramework.Dotnet.Tests.Context;

/// <summary>
/// Reqnroll instantiates this once per scenario and injects the same
/// instance into every binding class (Hooks, Steps) that asks for it via
/// its constructor - that's how the Playwright page created in Hooks
/// reaches the step definitions.
/// </summary>
public class WebContext
{
    public IPlaywright? Playwright { get; set; }
    public IBrowser? Browser { get; set; }
    public IPage? Page { get; set; }
}
