## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# das-findapprenticeship-web

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2Fdas-findapprenticeship-web?repoName=SkillsFundingAgency%2Fdas-findapprenticeship-web&branchName=refs%2Fpull%2F59%2Fmerge)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3455&repoName=SkillsFundingAgency%2Fdas-findapprenticeship-web&branchName=refs%2Fpull%2F59%2Fmerge)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-findapprenticeship-web&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-findapprenticeship-web)

[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## About

The [Find an apprenticeship](https://www.gov.uk/apply-apprenticeship) service is used by jobseekers and potential apprentices looking for an apprenticeship. The Find an Apprenticeship requires GovOne login to create a candidate profile and submit an application.

## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* The Outer API [das-apim-endpoints](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/master/src/FindAnApprenticeship) should be available either running locally or accessible in an Azure tenancy.
* The Inner API [das-candidate-account-api](https://github.com/SkillsFundingAgency/das-candidate-account-api) should be available either running locally or accessible in an Azure tenancy.

### Config
You can find the latest config file in [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-findapprenticeship-web/SFA.DAS.FindApprenticeship.Web.json)

In the web project, if it does not exist already, add `AppSettings.Development.json` file with the following content:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "ConfigNames": "SFA.DAS.FindApprenticeship.Web,SFA.DAS.Candidate.GovSignIn",
  "EnvironmentName": "LOCAL",
  "Version": "1.0",
  "APPINSIGHTS_INSTRUMENTATIONKEY": "",
  "AllowedHosts": "*",
  "cdn": {
    "url": "https://das-test-frnt-end.azureedge.net"
  },
  "ResourceEnvironmentName": "LOCAL",
  "StubAuth": "true"
}
```

Make sure you have GovSignIn configuration set up from [das-employer-config repository]
(https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-shared-config/SFA.DAS.Candidate.GovSignIn.json)

using: 
* PartitionKey: LOCAL
* RowKey: SFA.DAS.Candidate.GovSignIn_1.0
* Data: (config as above)

## Technologies
* .NetCore 10.0
* NUnit
* Moq
* FluentAssertions
* Azure App Insights
* MediatR
* Redis

### Running

* Open command prompt and change directory to _**/src/SFA.DAS.FAA.Web/**_
* Run the web project _**/src/SFA.DAS.FAA.Web.csproj**_

MacOS
```
ASPNETCORE_ENVIRONMENT=Development dotnet run
```
Windows cmd
```
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```

* Open second command prompt and change directory to _**/src/SFA.DAS.FAA.MockServer/**_
* Run the mock server project, this creates a mock server on `http://localhost:5027/`

MacOS
```
ASPNETCORE_ENVIRONMENT=Development dotnet run
```
Windows cmd
```
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```
* Browse to `https://localhost:7276/apprenticeshipsearch`

- Make sure the `FindAnApprenticeshipOuterApi` `BaseUrl` is pointing at the mock server base url.


### Application logs
Application logs are logged to [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) and can be viewed using [Azure Monitor](https://learn.microsoft.com/en-us/azure/azure-monitor/overview) at https://portal.azure.com

## Useful URLs

### Home
https://localhost:7276/apprenticeshipsearch -> Home page listing current of available apprenticeships

### Search
https://localhost:7276/apprenticeshipsearch?sort=AgeAsc -> Recently advertised vacancies

https://localhost:7276/apprenticeshipsearch?sort=ClosingAsc -> Closing soon vacancies

https://localhost:7276/apprenticeshipsearch?searchTerm=&location=Coventry%2C+West+Midlands&distance=10&sort=DistanceAsc -> Displays vacancies within 10 miles of Coventry

https://localhost:7276/apprenticeshipsearch?searchTerm=random -> Displays no search results page


### Vacancy
https://localhost:7276/apprenticeship/VAC1000012484 -> View vacancy details

https://localhost:7276/apprenticeship/VACABC1000012484 -> Vacancy not found

https://localhost:7276/saved-vacancies -> Saved Vacancies

### Browse by interests
https://localhost:7276/browse-by-interests -> Browse by your interests

### Applications
https://localhost:7276/applications?tab=Started -> View applications that are started

https://localhost:7276/applications?tab=Submitted -> View applications that are submitted

https://localhost:7276/applications?tab=Successful -> View applications that are successful

https://localhost:7276/applications?tab=Unsuccessful -> View applications that are Unsuccessful

### Profile
https://localhost:7276/settings -> Candidate/User Settings


## License

Licensed under the [MIT license](LICENSE)