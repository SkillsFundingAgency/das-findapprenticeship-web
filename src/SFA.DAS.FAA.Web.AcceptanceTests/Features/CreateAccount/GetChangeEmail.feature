#Feature: GetChangeEmail
#
#As an FAA User
#I can see the change email page
#So that I can see change my email through Gov.UK
#
#@WireMockServer
#@AuthenticatedUser
#Scenario: See change email page
#	When I navigate to the change email page
#	Then a http status code of 200 is returned
#	And the page content includes the following: Change your email address
