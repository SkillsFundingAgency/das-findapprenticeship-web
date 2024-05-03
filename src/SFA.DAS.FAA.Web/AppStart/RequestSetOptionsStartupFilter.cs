namespace SFA.DAS.FAA.Web.AppStart
{
    public class RequestSetOptionsStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseStatusCodePagesWithReExecute("/error/{0}");
                builder.Use(async (context, middleware) =>
                {
                    await middleware();
                    if (context.Response is { StatusCode: 404 or 500, HasStarted: false })
                    {
                        //Re-execute the request so the user gets the error page
                        var originalPath = context.Request.Path.Value;
                        context.Items["originalPath"] = originalPath;
                        context.Request.Path = $"/error/{context.Response.StatusCode}";
                        await middleware();
                    }
                });
                next(builder);
            };
        }
    }
}