Feature: AccountRecovery

As an FAA User
I want to be able to perform a recovery of my legacy account
So that I can import all my applications and personal details

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: See banner about transferring my legacy account
	When I navigate to the Create Account page
	Then a http status code of 200 is returned
	And the page content includes the following: transfer your data

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Opt to transfer my legacy account
	When I navigate to the Transfer Your Data page
	Then a http status code of 200 is returned
	And the page content includes the following: Transfer your data to the new Find an apprenticeship


@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Proceed with transferring my legacy account
	When I navigate to the Sign into your Old Account page
	Then a http status code of 200 is returned
	And the page content includes the following: Sign in to your old Find an apprenticeship account
