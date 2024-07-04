Feature: Home

As an FAA user
I want a clear home page
So that it is clear what actions I can take

@WireMockServer
	Scenario: Navigate to home page
	When I navigate to the following url: /apprenticeshipsearch
	Then a http status code of 200 is returned
	And the page content includes the following: Search apprenticeships
	And the page content includes the following: 1,034 apprenticeships currently listed

@WireMockServer
	Scenario: Location search from home page no location found
	When I navigate to the following url: /apprenticeshipsearch/?whereSearchTerm=Coventry
	Then a http status code of 200 is returned
	And the page content includes the following error: We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby

@WireMockServer
	Scenario: Location search from home page no option selected
	When I navigate to the following url: /apprenticeshipsearch/?search=1
	Then I am redirected to the following url: /apprenticeships
	
@WireMockServer
	Scenario: Location search from home page with valid entry
	When I navigate to the following url: /apprenticeshipsearch/?whereSearchTerm=Manchester&search=1
	Then I am redirected to the following url: /apprenticeships

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
	Then I am redirected to the following url: /location
	And the page redirect content includes the following: What is your location?

@WireMockServer
	Scenario: Browse by interests with validation failure
	When I post an empty form to the following url: /browse-by-interests
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
	Then the page content includes the following error: We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby

	
@WireMockServer
Scenario: Location search no option selected
	When I post an empty form to the following url: /location
	Then the page content includes the following error: Select if you want to enter a city or postcode
	
@WireMockServer
Scenario: Location search option selected no postcode
	When I post to the following url: /location
	  | Field          | Value |
	  | NationalSearch | false |
	Then the page content includes the following error: Enter a city or postcode 

@WireMockServer
Scenario: Navigate to search results page with no filters
	When I navigate to the following url: /apprenticeships
	Then a http status code of 200 is returned
	And the page content includes the following: 339 apprenticeships found

@WireMockServer
Scenario: Navigate to search results page with no results found
	When I navigate to the following url: /apprenticeships?location=manchester
	Then a http status code of 200 is returned
	And the page content includes the following: No apprenticeships found

@WireMockServer
Scenario: Navigate to vacancy details page with vacancy found
	When I navigate to the following url: /apprenticeship/VAC1000012484
	Then a http status code of 200 is returned
	And the page content includes the following: Summary

@WireMockServer
Scenario: Navigate to vacancy details page with invalid vacancy reference format
	When I navigate to the following url: /apprenticeship/ABC1000012484
	Then a http status code of 404 is returned
	And the page content includes the following: Page not found

@WireMockServer
Scenario: Navigate to cookies page
	When I navigate to the following url: /Home/cookies
	Then a http status code of 200 is returned
	And the page content includes the following: Cookies
