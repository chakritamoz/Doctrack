using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Doctrack.Authentication
{
  public class AuthenticationFilter : ActionFilterAttribute
  {
    public override void OnActionExecuted(ActionExecutedContext context)
    {
      if (context.HttpContext.Session.GetString("IsAuthenticated") != "true")
      {
        context.Result = new RedirectToActionResult("Login", "Accounts", null);
      }
      else
      {
        ((Controller)context.Controller).ViewBag.currentUser = 
          context.HttpContext.Session.GetString("Username");
      }

      base.OnActionExecuted(context);
    }
  }

  public class AuthenticationPrivilege : ActionFilterAttribute
  {
    public override void OnActionExecuted(ActionExecutedContext context)
    {
      if (context.HttpContext.Session.GetString("Role") != "Administrator")
      {
        ((Controller)context.Controller).ViewBag.isAdmin = false;
      }
      else
      {
        ((Controller)context.Controller).ViewBag.isAdmin = true;
      }

      base.OnActionExecuted(context);
    }
  }

    public class AuthenticationProtect : ActionFilterAttribute
  {
    public override void OnActionExecuted(ActionExecutedContext context)
    {
      if (context.HttpContext.Session.GetString("Role") != "Administrator")
      {
        context.Result = new RedirectToActionResult("Index", null, null);
      }

      base.OnActionExecuted(context);
    }
  }
}