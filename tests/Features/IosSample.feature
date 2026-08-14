@ios
Feature: iOS BrowserStack sample app text roundtrip

  Scenario: Entering text via BrowserStack's sample app echoes it back
    Given the iOS BrowserStack session is started
    When the user taps "Text Button"
    And the user enters "hello@browserstack.com" into "Text Input"
    Then "Text Output" should display "hello@browserstack.com"
