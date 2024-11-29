namespace Place4.TelegramBot.Logging
{
    using Telegram.Bot;

    public class TelegramLogger(string name, ITelegramBotClient telegramBotClient, BotConfiguration configuration) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState: notnull => default;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel > LogLevel.Information;
        }
        
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }
            telegramBotClient.SendMessage(
                configuration.LogChatId,
                formatter(state, exception),
                messageThreadId: configuration.LogChatMessageThreadId).GetAwaiter().GetResult();
        }
    }
}