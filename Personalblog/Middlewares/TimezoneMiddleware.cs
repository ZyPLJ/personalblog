using System.Globalization;

namespace Personalblog.Middlewares;

public class TimezoneMiddleware
{
    private readonly RequestDelegate _next;

    public TimezoneMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var timezone = context.Session.GetString("timezone");
        if (!string.IsNullOrEmpty(timezone))
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            if (timeZoneInfo != null)
            {
                DateTime dateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                DateTime convertedTime = TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo);
                CultureInfo.CurrentCulture = new CultureInfo("en-US");
                CultureInfo.CurrentUICulture = new CultureInfo("en-US");
                context.Items["LocalTime"] = convertedTime;
            }
        }

        await _next(context);
    }
}