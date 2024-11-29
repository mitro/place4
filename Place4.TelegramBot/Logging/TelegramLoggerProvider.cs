namespace Place4.TelegramBot.Logging
{
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Options;
    using Telegram.Bot;

    public class TelegramLoggerProvider : ILoggerProvider
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IDisposable? _onChangeToken;
        private BotConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, TelegramLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);

        public TelegramLoggerProvider(
            IOptionsMonitor<BotConfiguration> config,
            ITelegramBotClient telegramBotClient)
        {
            this._telegramBotClient = telegramBotClient;
            this._currentConfig = config.CurrentValue;
            this._onChangeToken = config.OnChange(updatedConfig => this._currentConfig = updatedConfig);
        }

        public ILogger CreateLogger(string categoryName) =>
            this._loggers.GetOrAdd(categoryName, name => new TelegramLogger(name, this._telegramBotClient, this.GetCurrentConfig()));

        private BotConfiguration GetCurrentConfig() => this._currentConfig;

        public void Dispose()
        {
            this._loggers.Clear();
            this._onChangeToken?.Dispose();
        }
    }
}