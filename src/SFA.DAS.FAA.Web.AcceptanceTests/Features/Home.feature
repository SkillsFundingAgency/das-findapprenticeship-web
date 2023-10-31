Feature: Home

As an FAA user
I want a clear home page
So that it is clear what actions I can take

@WireMockServer
	Scenario: Navigate to home page
	When I navigate to the following url: /
	Then a http status code of 200 is returned
	And the page content includes the following: Search apprenticeships
	And the page content includes the following: 1,034 apprenticeships currently listed

@WireMockServer
	Scenario: Navigate to the browse by your interests page
	When I navigate to the following url: /browse-by-interests
	Then a http status code of 200 is returned
	And the page content includes the following: Browse by your interests

@WireMockServer
	Scenario: Browse by interests no validation failure
	When I post to the following url: /browse-by-interests
	| Field            | Value |
	| SelectedRouteIds | 1     |
	Then a http status code of 302 is returned
	And I am redirected to the following url: /location
	And the page redirect content includes the following: What is your location?

@WireMockServer
	Scenario: Browse by interests with validation failure
	When I post to the following url: /browse-by-interests
		| Field | Value |
	Then a http status code of 200 is returned
	And the page content includes the following: Select at least one job category

@WireMockServer
	Scenario: Navigate to the location page
	When I navigate to the following url: /location
	Then a http status code of 200 is returned
	And the page content includes the following: What is your location?
	
@WireMockServer
Scenario: Location search no location found
	When I post to the following url: /location
	  | Field                     | Value    |
	  | NationalSearch            | false    |
	  | SearchTerm                | Coventry |
	  | Distance                  | 10       |
	  | SuggestedLocationSelected | true     |
	Then a http status code of 200 is returned
	And I am redirected to the following url: /location
	And the page content includes the following: There is a problem
	And the page content includes the following: We don&#x27;t recognise this city or postcode. Check what you&#x27;ve entered or enter a different location that&#x27;s nearby
	

	
@WireMockServer
Scenario: Location search no option selected
	When I post to the following url: /location
	  | Field          | Value    |
	Then a http status code of 200 is returned
	And I am redirected to the following url: /location
	And the page content includes the following: There is a problem
	And the page content includes the following: Select if you want to enter a city or postcode 
	
@WireMockServer
Scenario: Location search option selected no postcode
	When I post to the following url: /location
	  | Field          | Value |
	  | NationalSearch | false |
	Then a http status code of 200 is returned
	And I am redirected to the following url: /location
	And the page content includes the following: There is a problem
	And the page content includes the following: Enter a city or postcode 