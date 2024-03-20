Feature: Qualifications

As an FAA User
I can complete the Qualifications section
So that I can show how I am qualified for the role

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: See my options for entering qualifications
	When I navigate to the Qualifications page
	Then a http status code of 200 is returned
	And the page content includes the following: Education history
	And the page content includes the following: School, college and university qualifications 
	And the page content includes the following: Do you want to add any qualifications?

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Opt to add my first qualification
	When I post to the Qualifications page
	  | Field                           | Value |
	  | DoYouWantToAddAnyQualifications | true  |
	Then I am redirected to the Add Qualification Select Type page
	And the page redirect content includes the following: What is your most recent qualification?

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Validation error on qualifications page
	When I post an empty form to the Qualifications page
	Then a http status code of 200 is returned
	And the page content includes the following error: Select if you want to add any qualifications

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: See the qualifications I've already entered
	When I navigate to the Qualifications page
	Then a http status code of 200 is returned
	And the page content includes the following: Education history
	And the page content includes the following: School, college and university qualifications 
	And the page content includes the following: Have you completed this section?

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Validation error on completed qualifications page
	When I post to the Qualifications page
	| Field              | Value |
	| ShowQualifications | true  |
	Then a http status code of 200 is returned
	And the page content includes the following error: Select if you have finished this section

	
@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Confirm completion of the qualifications page
	When I post to the Qualifications page
	| Field              | Value |
	| ShowQualifications | true  |
	| IsSectionCompleted | true |
	Then I am redirected to the Application Tasklist page

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Confirm non-completion of the qualifications page
	When I post to the Qualifications page
	| Field              | Value |
	| ShowQualifications | true  |
	| IsSectionCompleted | false |
	Then I am redirected to the Application Tasklist page

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Delete multiple qualifications of a single type
	When I navigate to the Delete Qualifications (multiple) page
	Then a http status code of 200 is returned
	And the page content includes the following: Do you want to delete these qualifications?

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Delete a single qualification of a single type
	When I navigate to the Delete Qualifications (single) page
	Then a http status code of 200 is returned
	And the page content includes the following: Do you want to delete this qualification?

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Opt to add another qualification
	When I navigate to the Add Qualification Select Type page
	Then the page content includes the following: Add another qualification
