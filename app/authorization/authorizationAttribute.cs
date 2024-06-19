namespace app.authorization;

using System.Text.RegularExpressions;
using entities.models;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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
                    {"action", "error"},
                    {"errorMessage", "No context"}
                });
                return;
            }

            var userCookie = context.HttpContext.Request.Cookies["userCookie"];
            if (userCookie == null)
            {
                context!.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "error"},
                    {"action", "error"},
                    {"errorMessage", "No cookie"}
                });
                return;
            }

            var currentPath = context.HttpContext.Request.Path.HasValue ? context.HttpContext.Request.Path.Value : string.Empty;
            if (string.IsNullOrEmpty(currentPath))
            {
                context!.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "error"},
                    {"action", "error"},
                    {"errorMessage", "No path provider"}
                });
                return;
            }

            if (currentPath == "/login/login" || currentPath == "/")
            {
                context!.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "login"},
                    {"action", "logged"},
                    {"errorMessage", "Logged"}
                });
                return;
            }

            return;
        }
        catch (Exception exception)
        {
            context!.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "error"},
                    {"action", "exception"},
                    {"errorMessage", $"{exception.Message}"}
            });
            return;
        }
    }
}