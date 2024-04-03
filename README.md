## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# das-findapprenticeship-web

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2Fdas-findapprenticeship-web?repoName=SkillsFundingAgency%2Fdas-findapprenticeship-web&branchName=refs%2Fpull%2F59%2Fmerge)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3455&repoName=SkillsFundingAgency%2Fdas-findapprenticeship-web&branchName=refs%2Fpull%2F59%2Fmerge)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-findapprenticeship-web&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-findapprenticeship-web)

## About

The [Find an apprenticeship](https://www.gov.uk/apply-apprenticeship) service is used by jobseekers and potential apprentices who are looking for an apprenticeship.

## Requirements

- .net 6 and any supported IDE for dev running.
- Azure storage emulator

## Local running

- You must have the Azure storage emulator running. In it you will need a table called 'Configuration'. In that table add the following row:

PartitionKey: LOCAL
RowKey: SFA.DAS.FindApprenticeship.Web_1.0
Data:
```
{
      "FindAnApprenticeship": {},
      "FindAnApprenticeshipOuterApi": {
        "BaseUrl": "http://localhost:5027/",
        "Key": "123"
  }
}
```

- Make sure the `FindAnApprenticeshipOuterApi` `BaseUrl` is pointing at the mock server base url.
- Run the mock server project, this creates a mock server on `http://localhost:5027/`
- Run the web project