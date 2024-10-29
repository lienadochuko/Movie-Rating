using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace Movie_Rating.Services
{
	public class SessionCheckBackgroundService : IHostedService, IDisposable
	{
		private readonly ISessionChecker _sessionChecker;
		private readonly ILogger<SessionCheckBackgroundService> _logger;
		private Timer? _timer;

		public SessionCheckBackgroundService(ISessionChecker sessionChecker, ILogger<SessionCheckBackgroundService> logger)
		{
			_sessionChecker = sessionChecker;
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			Console.WriteLine("Sesion checker");
			_logger.LogInformation("SessionCheckService started.");
			_timer = new Timer(CheckSession, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
			return Task.CompletedTask;
		}

		private async void CheckSession(object? state)
		{
			try
			{
				Console.WriteLine("Sesion checking");
				_logger.LogInformation("Checking session timeout...");
				_sessionChecker.CheckSessionTimeout();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Sesion failed");
				_logger.LogError(ex, "Error occurred while checking session timeout.");
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			Console.WriteLine("Sesion stopped");
			_logger.LogInformation("SessionCheckService stopped.");
			_timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}

	//public class SessionCheckBackgroundService : BackgroundService
	//{
	//	private readonly IHttpContextAccessor _httpContextAccessor;
	//	private readonly ILogger<SessionCheckBackgroundService> _logger;
	//	private Timer? _timer;

	//	public SessionCheckBackgroundService(IHttpContextAccessor httpContextAccessor, ILogger<SessionCheckBackgroundService> logger)
	//	{
	//		_httpContextAccessor = httpContextAccessor;
	//		_logger = logger;
	//	}

	//	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	//	{
	//		// Do not start the timer here, it will be triggered after login
	//		return Task.CompletedTask;
	//	}

	//	public void StartSessionCheck()
	//	{
	//		// Start a timer that checks session every 5 minutes
	//		_timer = new Timer(CheckSession, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
	//		Console.WriteLine("Session check started.");
	//		_logger.LogInformation("Session check started.");
	//	}

	//	private void CheckSession(object? state)
	//	{
	//		var context = _httpContextAccessor.HttpContext;
	//		if (context == null) return;

	//		if (context.Request.Cookies.TryGetValue("LoginTime", out var loginTimeCookie))
	//		{
	//			if (DateTime.TryParse(loginTimeCookie, out var loginTime))
	//			{
	//				var currentTime = DateTime.UtcNow;
	//				var sessionDuration = currentTime - loginTime;

	//				if (sessionDuration.TotalMinutes >= 30)
	//				{
	//					// Logout the user and clear the cookie
	//					context.SignOutAsync();
	//					context.Response.Cookies.Delete("LoginTime");

	//					Console.WriteLine("Session expired.");
	//					_logger.LogInformation("Session expired, user logged out.");
	//					_timer?.Dispose(); // Stop the timer once session expires
	//				}
	//				else
	//				{
	//					Console.WriteLine("Session checked.");
	//					_logger.LogInformation($"Session valid, {30 - sessionDuration.TotalMinutes:N0} minutes left.");
	//				}
	//			}
	//		}
	//	}

	//	public override Task StopAsync(CancellationToken stoppingToken)
	//	{
	//		Console.WriteLine("Session check service stopping..");
	//		_logger.LogInformation("Session check service stopping.");
	//		_timer?.Dispose(); // Stop the timer when service stops
	//		return base.StopAsync(stoppingToken);
	//	}

	//	public override void Dispose()
	//	{
	//		_timer?.Dispose();
	//		base.Dispose();
	//	}
	//}
}
