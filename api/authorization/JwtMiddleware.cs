namespace api.authorization;

using api.services;

public class jwtMiddleware
{
    private readonly RequestDelegate _next;

    public jwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.validateJwtToken(token);
        if (userId != null)
        {
            context.Items["user"] = userId;
        }

        await _next(context);
    }
}