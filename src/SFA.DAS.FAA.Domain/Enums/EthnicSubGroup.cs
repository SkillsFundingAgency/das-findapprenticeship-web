using System.ComponentModel;

namespace SFA.DAS.FAA.Domain.Enums
{
    public enum EthnicSubGroup : short
    {
        [Description("English, Welsh, Scottish, Northern Irish or British")]
        EnglishOrWelshOrScottishOrNorthernIrishOrBritish = 1,
        [Description("Irish")]
        Irish = 2,
        [Description("Gypsy or Irish Traveller")]
        GypsyOrIrishTraveller = 3,
        [Description("Any other White background")]
        AnyOtherWhiteBackground = 4,
        [Description("Prefer not to say")]
        PreferNotToSayWhite = 5,
        [Description("White and Black Caribbean")]
        WhiteAndBlackCaribbean = 6,
        [Description("White and Black African")]
        WhiteAndBlackAfrican = 7,
        [Description("White and Asian")]
        WhiteAndAsian = 8,
        [Description("Any other mixed or multiple ethnic background")]
        AnyOtherMixedBackground = 9,
        [Description("Prefer not to say")]
        PreferNotToSayMixed = 10,
        [Description("Indian")]
        Indian = 11,
        [Description("Pakistani")]
        Pakistani = 12,
        [Description("Bangladeshi")]
        Bangladeshi = 13,
        [Description("Chinese")]
        Chinese = 14,
        [Description("Any other Asian background")]
        AnyOtherAsianBackground = 15,
        [Description("Prefer not to say")]
        PreferNotToSayAsian = 16,
        [Description("African")]
        African = 17,
        [Description("Caribbean")]
        Caribbean = 18,
        [Description("Any other Black, African or Caribbean background")]
        AnyOtherBlackAfricanOrCaribbeanBackground = 19,
        [Description("Prefer not to say")]
        PreferNotToSayBlack = 20,
        [Description("Arab")]
        Arab = 21,
        [Description("Any other ethnic group")]
        AnyOtherEthnicGroup = 22,
        [Description("Prefer not to say")]
        PreferNotToSayArab = 23
    }
}
