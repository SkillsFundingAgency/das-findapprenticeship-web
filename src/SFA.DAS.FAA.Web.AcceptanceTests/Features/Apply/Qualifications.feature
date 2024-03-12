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

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Opt to add my first qualification
	When I post to the Qualifications page
	  | Field                           | Value |
	  | DoYouWantToAddAnyQualifications | true  |
	Then I am redirected to the Add Qualification Select Type page

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Validation error on qualifications page
	When I post an empty form to the Qualifications page
	Then a http status code of 200 is returned
	And the page content includes the following error: Select if you want to add any qualifications