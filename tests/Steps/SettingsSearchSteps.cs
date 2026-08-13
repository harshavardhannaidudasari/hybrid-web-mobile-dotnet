using HybridFramework.Dotnet.Pages.Mobile;
using HybridFramework.Dotnet.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;

namespace HybridFramework.Dotnet.Tests.Steps;

[Binding]
public class SettingsSearchSteps
{
    private readonly MobileContext _mobileContext;
    private SettingsScreen _settings = null!;

    public SettingsSearchSteps(MobileContext mobileContext)
    {
        _mobileContext = mobileContext;
    }

    [Given(@"I open the Settings search screen")]
    public void GivenIOpenTheSettingsSearchScreen()
    {
        _settings = new SettingsScreen(_mobileContext.Driver!);
        _settings.OpenSearch();
    }

    [When(@"I search settings for ""(.*)""")]
    public void WhenISearchSettingsFor(string query)
    {
        _settings.SearchFor(query);
    }

    [Then(@"I should see at least one result")]
    public void ThenIShouldSeeAtLeastOneResult()
    {
        Assert.IsTrue(_settings.GetResults().Count > 0);
    }
}
