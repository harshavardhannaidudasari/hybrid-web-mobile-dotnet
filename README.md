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
  Features/        Login.feature (@web), SettingsSearch.feature (@mobile)
  Steps/           LoginSteps, SettingsSearchSteps
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
```

Reqnroll maps each Gherkin `@tag` to an MSTest `TestCategory`, which is how
`--filter` can target just the web or mobile scenarios.

## CI

`.github/workflows/ci.yml` runs the web scenarios headlessly on every
push/PR. Mobile scenarios require a real device/emulator + Appium server, so
they're left for local or device-farm execution
(`dotnet test --filter "TestCategory=mobile"`).
