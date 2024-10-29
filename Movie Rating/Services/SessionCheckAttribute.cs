using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Movie_Rating.Services;

public class SessionCheckFilter : IAsyncActionFilter
{
	private readonly ISessionChecker _sessionChecker;

	public SessionCheckFilter(ISessionChecker sessionChecker)
	{
		_sessionChecker = sessionChecker;
	}

	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var session = await _sessionChecker.CheckSession();
		if (session != null && session.status == "Time-Out")
		{
			context.Result = new RedirectToActionResult("Login", "Auth", null);
			return;
		}
		await next();
	}
}