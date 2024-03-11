Feature: DisabilityConfident

As an FAA User
I can complete the Disability Confident section
So that I can choose whether or not to apply under the scheme

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: See my options for Disability Confident
	When I navigate to the Disability Confident page
	Then a http status code of 200 is returned
	And the page content includes the following: Equal opportunity
	And the page content includes the following: Disability Confident scheme
