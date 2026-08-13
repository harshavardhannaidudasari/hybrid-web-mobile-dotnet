@mobile
Feature: Android Settings search

  Scenario: Searching settings returns results
    Given I open the Settings search screen
    When I search settings for "Wi-Fi"
    Then I should see at least one result
