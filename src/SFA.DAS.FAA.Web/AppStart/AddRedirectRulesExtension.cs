using System.Net;
using Microsoft.AspNetCore.Rewrite;

namespace SFA.DAS.FAA.Web.AppStart;

public static class AddRedirectRulesExtension
{
    public static void AddRedirectRules(this IApplicationBuilder app)
    {
        var options = new RewriteOptions();
        //Applications
        options.AddRedirect("(?i)myapplications/(.*)", "applications", (int)HttpStatusCode.PermanentRedirect);
        options.AddRedirect("(?i)myapplications", "applications", (int)HttpStatusCode.PermanentRedirect);
        options.AddRedirect("(?i)apprenticeship/apply/(.*)", "applications", (int)HttpStatusCode.PermanentRedirect);
        options.AddRedirect("(?i)apprenticeship/whatnext/(.*)", "applications", (int)HttpStatusCode.PermanentRedirect);
            
        //User settings
        options.AddRedirect("(?i)deleteaccount", "settings", (int)HttpStatusCode.PermanentRedirect);
        options.AddRedirect("(?i)savedsearches", "settings", (int)HttpStatusCode.PermanentRedirect);
        
        //Feedback/contact us
        options.AddRedirect("(?i)feedback", "apprenticeshipsearch", (int)HttpStatusCode.PermanentRedirect);
        options.AddRedirect("(?i)apprenticeship/reportthisvacancy/(.*)", "apprenticeshipsearch", (int)HttpStatusCode.PermanentRedirect);
        
        //Account
        options.AddRedirect("(?i)register", "create-account", (int)HttpStatusCode.PermanentRedirect);

        app.UseRewriter(options);
    }
}