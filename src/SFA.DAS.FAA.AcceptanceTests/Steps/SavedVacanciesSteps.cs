using Reqnroll;

namespace SFA.DAS.FAA.AcceptanceTests.Steps;

[Binding]
public class SavedVacanciesSteps
{
    [Given(@"I have no saved vacancies")]
    public void GivenIHaveNoSavedVacancies()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"the vacancy shows in the Saved Vacancies section")]
    public void ThenTheVacancyShowsInTheSavedVacanciesSection()
    {
        ScenarioContext.StepIsPending();
    }
}