namespace Place4.TelegramBot.Logging;

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
        _telegramBotClient = telegramBotClient;
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new TelegramLogger(name, _telegramBotClient, GetCurrentConfig()));

    private BotConfiguration GetCurrentConfig() => _currentConfig;

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }
}