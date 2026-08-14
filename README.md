# Hybrid Web + Mobile Automation Framework (.NET)

A single Reqnroll (Gherkin/BDD) + MSTest suite that drives **both** browser
(Playwright) and native mobile (Appium) scenarios. The `@web` / `@mobile`
tag on each feature decides which driver `Hooks.cs` stands up before the
scenario runs and tears down after — Playwright's `IPage` for web, Appium's
`IWebDriver` for mobile — while every scenario shares the same runner,
reporter, and `dotnet test` entry point.

This intentionally uses a different, more modern stack from the companion
**C#** project in this repo family (which uses Selenium + NUnit + a
classic Page Object Model without BDD). They're kept as two separate
projects on purpose.

## Stack

| Concern       | Tool                                        |
|---------------|-----------------------------------------------|
| Web driver    | Microsoft.Playwright                           |
| Mobile driver | Appium.WebDriver 5 (UiAutomator2)              |
| BDD/runner    | Reqnroll (SpecFlow's successor) + Reqnroll.MSTest |
| Test host     | MSTest                                         |
| Build         | .NET 8 SDK                                     |

## Project layout

```
src/
  Config/          Env.cs, config.json           # config + HYBRID_* env overrides
  Core/            MobileDriverFactory.cs, Waits.cs
  Pages/Web/       BasePage, LoginPage, InventoryPage    (Playwright IPage)
  Pages/Mobile/    BaseScreen, SettingsScreen             (Selenium/Appium IWebDriver)
tests/
  Features/        Login.feature (@web), SettingsSearch.feature (@mobile), IosSample.feature (@ios)
  Steps/           LoginSteps, SettingsSearchSteps, IosSampleSteps
  Hooks/           Hooks.cs           # tag-driven Playwright/Appium setup+teardown
  Context/         WebContext, MobileContext   # per-scenario DI, shared by Hooks+Steps
  reqnroll.json
```

## Prerequisites

- .NET 8 SDK
- For mobile scenarios: Appium server (`npm i -g appium && appium`), an
  Android emulator/device, and `appium driver install uiautomator2`

## Setup

```bash
dotnet restore
dotnet build
pwsh tests/bin/Debug/net8.0/playwright.ps1 install chromium
```

## Running tests

```bash
# Everything
dotnet test

# Web scenarios only, headless
HYBRID_WEB_HEADLESS=true dotnet test --filter "TestCategory=web"

# Mobile scenarios only (requires Appium server running on 127.0.0.1:4723)
dotnet test --filter "TestCategory=mobile"

# iOS scenario only (requires BrowserStack env vars below)
dotnet test --filter "TestCategory=ios"
```

Reqnroll maps each Gherkin `@tag` to an MSTest `TestCategory`, which is how
`--filter` can target just the web, mobile, or iOS scenarios.

## iOS (BrowserStack App Automate)

Local iOS simulation isn't possible on this machine (no Mac), so the `@ios`
scenario (`tests/Features/IosSample.feature`) runs instead against
[BrowserStack App Automate](https://www.browserstack.com/app-automate), a
cloud real-device farm, via a dedicated `[BeforeScenario("ios")]` hook and
`MobileDriverFactory.CreateIosDriver()`. It's kept as its own `@ios` tag
rather than folded into `@mobile` so the local-Android and BrowserStack-iOS
scenarios stay independently selectable.

The target app is BrowserStack's own public demo app, **BStackSampleApp**,
built exactly for this kind of smoke test: tap `Text Button`, type an email
into `Text Input`, and assert `Text Output` echoes it back.

### Prerequisites

- A BrowserStack account with App Automate access (username + access key
  from https://www.browserstack.com/accounts/settings)

### One-time setup: upload the sample app

BrowserStack hosts the `.ipa` publicly; upload it once to your account to get
an app id:

```bash
curl -u "$BROWSERSTACK_USERNAME:$BROWSERSTACK_ACCESS_KEY" -X POST "https://api-cloud.browserstack.com/app-automate/upload" \
  -F "url=https://www.browserstack.com/app-automate/sample-apps/ios/BStackSampleApp.ipa"
```

This returns `{"app_url":"bs://<hash>"}` — that `bs://...` value is your
`BROWSERSTACK_APP_ID`.

### Required environment variables

| Variable                  | Purpose                                             |
|----------------------------|------------------------------------------------------|
| `BROWSERSTACK_USERNAME`    | BrowserStack account username                        |
| `BROWSERSTACK_ACCESS_KEY`  | BrowserStack account access key                       |
| `BROWSERSTACK_APP_ID`      | `bs://...` id returned by the upload step above       |
| `BROWSERSTACK_HUB_URL`     | Optional; defaults to `https://hub-cloud.browserstack.com/wd/hub` |

These are read directly (BrowserStack's own docs use these exact plain
names), in addition to this project's usual `HYBRID_IOS_*` override
convention in `Env.cs`/`config.json` for device name and platform version.

### Running it

```bash
dotnet test --filter "TestCategory=ios"
```

## CI

`.github/workflows/ci.yml` runs the web scenarios headlessly on every
push/PR. Mobile scenarios require a real device/emulator + Appium server, so
they're left for local or device-farm execution
(`dotnet test --filter "TestCategory=mobile"`).
