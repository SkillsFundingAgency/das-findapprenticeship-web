Feature: VolunteeringAndWorkExperience

As an FAA User
I can complete the Volunteering and Work Experience section
So that I can show work I've volunteered for

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: See my options for entering work experience
	When I navigate to the Volunteering and Work Experience page
	Then the page is successfully returned
	And the page content includes the following: Work history
	And the page content includes the following: Volunteering and work experience

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Opt to add my work experience
	When I post to the Volunteering and Work Experience page
	  | Field                                         | Value |
	  | DoYouWantToAddAnyVolunteeringOrWorkExperience | true  |
	Then I am redirected to the Add Work Experience page

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Revisit the work experience page having previously completed it
	When I navigate to the Volunteering and Work Experience page
	Then I am redirected to the Volunteering and Work Experience Summary page

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Revisit the work experience summary page having previously completed it
	When I navigate to the Volunteering and Work Experience Summary page
	Then the page is successfully returned
	And the page content includes the following: Work history
	And the page content includes the following: Volunteering and work experience

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Delete work experience
	When I navigate to the Delete Work Experience page
	Then the page is successfully returned
	And the page content includes the following: Work history
	And the page content includes the following: Do you want to delete this volunteering or work experience?
	And the page content includes the following: Cleaning bottles

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Mark the work experience section as complete
	When I post to the Volunteering and Work Experience Summary page
	| Field                  | Value |
	| IsSectionCompleted | true  |
	Then I am redirected to the Application Tasklist page
	

