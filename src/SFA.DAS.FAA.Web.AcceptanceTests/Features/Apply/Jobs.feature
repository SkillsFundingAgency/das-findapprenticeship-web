Feature: Jobs

As an FAA User
I can complete the Jobs section
So that I can show my work experience

@WireMockServer
@AuthenticatedUser
Scenario: Add my first job
	When I navigate to the following url: /apply/1b82e2a2-e76e-40c7-8a20-5736bed1afd1/jobs
	Then a http status code of 200 is returned
	And the page content includes the following: Work history
	And the page content includes the following: Jobs

@WireMockServer
@AuthenticatedUser
Scenario: Validation error on adding first job
	When I post an empty form to the following url: /apply/1b82e2a2-e76e-40c7-8a20-5736bed1afd1/jobs
	Then a http status code of 200 is returned
	And the page content includes the following error: Select if you want to add any jobs