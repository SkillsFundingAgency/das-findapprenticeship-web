using System.ComponentModel;

namespace SFA.DAS.FAA.Domain.Enums
{
    public enum GenderIdentity : short
    {
        [Description("Female")]
        Female = 1,
        [Description("Male")]
        Male = 2,
        [Description("Prefer not to say")]
        PreferNotToSay = 3,
    }
}
