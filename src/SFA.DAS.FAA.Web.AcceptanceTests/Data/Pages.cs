namespace SFA.DAS.FAA.Web.AcceptanceTests.Data;

public static class Pages
{
    public static List<Page> GetPages()
    {
        return
        [
            //Create Account
            new Page { Name = "Create Account", Url= "/create-account"},
            new Page { Name = "Transfer Your Data", Url = "/create-account/transfer-your-data" },
            new Page { Name = "Sign into your Old Account", Url = "/create-account/sign-in-to-your-old-account" },
            new Page { Name = "User Name", Url = "/user-name" },
            new Page { Name = "User Date of Birth", Url = "/date-of-birth" },
            new Page { Name = "User Address - Postcode", Url = "/postcode-address"},
            new Page { Name = "User Address - Select", Url = "/select-address" },
            //Vacancies
            new Page { Name = "Vacancy Details", Url= "/apprenticeship/{vacancyReference}" },
            //Applications
            new Page { Name = "Applications", Url = "/applications" },
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
            new Page { Name = "Disability Confident Confirmation", Url = "/apply/{applicationId}/disability-confident/confirm" },
            //Qualifications
            new Page { Name = "Qualifications", Url="/apply/{applicationId}/qualifications"},
            new Page { Name = "Add Qualification Select Type", Url="/apply/{applicationId}/qualifications/add/select-type"},
            new Page { Name = "Delete Qualifications (single)", Url = "/apply/{applicationId}/qualifications/delete/2cb6af9b-77f9-4f47-af64-253a8bcc87bb" },
            new Page { Name = "Delete Qualifications (multiple)", Url = "/apply/{applicationId}/qualifications/delete/20d1923f-25b4-4a37-8580-d04643cf1fba" },
            //Create Account
            new Page { Name = "change email", Url="/email"},
            //Saved Search
            new Page { Name = "Saved Searches", Url="/saved-searches"},
            new Page { Name = "Saved Search Unsubscribe", Url="/saved-searches/unsubscribe"},
            new Page { Name = "Saved Search Unsubscribe Complete", Url="/saved-searches/unsubscribe-complete?Id=00ea277e-bf45-4110-b7a2-7aa5a32f31b7"},
            //Search
            new Page { Name = "Search Results", Url="/apprenticeships" },
            new Page { Name = "Location", Url="/location" },
            //Start
            new Page { Name = "Home", Url="/apprenticeshipsearch" }
        ];
    }

    public class Page
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}