Feature: GetApplications

As an FAA User
I can see all the applications I have made
So that I can see their status and resume draft ones

@WireMockServer
@AuthenticatedUser
Scenario: See my draft applications
	When I navigate to the Applications page
	Then the page is successfully returned
	And the page content includes the following: Your applications

@WireMockServer
@AuthenticatedUser
Scenario: See my submitted applications
	When I navigate to the Applications page with querystring parameters
		| Field  | Value     |
		| tab    | Submitted |
	Then the page is successfully returned
	And the page content includes the following: Submitted

@WireMockServer
@AuthenticatedUser
Scenario: See my withdrawn applications
	When I navigate to the Applications page with querystring parameters
		| Field  | Value     |
		| tab    | Submitted |
	Then the page is successfully returned
	And the page content includes the following: Withdrawn

@WireMockServer
@AuthenticatedUser
Scenario: See my expired applications
	When I navigate to the Applications page
	Then the page is successfully returned
	And the page content includes the following: Expired
