Feature: DisabilityConfident

As an FAA User
I can complete the Disability Confident section
So that I can choose whether or not to apply under the scheme

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: See my options for Disability Confident
	When I navigate to the Disability Confident page
	Then a http status code of 200 is returned
	And the page content includes the following: Equal opportunity
	And the page content includes the following: Disability Confident scheme

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Validation error on disability confident page
	When I post an empty form to the Disability Confident page
	Then a http status code of 200 is returned
	And the page content includes the following error: Select if you want to apply under the Disability Confident scheme

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Opt to apply under the scheme
	When I post to the Disability Confident page
	  | Field								   | Value |
	  | ApplyUnderDisabilityConfidentScheme	   | true  |
	  Then I am redirected to the Disability Confident Confirmation page

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Opt not to apply under the scheme
	When I post to the Disability Confident page
	  | Field								   | Value |
	  | ApplyUnderDisabilityConfidentScheme	   | false  |
	  Then I am redirected to the Disability Confident Confirmation page

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: See my options for Disability Confident having previously answered
	When I navigate to the Disability Confident page
	Then I am redirected to the Disability Confident Confirmation page

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: See my options for Disability Confident while editing previous answer
	When I navigate to the Disability Confident page with querystring parameters
	  | Field  | Value |
	  | isEdit | true  |
	Then a http status code of 200 is returned
	And the page content includes the following: Equal opportunity
	And the page content includes the following: Disability Confident scheme
