namespace SFA.DAS.FAA.Web.AcceptanceTests.Data;

public static class Pages
{
    public static List<Page> GetPages()
    {
        return
        [
            //Vacancies
            new Page { Name = "Vacancy Details", Url= "/vacancies/{vacancyReference}" },
            //TaskList
            new Page { Name = "Application Tasklist", Url = "/applications/{applicationId}" },
            //Jobs
            new Page { Name = "Jobs", Url = "/apply/{applicationId}/jobs" },
            new Page { Name = "Add a Job", Url = "/apply/{applicationId}/jobs/add" },
            new Page { Name = "Edit Job", Url = "/apply/{applicationId}/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae" },
            new Page { Name = "Delete Job", Url = "/apply/{applicationId}/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae/delete" },
            //Training Courses
            new Page { Name = "Training Courses", Url = "/apply/{applicationId}/training-courses" },
            new Page { Name = "Add a Training Course", Url = "/apply/{applicationId}/trainingcourses/add" },
        ];
    }

    public class Page
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}