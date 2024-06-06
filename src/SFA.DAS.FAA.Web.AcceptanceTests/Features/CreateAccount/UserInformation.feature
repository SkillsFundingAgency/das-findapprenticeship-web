Feature: UserInformation

As an FAA User
I want to be able to provide all of my details
So that I can create my account

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Opt to add my user name
	When I navigate to the User Name page
	Then a http status code of 200 is returned
	And the page content includes the following: What is your name?

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Enter my name without my first name
	When I post to the User Name page
		  | Field     | Value |
		  | FirstName |       |
		  | LastName  | Smith |
	Then a http status code of 200 is returned
	And the page content includes the following: There is a problem

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Enter my name without my last name
	When I post to the User Name page
		  | Field     | Value |
		  | FirstName | John  |
		  | LastName  |       |
	Then a http status code of 200 is returned
	And the page content includes the following: There is a problem

	
@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Enter my user name
	When I post to the User Name page
		  | Field     | Value |
		  | FirstName | Joe   |
		  | LastName  | Smith |
	Then I am redirected to the User Date of Birth page

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Opt to enter my date of birth
	When I navigate to the User Date of Birth page
	Then a http status code of 200 is returned
	And the page content includes the following: What is your date of birth?

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Enter my date of birth without a value
	When I post to the User Date of Birth page
		| Field            | Value |
		| DateOfBirthDay   |       |
		| DateOfBirthMonth |       |
		| DateOfBirthYear  |       |
	Then a http status code of 200 is returned
	And the page content includes the following: There is a problem

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Enter my date of birth with an invalid value
	When I post to the User Date of Birth page
		| Field            | Value |
		| DateOfBirthDay   | X     |
		| DateOfBirthMonth | Y     |
		| DateOfBirthYear  | Z     |
	Then a http status code of 200 is returned
	And the page content includes the following: There is a problem

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Enter my date of birth
	When I post to the User Date of Birth page
		| Field            | Value |
		| DateOfBirthDay   | 26    |
		| DateOfBirthMonth | 7     |
		| DateOfBirthYear  | 2001  |
	Then I am redirected to the User Address - Postcode page

@WireMockServer
@AuthenticatedUserWithIncompleteSetup
Scenario: Opt to add my address
	When I navigate to the User Address - Postcode page
	Then a http status code of 200 is returned
	And the page content includes the following: What is your address?
