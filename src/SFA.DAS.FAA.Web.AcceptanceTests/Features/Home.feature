Feature: Home

As an FAA user
I want a clear home page
So that it is clear what actions I can take

@WireMockServer
	Scenario: Navigate to home page has total vacancy positions
	When I navigate to the Home page
	Then the page is successfully returned
	And the page content includes the following: Search apprenticeships
	And the page content includes the following: 1,034 apprenticeships currently listed
	
@AuthenticatedUser
@WireMockServer
Scenario: Navigate to home page as an authenticated user with saved searches shows saved searches on home page
	When I navigate to the Home page
	Then the page is successfully returned
	And the page content includes the following: Search apprenticeships
	And the page content includes the following: Your search alerts

@WireMockServer
	Scenario: Location search from home page with non existing location shows no location found
	When I navigate to the Home page with querystring parameters
	  | Field           | Value    |
	  | whereSearchTerm | Coventry |	
	Then the page is successfully returned
	And the page content includes the following error: We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby

@WireMockServer
	Scenario: Location search from home page no option selected
	When I navigate to the Home page with querystring parameters
	  | Field  | Value |
	  | search | 1     |
	Then I am redirected to the Search Results page
	
@WireMockServer
	Scenario: Location search from home page with valid location shows search results
	When I navigate to the Home page with querystring parameters
	  | Field           | Value      |
	  | whereSearchTerm | Manchester |
	  | &search          | 1          |
	Then I am redirected to the Search Results page

@WireMockServer @RunOnEnvironment
Scenario: Navigate to the browse by your interests page
When I navigate to the following url: /browse-by-interests
Then the page is successfully returned
And the page content includes the following: Browse by your interests

@WireMockServer
Scenario: Browse by interests no validation failure
When I post to the following url: /browse-by-interests
	| Field            | Value |
	| SelectedRouteIds | 1     |
Then I am redirected to the Location page
And the page redirect content includes the following: Location
	
@WireMockServer @RunOnEnvironment
	Scenario: Browse by interests with validation failure
	When I post an empty form to the following url: /browse-by-interests
	Then the page is successfully returned
	And the page content includes the following: Select at least one job category

@WireMockServer @RunOnEnvironment
	Scenario: Navigate to the location page
	When I navigate to the following url: /location
	Then the page is successfully returned
	And the page content includes the following: Location
	
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
	
@WireMockServer @RunOnEnvironment
Scenario: Location search option selected no postcode
	When I post to the following url: /location
	  | Field          | Value |
	  | NationalSearch | false |
	Then the page content includes the following error: Enter a city or postcode 

@WireMockServer
Scenario: Navigate to search results page with no filters
	When I navigate to the following url: /apprenticeships
	Then the page is successfully returned
	And the page content includes the following: 339 results found

@WireMockServer
Scenario: Navigate to search results page with no results found
	When I navigate to the following url: /apprenticeships?location=manchester
	Then the page is successfully returned
	And the page content includes the following: No results found

@WireMockServer
Scenario: Navigate to vacancy details page with vacancy found
	When I navigate to the following url: /apprenticeship/VAC1000012484
	Then the page is successfully returned
	And the page content includes the following: Summary

@WireMockServer
Scenario: Navigate to vacancy details page with invalid vacancy reference format
	When I navigate to the following url: /apprenticeship/ABC1000012484
	Then the page is successfully returned
	And the page content includes the following: Page not found


@WireMockServer
Scenario: Navigate to vacancy details page with closed vacancy
	When I navigate to the following url: /apprenticeship/VAC1000012333
	Then the page is successfully returned
	And the page content includes the following: You can no longer apply for this apprenticeship

@WireMockServer
Scenario: Navigate to cookies page
	When I navigate to the following url: /Home/cookies
	Then the page is successfully returned
	And the page content includes the following: Cookies

@WireMockServer
Scenario: Navigate to Accessibility Statement page
	When I navigate to the following url: /Home/Accessibility-statement
	Then the page is successfully returned
	And the page content includes the following: Accessibility

@WireMockServer
Scenario: Navigate to Terms and Conditions page
	When I navigate to the following url: /Home/terms-and-conditions
	Then the page is successfully returned
	And the page content includes the following: Terms and conditions

@WireMockServer
Scenario: Navigate to Get Help page
	When I navigate to the following url: /Home/get-help
	Then the page is successfully returned
	And the page content includes the following: Get help
