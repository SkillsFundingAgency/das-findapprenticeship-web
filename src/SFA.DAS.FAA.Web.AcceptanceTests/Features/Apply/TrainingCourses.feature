Feature: Training Courses

As an FAA User
I can complete the Training Courses section
So that I can show my relevant training

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: See my options for entering training courses
	When I navigate to the Training Courses page
	Then a http status code of 200 is returned
	And the page content includes the following: Education history
	And the page content includes the following: Training courses

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Opt to add my first training course
	When I post to the Training Courses page
	  | Field									| Value |
	  | DoYouWantToAddAnyTrainingCourses	    | true  |
	Then I am redirected to the Add a Training Course page
	
@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Opt to add no training courses
	When I post to the Training Courses page
	  | Field									| Value |
	  | DoYouWantToAddAnyTrainingCourses	    | false  |
	Then I am redirected to the Application Tasklist page

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Add a Training Course
	When I navigate to the Add a Training Course page
	Then a http status code of 200 is returned
	And the page content includes the following: Education history
	And the page content includes the following: Add a training course

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Fill out the Add a Training Course page
	When I post to the Add a Training Course page
		| Field          | Value                        |
		| CourseName     | Super Skill Booster Training |
		| YearAchieved   | 2021                         |
	Then I am redirected to the Training Courses page

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Validation on the Add a Training Course page
	When I post an empty form to the Add a Training Course page
	Then the page content includes the following error: Enter the name of this training course
	And the page content includes the following error: Enter the year you finished this training course

@WireMockServer
@AuthenticatedUser
@NewApplication
Scenario: Validation error on job summary page
	When I post an empty form to the Training Courses page
	Then a http status code of 200 is returned
	And the page content includes the following error: Select if you want to add any training courses