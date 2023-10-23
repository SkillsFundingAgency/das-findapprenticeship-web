#Feature: Home
#
#As an FAA user
#I want a clear home page
#So that it is clear what actions I can take
#
#@WireMockServer
#	Scenario: Navigate to home page
#	When I navigate to the following url: /
#	Then a http status code of 200 is returned
#	And the page content includes the following: Search apprenticeships
#
#@WireMockServer
#	Scenario: Navigate to the browse by your interests page
#	When I navigate to the following url: /browse-by-interests
#	Then a http status code of 200 is returned
#	And the page content includes the following: Browse by your interests
#
#@WireMockServer
#	Scenario: Browse by interests no validation failure
#	When I post to the following url: /browse-by-interests
#	| Field | Value |
#	|SelectedRouteIds         | 1             |
#	Then a http status code of 200 is returned
#	And I am redirected to the following url: /location
#
#@WireMockServer
#	Scenario: Browse by interests with validation failure
#	When I post to the following url: /browse-by-interests
#		| Field | Value |
#	Then a http status code of 200 is returned
#	And the page content includes the following: Select at least one job category you're interested in
#
#@WireMockServer
#	Scenario: Navigate to the location page
#	When I navigate to the following url: /location
#	Then a http status code of 200 is returned
#	And the page content includes the following: What is your location?
