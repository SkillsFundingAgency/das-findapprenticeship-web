Feature: Saved vacancies
    As an FAA User
    I want to save vacancies
    So that I can quickly return to them at a later point in time
    
@Authenticated
Scenario: I can save a vacancy
    Given I have no saved vacancies
    When I search for
        | What       | Where  |
        | apprentice | London |
    And save the first vacancy in the search results
    Then the vacancy shows in the Saved Vacancies section