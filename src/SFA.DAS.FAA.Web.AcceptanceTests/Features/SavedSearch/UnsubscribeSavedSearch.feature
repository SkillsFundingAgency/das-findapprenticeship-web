Feature: Home

As an FAA user
I want a clear home page
So that it is clear what actions I can take

@WireMockServer
Scenario: Navigate to unsubscribe page
    When I navigate to the following url: /saved-searches/unsubscribe?id=00EA277E-BF45-4110-B7A2-7AA5A32F31B7
    Then a http status code of 200 is returned
    And the page content includes the following: Do you want to delete this search alert?