Feature: TaskList

As an FAA User
I can complete the Application Task List
So that I can apply for a vacancy

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Continue a started application
	When I post an empty form to the Vacancy Details page
	Then I am redirected to the Application Tasklist page

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Start a new application
	When I post an empty form to the Vacancy Details page
	Then I am redirected to the Application Tasklist page

	
