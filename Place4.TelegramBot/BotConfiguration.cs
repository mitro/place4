namespace Place4.TelegramBot;

public class BotConfiguration
{
    public required string BotToken { get; init; }
    public required string LogChatId { get; init; }
    public int LogChatMessageThreadId { get; init; }
}