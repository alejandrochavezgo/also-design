namespace app.authorization;

using entities.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

public sealed class authorizationAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            if (context == null)
            {
                context!.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "error"},
                    {"action", "errorWithParams"},
                    {"errorCode", "500"},
                    {"errorMessage", "No context."}
                });
                return;
            }

            var userCookie = context.HttpContext.Request.Cookies["userCookie"];
            if (userCookie == null)
            {
                context!.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "login"},
                    {"action", "login"}
                });
                return;
            }

            var currentPath = context.HttpContext.Request.Path.HasValue ? context.HttpContext.Request.Path.Value : string.Empty;
            if (string.IsNullOrEmpty(currentPath))
            {
                context!.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "error"},
                    {"action", "errorWithParams"},
                    {"errorCode", "400"},
                    {"errorMessage", "No path provider."}
                });
                return;
            }

            if (currentPath == "/login/login" || currentPath == "/")
            {
                context!.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "dashboard"},
                    {"action", "dashboard"}
                });
                return;
            }

            var userInfo = JsonConvert.DeserializeObject<userCookieModel>(userCookie);
            if (!userInfo!.menus!.Any(menu => menu.path!.Trim().ToUpper().Split('/')[1] == currentPath.Trim().ToUpper().Split('/')[1]))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "error"},
                    {"action", "errorWithParams"},
                    {"errorCode", "403"},
                    {"errorMessage", "You do not have permission to access this page."}
                });
                return;
            }

            return;
        }
        catch (Exception exception)
        {
            context!.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "error"},
                    {"action", "errorWithParams"},
                    {"errorCode", "500"},
                    {"errorMessage", $"{exception.Message}"}
            });
            return;
        }
    }
}