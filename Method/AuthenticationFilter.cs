using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Doctrack.Authentication
{
  public class AuthenticationFilter : ActionFilterAttribute
  {
    public override void OnActionExecuted(ActionExecutedContext context)
    {
      if(context.HttpContext.Session.GetString("IsAuthenticated") != "true")
      {
        context.Result = new RedirectToActionResult("Login", "Accounts", null);
      }
      base.OnActionExecuted(context);
    }
  }
}