using System.Text.RegularExpressions;
using System.Web;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Steps;

[Binding]
public sealed class SavedSearchSteps(ScenarioContext context)
{
    //[When("I save my search")]
    //public async Task WhenISaveMySearch()
    //{
    //    Console.WriteLine("DEBUG: Entering WhenISaveMySearch() step."); // DEBUG

    //    var content = context.Get<string>(ContextKeys.HttpResponseContent);

    //    if (context.Keys.Contains(ContextKeys.HttpResponseContent)) // DEBUG
    //    {
    //        content = context.Get<string>(ContextKeys.HttpResponseContent);
    //        Console.WriteLine($"DEBUG: Retrieved previous HTTP response content (first 500 chars): {content.Substring(0, Math.Min(content.Length, 500))}");
    //    }
    //    else
    //    {
    //        Console.WriteLine("ERROR: ContextKeys.HttpResponseContent not found in ScenarioContext.");
    //        Assert.Fail("Previous HTTP response content is missing from context. Cannot proceed with saving search.");
    //        return; // Exit if critical data is missing
    //    } // DEBUG

    //    var data = Regex.Match(content, "<input name=\"Data\" type=\"hidden\" value=\"(?<val>.*)\"");
    //    if (data is { Captures.Count: 1 })
    //    {
    //        var value = HttpUtility.HtmlDecode(data.Groups["val"].Value);

    //        Console.WriteLine($"DEBUG: Successfully extracted hidden 'Data' field value: '{value}'"); // DEBUG

    //        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);
    //        var formData = new MultipartFormDataContent()
    //        {
    //            { new StringContent(value), "Data" },
    //        };

    //        Console.WriteLine($"DEBUG: Preparing MultipartFormDataContent with 'Data': '{value}'"); // DEBUG

    //        Console.WriteLine($"DEBUG: Sending POST request to: apprenticeships/save-search");
    //        Console.WriteLine($"DEBUG: Content-Type: application/x-www-form-urlencoded (assuming Dictionary<string, string>)");


    //        var response = await client.PostAsync("apprenticeships/save-search", new Dictionary<string, string>{{"Data", value}});
    //        Console.WriteLine($"DEBUG: Received HTTP Response Status Code: {(int)response.StatusCode} {response.StatusCode}"); // DEBUG
    //        var responseContent = await response.Content.ReadAsStringAsync();
    //        Console.WriteLine($"DEBUG: Received HTTP Response Body (first 500 chars): {responseContent.Substring(0, Math.Min(responseContent.Length, 500))}"); // DEBUG
    //        context.ClearResponseContext();
    //        context.Set(response, ContextKeys.HttpResponse);
    //        Console.WriteLine("DEBUG: Exiting WhenISaveMySearch() step."); // DEBUG
    //    }
    //    else
    //    {
    //        Console.WriteLine($"ERROR: Failed to extract 'Data' field from response content. Regex match success: {data.Success}, Group 'val' success: {data.Groups["val"].Success}."); // DEBUG
    //        Console.WriteLine($"DEBUG: Content searched (first 500 chars): {content.Substring(0, Math.Min(content.Length, 500))}"); // DEBUG

    //        Assert.Fail("The response does not contain the required encoded search data");
    //    }
    //}

    [When("I save my search")]
    public async Task WhenISaveMySearch()
    {
        Console.WriteLine("DEBUG: Entering WhenISaveMySearch() step.");
        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);

        string content;
        if (context.Keys.Contains(ContextKeys.HttpResponseContent))
        {
            content = context.Get<string>(ContextKeys.HttpResponseContent);
            Console.WriteLine($"DEBUG: Retrieved previous HTTP response content (first 500 chars): {content.Substring(0, Math.Min(content.Length, 500))}");
        }
        else
        {
            Console.WriteLine("ERROR: ContextKeys.HttpResponseContent not found in ScenarioContext. Cannot proceed with saving search.");
            Assert.Fail("Previous HTTP response content is missing from context. Cannot proceed with saving search.");
            return;
        }

