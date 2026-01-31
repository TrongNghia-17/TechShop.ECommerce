namespace TechShop.ECommerce.Infrastructure.Logging;

public class LoggerAdapter<T>(ILogger<T> logger) : IAppLogger<T>
{
    private readonly ILogger<T> _logger = logger;

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }
}
