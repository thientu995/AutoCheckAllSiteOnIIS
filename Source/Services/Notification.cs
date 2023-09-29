using Microsoft.Extensions.Logging;

interface INotification
{
    void WriteErrorLog(string message);
    void WriteInfoLog(string message);
}

class Notification : INotification
{
    readonly ILogger<Notification> logger;
    public Notification(ILogger<Notification> logger)
    {
        this.logger = logger;
    }

    public void WriteErrorLog(string message)
    {
        this.logger.Log(LogLevel.Error, message);
    }

    public void WriteInfoLog(string message)
    {
        this.logger.Log(LogLevel.Information, message);
    }
}
