namespace SFA.DAS.FAA.Web.AcceptanceTests.Data;

public static class Responses
{
    public static List<Response> GetResponses()
    {
        return
        [
            new Response { Name = "successfully returned", StatusCode= 200 },
            new Response { Name = "redirected", StatusCode= 302 },
        ];
    }

    public class Response
    {
        public string Name { get; set; }
        public int StatusCode { get; set; }
    }
}