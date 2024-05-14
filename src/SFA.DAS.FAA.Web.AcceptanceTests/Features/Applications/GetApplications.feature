Feature: GetApplications

As an FAA User
I can see all the applications I have made
So that I can see their status and resume draft ones

@WireMockServer
@AuthenticatedUser
Scenario: See my draft applications
	When I navigate to the Applications page
	Then a http status code of 200 is returned
	And the page content includes the following: Your applications
