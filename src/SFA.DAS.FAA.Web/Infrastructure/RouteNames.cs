namespace SFA.DAS.FAA.Web.Infrastructure;

public static class RouteNames
{
    public const string ServiceStartDefault = "default";
    public const string BrowseByInterests = "browse-by-interests";
    public const string Location = "location";
    public const string SearchResults = "search-results";
    public const string MapSearchResults = "map-search-results";
    public const string Vacancies = "vacancies";
    public const string SaveVacancy = nameof(SaveVacancy);
    public const string DeleteSavedVacancy = nameof(DeleteSavedVacancy);
    public const string VacanciesReference = "vacancies-reference";
    public const string Apply = "apply";
    public const string UserName = "user-name";
    public const string DateOfBirth = "date-of-birth";
    public const string PostcodeAddress = "postcode-address";
    public const string SelectAddress = "select-address";
    public const string EnterAddressManually = "enter-address";
    public const string PhoneNumber = "phone-number";
    public const string NotificationPreferences = "notification-preferences";
    public const string NotificationPreferencesSkip = "notification-preferences-skip";
    public const string ConfirmAccountDetails = "check-your-account-details";
    public const string CreateAccount = "create-account";
    public const string TransferYourData = "transfer-your-data";
    public const string SignInToYourOldAccount = "sign-in-to-your-old-account";
    public const string SignOut = "signout";
    public const string SignIn = "sign-in";
    public const string AccountUnavailable = "account-unavailable";
    public const string StubAccountDetailsGet = "account-details-get";
    public const string StubAccountDetailsPost = "account-details-post";
    public const string StubSignedIn = "stub-signedin-get";
    public const string Settings = "settings";
    public const string ChangeNotificationPreferences = "change-notification-preferences";
    public const string Email = "email";
    public const string SavedVacancies = "saved-vacancies";
    public const string ConfirmDataTransfer = "confirm-data-transfer";
    public const string FinishAccountSetup = "finish-account-setup";
    public const string GetHelp = "get-help";
    public const string Cookies = "cookies";
    public const string AccessibilityStatement = "accessibility-statement";
    public const string TermsAndConditions = "terms-and-conditions";
    public const string EmailAlreadyMigrated = "email-already-migrated";
    public const string AccountFound = "account-found";
    public const string AccountFoundTermsAndConditions = "account-found-terms-and-conditions";
    public const string ConfirmAccountDelete = "confirm-account-deletion";
    public const string AccountDelete = "account-delete";
    public const string AccountDeleteWithDrawApplication = "account-deletion-withdraw-applications";
    public const string SaveVacancyFromDetailsPage = "save-vacancy-details-page";
    public const string DeleteSavedVacancyFromDetailsPage = "delete-vacancy-details-page";
    public const string SaveVacancyFromSearchResultsPage = "save-vacancy-search-results-page";
    public const string DeleteSavedVacancyFromSearchResultsPage = "delete-vacancy-search-results-page";
    public const string SaveSearch = "save-search";
    public const string SavedSearches = "saved-searches";
    public const string DeleteSavedSearch = "delete-saved-search";

    public static class Applications
    {
        public const string ViewApplications = "view-applications";
        public const string DeleteApplication = "delete-application";
        public const string ViewApplication = "view-application";
        public const string WithdrawApplicationGet = "withdraw-application-get";
        public const string WithdrawApplicationPost = "withdraw-application-post";
    }

    public static class ApplyApprenticeship
    {
        public const string Preview = nameof(Preview);
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
        public const string EqualityFlow = nameof(EqualityFlow);
        public const string DisabilityConfident = nameof(DisabilityConfident);
        public const string DisabilityConfidentConfirmation = nameof(DisabilityConfidentConfirmation);
        public const string Qualifications = nameof(Qualifications);
        public const string AddQualificationSelectType = nameof(AddQualificationSelectType);
        public const string AddQualification = nameof(AddQualification);
        public const string DeleteQualifications = nameof(DeleteQualifications);
        public const string ApplicationSubmitted = nameof(ApplicationSubmitted);
        public const string ApplicationSubmittedConfirmation = nameof(ApplicationSubmittedConfirmation);

        public static class EqualityQuestions
        {
            public const string EqualityFlowGender = nameof(EqualityFlowGender);
            public const string EqualityFlowEthnicGroup = nameof(EqualityFlowEthnicGroup);
            public const string EqualityFlowEthnicSubGroupWhite = nameof(EqualityFlowEthnicSubGroupWhite);
            public const string EqualityFlowEthnicSubGroupMixed = nameof(EqualityFlowEthnicSubGroupMixed);
            public const string EqualityFlowEthnicSubGroupAsian = nameof(EqualityFlowEthnicSubGroupAsian);
            public const string EqualityFlowEthnicSubGroupBlack = nameof(EqualityFlowEthnicSubGroupBlack);
            public const string EqualityFlowEthnicSubGroupOther = nameof(EqualityFlowEthnicSubGroupOther);
            public const string EqualityFlowSummary = nameof(EqualityFlowSummary);
            public const string EditEqualityQuestions = nameof(EditEqualityQuestions);
        }
    }

    public static class UserProfile
    {
        public const string YourApplications = nameof(YourApplications);
    }

    public static class Error
    {
        public const string Error403 = "403";
        public const string Error404 = "404";
        public const string Error500 = "500";
    }
}