        // Debugging Step: Save the raw HTML content to a file
        // This will allow you to open the file and manually inspect its contents.
        string debugFilePath = "c:\\temp\\last_page_html.html"; // Choose a suitable path
        System.IO.File.WriteAllText(debugFilePath, content);
        Console.WriteLine($"DEBUG: Full HTML content saved to: {debugFilePath}");

        // --- Step 2: Extract Anti-Forgery Token ---
        string antiForgeryToken = "";
        var antiForgeryTokenMatch = Regex.Match(content, "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(?<val>.*)\"");
        if (antiForgeryTokenMatch.Success && antiForgeryTokenMatch.Groups["val"].Success)
        {
            antiForgeryToken = HttpUtility.HtmlDecode(antiForgeryTokenMatch.Groups["val"].Value);
            Console.WriteLine($"DEBUG: Successfully extracted Anti-Forgery Token (first 20 chars): {antiForgeryToken.Substring(0, Math.Min(antiForgeryToken.Length, 20))}...");
        }
        else
        {
            Console.WriteLine("ERROR: Anti-Forgery Token (__RequestVerificationToken) not found in response content. This is required for POST.");
            Console.WriteLine($"DEBUG: Content where token was searched (first 500 chars): {content.Substring(0, Math.Min(content.Length, 500))}");
            Assert.Fail("Anti-Forgery Token missing from form. Cannot submit search."); // Now make this a hard fail if not found
            return;
        }


        // --- Step 3: Extract the 'Data' field ---
        string searchDataValue = "";
        var dataMatch = Regex.Match(content, "<input name=\"Data\" type=\"hidden\" value=\"(?<val>.*)\"");
        if (dataMatch.Success && dataMatch.Groups["val"].Success)
        {
            searchDataValue = HttpUtility.HtmlDecode(dataMatch.Groups["val"].Value);
            Console.WriteLine($"DEBUG: Successfully extracted hidden 'Data' field value (first 20 chars): {searchDataValue.Substring(0, Math.Min(searchDataValue.Length, 20))}...");
        }
        else
        {
            Console.WriteLine("ERROR: Expected hidden 'Data' field not found in response content. Cannot proceed with saving search.");
            Assert.Fail("The response does not contain the required encoded search data (hidden input 'Data').");
            return;
        }

        // --- Step 4: Prepare the form data for POST - INCLUDE BOTH FIELDS ---
        var formData = new Dictionary<string, string>
        {
            { "Data", searchDataValue }, // This contains the serialized search criteria
            { "__RequestVerificationToken", antiForgeryToken } // This is the standard CSRF token
        };

        Console.WriteLine($"DEBUG: Final POST form data being sent:");
        foreach (var item in formData)
        {
            Console.WriteLine($"  - {item.Key}: {item.Value.Substring(0, Math.Min(item.Value.Length, 50))}...");
        }

        // --- Step 5: Send the POST request ---
        string requestUri = "apprenticeships/save-search";
        Console.WriteLine($"DEBUG: Sending POST request to: '{requestUri}' (via ITestHttpClient.PostAsync(string, Dictionary<string, string>))");

        HttpResponseMessage response = await client.PostAsync(requestUri, formData);


        // --- Step 6: Process and log the response ---
        Console.WriteLine($"DEBUG: Received HTTP Response Status Code: {(int)response.StatusCode} {response.StatusCode}");
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"DEBUG: Received HTTP Response Body (first 500 chars): {responseContent.Substring(0, Math.Min(responseContent.Length, 500))}");

        // --- Step 7: Update scenario context ---
        context.ClearResponseContext();
        context.Set(response, ContextKeys.HttpResponse);
        context.Set(responseContent, ContextKeys.HttpResponseContent);

        Console.WriteLine("DEBUG: Exiting WhenISaveMySearch() step.");
    }
}