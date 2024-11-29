namespace Place4.TelegramBot.UpdateHandlers;

using Telegram.Bot;
using Telegram.Bot.Types;

public interface ITelegramUpdateHandler
{
    Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken);
}