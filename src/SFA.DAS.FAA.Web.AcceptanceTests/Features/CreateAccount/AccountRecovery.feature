Feature: AccountRecovery

As an FAA User
I want to be able to perform a recovery of my legacy account
So that I can import all my applications and personal details

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: See banner about transferring my legacy account
	When I navigate to the Create Account page
	Then the page is successfully returned
	And the page content includes the following: transfer your data

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Opt to transfer my legacy account
	When I navigate to the Transfer Your Data page
	Then the page is successfully returned
	And the page content includes the following: Transfer your data to the new Find an apprenticeship


@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Proceed with transferring my legacy account
	When I navigate to the Sign into your Old Account page
	Then the page is successfully returned
	And the page content includes the following: Sign in to your old Find an apprenticeship account

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Attempt to login to my legacy account with invalid credentials
	When I post to the Sign into your Old Account page
	  | Field    | Value            |
	  | email    | invalid@test.com |
	  | password | password         |
	Then the page is successfully returned
	And the page content includes the following: There is a problem
	And the page content includes the following: Check your account details.
	And the page content includes the following: entered an incorrect email address or password.

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Attempt to login to my legacy account with no email address
	When I post to the Sign into your Old Account page
	  | Field    | Value    |
	  | email    |          |
	  | password | password |
	Then the page is successfully returned
	And the page content includes the following: There is a problem
	And the page content includes the following: Enter your email address

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Attempt to login to my legacy account with an invalid format for email address
	When I post to the Sign into your Old Account page
	  | Field    | Value           |
	  | email    | thisaintnoemail |
	  | password | password        |
	Then the page is successfully returned
	And the page content includes the following: There is a problem
	And the page content includes the following: Enter an email address in the correct format, like name@example.com

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Attempt to login to my legacy account with no password
	When I post to the Sign into your Old Account page
	  | Field    | Value         |
	  | email    | test@test.com |
	  | password |               |
	Then the page is successfully returned
	And the page content includes the following: There is a problem
	And the page content includes the following: Enter your password
