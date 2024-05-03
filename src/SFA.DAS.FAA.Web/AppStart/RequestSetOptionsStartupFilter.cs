namespace SFA.DAS.FAA.Web.AppStart
{
    public class RequestSetOptionsStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> app)
        {
            return builder =>
            {
                builder.UseStatusCodePagesWithReExecute("/error/{0}");
                builder.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response is { StatusCode: 404 or 500, HasStarted: false })
                    {
                        //Re-execute the request so the user gets the error page
                        var originalPath = context.Request.Path.Value;
                        context.Items["originalPath"] = originalPath;
                        context.Request.Path = $"/error/{context.Response.StatusCode}";
                        await next();
                    }
                });
                app(builder);
            };
        }
    }
}