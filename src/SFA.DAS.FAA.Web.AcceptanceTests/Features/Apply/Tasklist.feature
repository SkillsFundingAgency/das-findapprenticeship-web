Feature: TaskList

As an FAA User
I can complete the Application Task List
So that I can apply for a vacancy

@WireMockServer
@AuthenticatedUser
Scenario: Continue a started application
	When I post an empty form to the following url: /vacancies/1000012013
	Then I am redirected to the following url: /applications/676476cc-525a-4a13-8da7-cf36345e6f61

@WireMockServer
@AuthenticatedUser
Scenario: Start a new application
	When I post an empty form to the following url: /vacancies/2000012013
	Then I am redirected to the following url: /applications/1b82e2a2-e76e-40c7-8a20-5736bed1afd1

	
