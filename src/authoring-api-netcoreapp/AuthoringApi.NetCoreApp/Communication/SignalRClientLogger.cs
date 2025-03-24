using Microsoft.AspNetCore.SignalR;

namespace AuthoringApi.NetCoreApp.Communication
{
    public class SignalRClientLogger : IClientLogger
    {
        private readonly IHubContext<ClientProgressHub> _hubContext;
        private readonly ILogger<SignalRClientLogger> _logger;

        public SignalRClientLogger(IHubContext<ClientProgressHub> hubContext, ILogger<SignalRClientLogger> logger)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Info(string message)
        {
            LogMessage(message, "info");
            _logger.LogInformation(message);
        }

        public void Warn(string message)
        {
            LogMessage(message, "warn");
            _logger.LogWarning(message);
        }

        public void Error(string message)
        {
            LogMessage(message, "errr");
            _logger.LogError(message);
        }

        public void Debug(string message)
        {
            LogMessage(message, "dbug");
            _logger.LogDebug(message);
        }

        private void LogMessage(string message, string logLevel)
        {
            var logMessage = new
            {
                date = DateTime.Now.ToString("hh:mm:ss tt"),
                status = message,
                level = logLevel
            };
            _hubContext.Clients.All.SendAsync("ClientUpdate", logMessage);
        }
    }
}
