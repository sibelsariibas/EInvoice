public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string HeaderKey = "X-Correlation-Id";
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(HeaderKey, out Microsoft.Extensions.Primitives.StringValues correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers.Append(HeaderKey, correlationId);
        }

        context.Request.EnableBuffering();
        string requestBody;
        using (StreamReader reader = new(context.Request.Body, leaveOpen: true))
        {
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        Console.WriteLine("---- Request ----");
        Console.WriteLine($"CorrelationId : {correlationId}");
        Console.WriteLine($"Method        : {context.Request.Method}");
        Console.WriteLine($"Path          : {context.Request.Path}");
        Console.WriteLine($"QueryString   : {context.Request.QueryString}");
        Console.WriteLine("------------------");

        Stream originalBody = context.Response.Body;
        await using MemoryStream responseBody = new();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                string errorJson = System.Text.Json.JsonSerializer.Serialize(new
                {
                    message = ex.Message,
                });

                await context.Response.WriteAsync(errorJson);
            }

            Console.WriteLine("---- EXCEPTION ----");
            Console.WriteLine(ex);
            Console.WriteLine("-------------------");

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await context.Response.Body.CopyToAsync(originalBody);

            return;
        }

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        string responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        Console.WriteLine("---- Response ----");
        Console.WriteLine($"CorrelationId : {correlationId}");
        Console.WriteLine($"Status Code   : {context.Response.StatusCode}");
        Console.WriteLine($"Response Body : {responseText}");
        Console.WriteLine("-------------------");

        await responseBody.CopyToAsync(originalBody);
    }
}
