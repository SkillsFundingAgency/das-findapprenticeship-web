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
            new Page { Name = "Edit a Training Course", Url = "/apply/{applicationId}/trainingcourses/d96b7cbd-fee2-4084-ae69-b138959edce4" },
            new Page { Name = "Delete a Training Course", Url = "/apply/{applicationId}/trainingcourses/d96b7cbd-fee2-4084-ae69-b138959edce4/delete" },
            //Volunteering and Work Experience
            new Page { Name = "Volunteering and Work Experience", Url="/apply/{applicationId}/volunteering-and-work-experience" },
            new Page { Name = "Volunteering and Work Experience Summary", Url = "/apply/{applicationId}/volunteering-and-work-experience/summary" },
            new Page { Name = "Add Work Experience", Url="/apply/{applicationId}/volunteering-and-work-experience/add" },
            new Page { Name = "Delete Work Experience", Url= "/apply/{applicationId}/volunteering-and-work-experience/3584e247-ae13-46b0-bca3-55ff9ba096ef/delete" },
            //Disability Confident
            new Page { Name = "Disability Confident", Url = "/apply/{applicationId}/disability-confident" },
            new Page { Name = "Disability Confident Confirmation", Url = "/apply/{applicationId}/disability-confident/summary" }
        ];
    }

    public class Page
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}