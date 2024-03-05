// See https://aka.ms/new-console-template for more information

using SFA.DAS.FAA.MockServer.MockServerBuilder;

Console.WriteLine("Mock Server starting on http://localhost:5027");

MockApiServer.Start();

Console.WriteLine(("Press any key to stop the server"));
Console.ReadKey();