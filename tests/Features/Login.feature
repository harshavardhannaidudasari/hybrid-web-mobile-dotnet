@web
Feature: SauceDemo login

  Scenario: Standard user logs in successfully
    Given I open the SauceDemo login page
    When I log in as "standard_user" with password "secret_sauce"
    Then I should see the "Products" inventory page

  Scenario: Locked out user sees an error
    Given I open the SauceDemo login page
    When I log in as "locked_out_user" with password "secret_sauce"
    Then I should see a "locked out" error message
