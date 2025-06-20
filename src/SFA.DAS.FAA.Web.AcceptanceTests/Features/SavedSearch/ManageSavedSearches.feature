Feature: Manage Saved Searches

  As an FAA user
  I want to view and managed my saved searches
  So that I can view or delete searches

  @AuthenticatedUser
  @WireMockServer
  @RunOnEnvironment
  Scenario: View my saved searches
    When I navigate to the following url: /saved-searches
    Then the page is successfully returned
    And the page content includes the following: every Monday
    And the page content includes the following: All apprenticeships in Manchester, Greater Manchester

  @AuthenticatedUser
  @WireMockServer
  Scenario: Delete a saved-search
    When I post to the following url: /saved-searches/a0b9bd85-f087-449e-82b2-7e773d5ed4cf/delete
      | Field          | Value |
    Then I am redirected to the Saved Searches page
    Then the page is redirected
    And the page redirect content includes the following: Search alert deleted.