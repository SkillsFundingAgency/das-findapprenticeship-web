Feature: Unsubscribe Saved Search

As an FAA user
I want a clear home page
So that it is clear what actions I can take

@WireMockServer
Scenario: Navigate to unsubscribe page
    When I navigate to the following url: /saved-searches/unsubscribe?id=00EA277E-BF45-4110-B7A2-7AA5A32F31B7
    Then the page is successfully returned
    And the page content includes the following: Do you want to delete this search alert?

@WireMockServer    
Scenario: Navigate to unsubscribe page and delete
    When I navigate to the following url: /saved-searches/unsubscribe?id=00EA277E-BF45-4110-B7A2-7AA5A32F31B7
    Then the page is successfully returned
    When I post to the Saved Search Unsubscribe page
    | Field | Value                                |
    | Id    | 00EA277E-BF45-4110-B7A2-7AA5A32F31B7 |
    | Title | My Deleted Search Alert              |
    Then I am redirected to the Saved Search Unsubscribe Complete page
    Then the page is redirected
    And the page redirect content includes the following:  Search alert deleted
    And the page redirect content includes the following:  My Deleted Search Alert