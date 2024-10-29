using Microsoft.AspNetCore.Authentication;
using Movie_Rating.Models.DTO;

namespace Movie_Rating.Services
{
	public interface ISessionChecker
	{
		Task<session> CheckSessionTimeout();
		Task<session> CheckSession();
	}
	public class SessionChecker(ILogger<SessionChecker> logger, IHttpContextAccessor _httpContextAccessor) : ISessionChecker
	{
		public async Task<session> CheckSessionTimeout()
		{	
			session session = null;
			var context = _httpContextAccessor.HttpContext;
			if (context != null)
			{
				Console.WriteLine("Sesion checker");
				var loginTimeCookie = context.Request.Cookies["LoginTime"];
				if (!string.IsNullOrEmpty(loginTimeCookie))
				{
					var loginTime = DateTime.Parse(loginTimeCookie);
					if (DateTime.UtcNow.Subtract(loginTime).TotalMinutes >= 30)
					{
						logger.LogInformation($"User was logged out at {loginTime}");
						// Log out user
						await context.SignOutAsync();
						session = new()
						{
							loginTime = loginTime,
							status = "Session Time-Out"
						};

					}
				} else
				{
					var loginTime = DateTime.Parse(loginTimeCookie);
					session = new()
					{
						loginTime = loginTime,
						status = "Session still on"
					};
				}
			}

			return session;

		}
		public async Task<session> CheckSession()
		{	
			session session = null;
			var context = _httpContextAccessor.HttpContext;
			if (context != null)
			{
				Console.WriteLine("Sesion checker");
				var loginTimeCookie = context.Request.Cookies["LoginTime"];
				if (!string.IsNullOrEmpty(loginTimeCookie))
				{
					var loginTime = DateTime.Parse(loginTimeCookie);
					var totalTime = DateTime.UtcNow.Subtract(loginTime).TotalMinutes;

					if (totalTime >= 5)
					{
						Console.WriteLine($"User was logged out at {loginTime}");
						logger.LogInformation($"User was logged out at {loginTime}");
						// Log out user
						await context.SignOutAsync();
						session = new()
						{
							loginTime = loginTime,
							status = "Time-Out"
						};

					}
					if (totalTime < 5)
					{
						{
							Console.WriteLine("Token viable");
							session = new()
							{
								loginTime = loginTime,
								status = "!Time-Out"
							};
						}
					}
				}
			else
			{
				var loginTime = DateTime.UtcNow;
				Console.WriteLine($"User was logged out at {loginTime} due to null token");
				logger.LogInformation($"User was logged out at {loginTime} due to null token");
				// Log out user
				await context.SignOutAsync();
				session = new()
				{
					loginTime = loginTime,
					status = "Time-Out"
				};

				}
			}

			return session;
		}
	}
}
