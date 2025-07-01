Feature: Create Saved Searches
  
  As an FAA user
  I want a create a saved search
  So that I can be notified when new matching apprenticeships are available
    
  @WireMockServer
  @RunOnEnvironment
  Scenario: Sign in to create a saved search
    When I navigate to the following url: /apprenticeships?sort=DistanceAsc&location=Manchester%2C+Greater+Manchester&distance=10
    Then the page is successfully returned
    And the page content includes the following: No results found
    And the page content includes the following: Sign in to create an alert for this search</a>
    
  @AuthenticatedUser
  @WireMockServer
  @RunOnEnvironment
  Scenario: Can save a search
    When I navigate to the following url: /apprenticeships?sort=DistanceAsc&location=Manchester%2C+Greater+Manchester&distance=10
    Then the page is successfully returned
    And the page content includes the following: No results found
    And the page content includes the following: Create an alert for this search</button>
  
  @AuthenticatedUser
  @WireMockServer
  @RunOnEnvironment
  Scenario: Save a search and delete
    When I navigate to the following url: /apprenticeships?sort=DistanceAsc&location=Manchester%2C+Greater+Manchester&distance=10
    And I save my search
    When I navigate to the following url: /saved-searches
    Then the page is successfully returned
    And the page content includes the following: All apprenticeships in Manchester, Greater Manchester
    And I go to delete saved search page
    And I delete the saved search
    Then I am redirected to the following url: /saved-searches
    And the page redirect content includes the following: Search alert ^for All apprenticeships in Manchester, Greater Manchester^ deleted