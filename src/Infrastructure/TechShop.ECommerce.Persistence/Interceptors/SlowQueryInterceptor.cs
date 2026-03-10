using Microsoft.Extensions.Logging;

namespace TechShop.ECommerce.Persistence.Interceptors;

public sealed class SlowQueryInterceptor(
    ILogger<SlowQueryInterceptor> logger)
    : DbCommandInterceptor
{
    private readonly TimeSpan _threshold = TimeSpan.FromMilliseconds(500);

    // =========================
    // SELECT queries
    // =========================
    public override async ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData);
        return await base.ReaderExecutedAsync(
            command,
            eventData,
            result,
            cancellationToken);
    }

    // =========================
    // INSERT / UPDATE / DELETE
    // =========================
    public override async ValueTask<int> NonQueryExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData);
        return await base.NonQueryExecutedAsync(
            command,
            eventData,
            result,
            cancellationToken);
    }

    // =========================
    // COUNT / SUM / MAX ...
    // =========================
    public override async ValueTask<object?> ScalarExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        object? result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData);
        return await base.ScalarExecutedAsync(
            command,
            eventData,
            result,
            cancellationToken);
    }

    // =========================
    // Common logging logic
    // =========================
    private void LogIfSlow(
        DbCommand command,
        CommandExecutedEventData eventData)
    {
        if (eventData.Duration <= _threshold)
            return;

        logger.LogWarning(
            """
            Slow Query Detected
            Duration: {Duration} ms
            Command: {CommandText}
            """,
            eventData.Duration.TotalMilliseconds,
            command.CommandText);
    }
}