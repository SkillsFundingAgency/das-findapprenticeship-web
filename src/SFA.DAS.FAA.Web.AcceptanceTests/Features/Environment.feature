Feature: Environment

As an outer API consumer
I want to know that my responses are targetting the API correctly
So that I dont have any unexpected errors

@ApiContract
Scenario: Check Outer API matches defined responses
    Then all the api response types match