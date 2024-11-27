Feature: Manage Saved Searches

  As an FAA user
  I want to view and managed my saved searches
  So that I can view or delete searches

  @AuthenticatedUser
  @WireMockServer
  Scenario: View my saved searches
    When I navigate to the following url: /saved-searches
    Then a http status code of 200 is returned
    And the page content includes the following: 2 apprenticeship levels in all of England

  @AuthenticatedUser
  @WireMockServer
  Scenario: Delete a saved-search
    When I post to the following url: /saved-searches/a0b9bd85-f087-449e-82b2-7e773d5ed4cf/delete
      | Field          | Value |
    Then I am redirected to the Saved Searches page
    And a http status code of 302 is returned
    And the page redirect content includes the following: Search alert deleted.