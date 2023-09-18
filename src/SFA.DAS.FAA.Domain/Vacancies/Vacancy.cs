using Newtonsoft.Json;


namespace SFA.DAS.FAA.Domain.Vacancies
{
    public class Vacancy
    {
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
