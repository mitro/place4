namespace Place4.TelegramBot.UpdateHandlers;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class NewUserUpdateHandler(ILogger<NewUserUpdateHandler> logger) : ITelegramUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
    {
        var newUserUpdate = IsNewUserUpdate(update);
        if (newUserUpdate.chat == null || newUserUpdate.user == null)
        {
            return;
        }

        var chatId = newUserUpdate.chat.Id;
        var message = $"Welcome, [{newUserUpdate.user.FirstName} {newUserUpdate.user.LastName}](tg://user?id={newUserUpdate.user.Id})";
        await telegramBotClient.SendMessage(
            chatId, 
            message, 
            cancellationToken: cancellationToken,
            parseMode: ParseMode.Markdown);
    }
        
    private static (Chat? chat, User? user) IsNewUserUpdate(Update update)
    {
        var isFakeNewMember = update is { Type: UpdateType.Message, Message.Text: "I am new" }; // for test only
        var isNewMember = update is { Type: UpdateType.ChatMember, ChatMember.NewChatMember: not null };
        if (isNewMember)
        {
            return (update.ChatMember?.Chat, update.ChatMember?.NewChatMember?.User);
        }

        return isFakeNewMember ? (update.Message?.Chat, update.Message?.From) : (null, null);
    }
}