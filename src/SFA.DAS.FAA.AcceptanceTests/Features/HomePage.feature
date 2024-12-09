Feature: Home page search
    As an FAA User
    I can visit the FindAnApprenticeship service page
    And search for apprenticeships
    So that I can apply for vacancies


Scenario: I can visit the homepage
    Given I visit the HomePage
    Then the page title is "Search apprenticeship – Find an apprenticeship – GOV.UK"
    And the page heading is "Search apprenticeships"
    
Scenario: I can search from the homepage
    Given I visit the HomePage
    When I search for
          | What          |
          | apprentice    |
    Then I am shown the search results
    And my search criteria are populated in the sidebar
   
@Authenticated
Scenario: I can search from the homepage and specify a search area
    Given I visit the HomePage
    When I search for
        | What       | Where  |
        | apprentice | London |
    Then I am shown the search results
    And my search criteria are populated in the sidebar