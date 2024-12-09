Feature: Home page search
  As an FAA User
  I can visit the FindAnApprenticeship service page
  And search for apprenticeships
  So that I can apply for vacancies

Scenario: I can search from the homepage
  When I visit the HomePage
  And I search for
    | What          |
    | apprentice    |
  Then I am shown the search results
  And My search criteria populated in the sidebar
   
Scenario: I can search from the homepage and specify a search area
  When I visit the HomePage
  And I search for
    | What       | Where  |
    | apprentice | London |
  Then I am shown the search results
  And My search criteria populated in the sidebar