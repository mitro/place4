namespace Place4.TelegramBot.UpdateHandlers
{
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class ParrotUpdateHandler(ILogger<ParrotUpdateHandler> logger) : ITelegramUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message || update.Message?.From == null)
            {
                return;
            }
            var chatId = update.Message.Chat.Id;
            var message = $"Hi, [{update.Message.From.FirstName} {update.Message.From.LastName}](tg://user?id={update.Message.From.Id}). You said: {update.Message.Text}";
            await botClient.SendMessage(
                chatId, 
                message, 
                cancellationToken: cancellationToken,
                messageThreadId: update.Message.MessageThreadId,
                parseMode: ParseMode.Markdown);
        }

    }
}