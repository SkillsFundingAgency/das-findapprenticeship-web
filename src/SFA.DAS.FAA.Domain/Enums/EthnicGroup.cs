using System.ComponentModel;

namespace SFA.DAS.FAA.Domain.Enums
{
    public enum EthnicGroup : short
    {
        [Description("White")]
        White = 1,
        [Description("Mixed or multiple ethnic groups")]
        MixedOrMultiple = 2,
        [Description("Asian or Asian British")]
        AsianOrAsianBritish = 3,
        [Description("Black, African, Caribbean or Black British")]
        BlackOrAfricanOrCaribbeanOrBlackBritish = 4,
        [Description("Other ethnic group")]
        Other = 5,
        [Description("Prefer not to say")]
        PreferNotToSay = 6,
    }
}
