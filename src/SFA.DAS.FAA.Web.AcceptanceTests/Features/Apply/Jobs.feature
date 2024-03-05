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
Scenario: Fill out the Add a job page
	When I post to the following url: /apply/1b82e2a2-e76e-40c7-8a20-5736bed1afd1/jobs/add
		| Field          | Value                                       |
		| JobTitle       | Lapidary worker                             |
		| EmployerName   | Rocks u Like                                |
		| JobDescription | Polishing semi-precious stones and the like |
		| StartDateMonth | 07                                          |
		| StartDateYear  | 2021                                        |
		| IsCurrentRole  | true                                        |
	Then I am redirected to the following url: /apply/1b82e2a2-e76e-40c7-8a20-5736bed1afd1/jobs

@WireMockServer
@AuthenticatedUser
Scenario: Validation on the Add a job page
	When I post an empty form to the following url: /apply/1b82e2a2-e76e-40c7-8a20-5736bed1afd1/jobs/add
	Then the page content includes the following error: Enter the job title for this job
	And the page content includes the following error: Enter the company you worked for
	And the page content includes the following error: Enter the responsibilities you had for this job
	And the page content includes the following error: Enter the start date for this job
	And the page content includes the following error: Select if this is your current job

@WireMockServer
@AuthenticatedUser
Scenario: Validation error on job summary page
	When I post an empty form to the following url: /apply/1b82e2a2-e76e-40c7-8a20-5736bed1afd1/jobs
	Then a http status code of 200 is returned
	And the page content includes the following error: Select if you want to add any jobs

@WireMockServer
@AuthenticatedUser
Scenario: Completion of job section without entering any jobs
	When I post to the following url: /apply/1b82e2a2-e76e-40c7-8a20-5736bed1afd1/jobs
	  | Field					   | Value |
	  | DoYouWantToAddAnyJobs	   | false |
	Then I am redirected to the following url: /applications/1b82e2a2-e76e-40c7-8a20-5736bed1afd1

@WireMockServer
@AuthenticatedUser
Scenario: Revisit the jobs page having already completed it previously
	When I navigate to the following url: /apply/676476cc-525a-4a13-8da7-cf36345e6f61/jobs
	Then a http status code of 200 is returned
	And the page content includes the following: Work history
	And the page content includes the following: Jobs
	And the page content includes the following: Junior Developer

@WireMockServer
@AuthenticatedUser
Scenario: Confirm completion of the jobs page on a return visit
	When I post to the following url: /apply/676476cc-525a-4a13-8da7-cf36345e6f61/jobs
	| Field              | Value |
	| IsSectionCompleted | false |
	| ShowJobHistory     | true  |
	Then I am redirected to the following url: /applications/676476cc-525a-4a13-8da7-cf36345e6f61

@WireMockServer
@AuthenticatedUser
Scenario: Edit an existing job
	When I navigate to the following url: /apply/676476cc-525a-4a13-8da7-cf36345e6f61/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae
	Then a http status code of 200 is returned
	And the page content includes the following: Work history
	And the page content includes the following: Add a job

@WireMockServer
@AuthenticatedUser
Scenario: Delete a job
	When I navigate to the following url: /apply/676476cc-525a-4a13-8da7-cf36345e6f61/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae/delete
	Then a http status code of 200 is returned
	And the page content includes the following: Do you want to delete this job?
	And the page content includes the following: Junior Developer

	