namespace SFA.DAS.FAA.Web.Infrastructure;

public static class RouteNames
{
    public const string ServiceStartDefault = "default";
    public const string BrowseByInterests = "browse-by-interests";
    public const string Location = "location";
    public const string SearchResults = "search-results";
    public const string Vacancies = "vacancies";
    public const string Apply = "apply";
    public const string UserName = "user-name";
    public const string DateOfBirth = "date-of-birth";
    public const string CreateAccount = "create-account";
    public const string SignOut = "sign-out";
    public const string SignIn = "sign-in";
    public const string SignedOut = "signed-out";
    public const string AccountUnavailable = "account-unavailable";
    public const string StubAccountDetailsGet = "account-details-get";
    public const string StubAccountDetailsPost = "account-details-post";
    public const string StubSignedIn = "stub-signedin-get";

    public static class ApplyApprenticeship
    {
        public const string AddJob = nameof(AddJob);
        public const string Jobs = nameof(Jobs);
        public const string JobsSummary = nameof(JobsSummary);
        public const string WorkHistory = nameof(WorkHistory);
		public const string EditJob = nameof(EditJob);
        public const string DeleteJob = nameof(DeleteJob);
        public const string AddTrainingCourse = nameof(AddTrainingCourse);
        public const string TrainingCourses = nameof(TrainingCourses);
        public const string EditTrainingCourse = nameof(EditTrainingCourse);
        public const string DeleteTrainingCourse = nameof(DeleteTrainingCourse);
        public const string VolunteeringAndWorkExperience = nameof(VolunteeringAndWorkExperience);
        public const string DeleteVolunteeringOrWorkExperience = nameof(DeleteVolunteeringOrWorkExperience);
        public const string AddVolunteeringAndWorkExperience = nameof(AddVolunteeringAndWorkExperience);
        public const string SkillsAndStrengths = nameof(SkillsAndStrengths);
        public const string VolunteeringAndWorkExperienceSummary = nameof(VolunteeringAndWorkExperienceSummary);
        public const string EditVolunteeringAndWorkExperience = nameof(EditVolunteeringAndWorkExperience);
        public const string AddAdditionalQuestion = nameof(AddAdditionalQuestion);
        public const string InterviewAdjustments = nameof(InterviewAdjustments);
        public const string InterviewAdjustmentsSummary = nameof(InterviewAdjustmentsSummary);
        public const string WhatInterestsYou = nameof(WhatInterestsYou);
        public const string DisabilityConfident = nameof(DisabilityConfident);
        public const string DisabilityConfidentConfirmation = nameof(DisabilityConfidentConfirmation);
        public const string ApplicationSubmitted = nameof(ApplicationSubmitted);
        public const string EqualityFlow = nameof(EqualityFlow);
    }

    public static class UserProfile
    {
        public const string YourApplications = nameof(YourApplications);
    }
}