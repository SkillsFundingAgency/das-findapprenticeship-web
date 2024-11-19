Feature: Create Saved Searches
  
  As an FAA user
  I want a create a saved search
  So that I can be notified when new matching apprenticeships are available
    
  @WireMockServer
  Scenario: Sign in to create a saved search
    When I navigate to the following url: /apprenticeships?location=manchester
    Then a http status code of 200 is returned
    And the page content includes the following: No results found
    And the page content includes the following: Sign in to create an alert for this search
    
  @AuthenticatedUser
  @WireMockServer
  Scenario: Can save a search
    When I navigate to the following url: /apprenticeships?location=manchester
    Then a http status code of 200 is returned
    And the page content includes the following: No results found
    And the page content includes the following: Create an alert for this search</button>
  
  @AuthenticatedUser
  @WireMockServer
  Scenario: Save a search
    When I navigate to the following url: /apprenticeships?location=manchester
    And I save my search
    Then I am redirected to the Search Results page