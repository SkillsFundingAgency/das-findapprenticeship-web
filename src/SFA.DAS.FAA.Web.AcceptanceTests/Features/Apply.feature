Feature: Apply

As an FAA User
I can complete the Application Task List
So that I can apply for a vacancy

@WireMockServer
@AuthenticatedUser
Scenario: Start an application
	When I post an empty form to the following url: /vacancies/1000012013
	Then I am redirected to the following url: /applications/676476cc-525a-4a13-8da7-cf36345e6f61

