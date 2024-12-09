Feature: Cookies preferences
    As an FAA User
    I want to specify my cookie settings
    So that I can select my privacy preferences
    
Scenario: I can accept cookies
    Given I visit the HomePage
    When I accept the cookie preferences from the banner
    Then the cookie banner confirms that I accepted cookies
    And the "AnalyticsConsent" cookie is set to "true"
    
Scenario: I can reject cookies
    Given I visit the HomePage
    When I reject the cookie preferences from the banner
    Then the cookie banner confirms that I rejected cookies
    And the "AnalyticsConsent" cookie is set to "false"