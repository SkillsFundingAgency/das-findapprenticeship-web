Feature: Jobs

As an FAA User
I can complete the Jobs section
So that I can show my work experience

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: See my options for entering jobs
	When I navigate to the Jobs page
	Then a http status code of 200 is returned
	And the page content includes the following: Work history
	And the page content includes the following: Jobs

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Opt to add my first job
	When I post to the Jobs page
	  | Field					   | Value |
	  | DoYouWantToAddAnyJobs	   | true  |
	Then I am redirected to the Add a Job page

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Add a job
	When I navigate to the Add a Job page
	Then a http status code of 200 is returned
	And the page content includes the following: Work history
	And the page content includes the following: Add a job

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Fill out the Add a job page
	When I post to the Add a Job page
		| Field          | Value                                       |
		| JobTitle       | Lapidary worker                             |
		| EmployerName   | Rocks u Like                                |
		| JobDescription | Polishing semi-precious stones and the like |
		| StartDateMonth | 07                                          |
		| StartDateYear  | 2021                                        |
		| IsCurrentRole  | true                                        |
	Then I am redirected to the Jobs page

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Validation on the Add a job page
	When I post an empty form to the Add a Job page
	Then the page content includes the following error: Enter the job title for this job
	And the page content includes the following error: Enter the company you worked for
	And the page content includes the following error: Enter the responsibilities you had for this job
	And the page content includes the following error: Enter the start date for this job
	And the page content includes the following error: Select if this is your current job

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Validation error on job summary page
	When I post an empty form to the Jobs page
	Then a http status code of 200 is returned
	And the page content includes the following error: Select if you want to add any jobs

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Completion of job section without entering any jobs
	When I post to the Jobs page
	  | Field					   | Value |
	  | DoYouWantToAddAnyJobs	   | false |
	Then I am redirected to the Application Tasklist page

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Revisit the jobs page having already completed it previously
	When I navigate to the Jobs page
	Then a http status code of 200 is returned
	And the page content includes the following: Work history
	And the page content includes the following: Jobs
	And the page content includes the following: Junior Developer

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Confirm completion of the jobs page on a return visit
	When I post to the Jobs page
	| Field              | Value |
	| IsSectionCompleted | false |
	| ShowJobHistory     | true  |
	Then I am redirected to the Application Tasklist page

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Edit an existing job
	When I navigate to the Edit Job page
	Then a http status code of 200 is returned
	And the page content includes the following: Work history
	And the page content includes the following: Add a job

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Complete edit of an existing job
	When I post to the Edit Job page
		| Field          | Value                                       |
		| JobTitle       | Lapidary worker level 2                     |
		| EmployerName   | Rocks u Like                                |
		| JobDescription | Polishing semi-precious stones and the like |
		| StartDateMonth | 09                                          |
		| StartDateYear  | 2022                                        |
		| IsCurrentRole  | true                                        |
	Then I am redirected to the Jobs page

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Delete a job
	When I navigate to the Delete Job page
	Then a http status code of 200 is returned
	And the page content includes the following: Do you want to delete this job?
	And the page content includes the following: Junior Developer

@WireMockServer
@AuthenticatedUser
@ExistingApplication
Scenario: Confirm deletion of a job
	When I post an empty form to the Delete Job page
	Then I am redirected to the Jobs page

