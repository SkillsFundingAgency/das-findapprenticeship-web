namespace SFA.DAS.FAA.Web.Infrastructure
{
    public static class CacheKeys
    {
        public static string EqualityQuestionsDataProtectionKey => "govIdentifier-{0}";
        public static string EqualityQuestions => nameof(EqualityQuestions);
        public static string CreateAccountReturnUrl => nameof(CreateAccountReturnUrl);
        public static string AccountCreated => nameof(AccountCreated);
        public static string AccountFound => nameof(AccountFound);
        public static string AccountDeleted => nameof(AccountDeleted);
        public static string LegacyEmail => nameof(LegacyEmail);
    }
}