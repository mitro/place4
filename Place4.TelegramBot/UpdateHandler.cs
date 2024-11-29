namespace Place4.TelegramBot
{
    using Telegram.Bot;
    using Telegram.Bot.Polling;
    using Telegram.Bot.Types;
    using UpdateHandlers;

    public class UpdateHandler(IEnumerable<ITelegramUpdateHandler> updateHandlers, ILogger<UpdateHandler> logger) : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            foreach (var updateHandler in updateHandlers)
            {
                try
                {
                    await updateHandler.HandleUpdateAsync(botClient, update, cancellationToken);
                }
                catch (Exception ex)
                {
                    await this.HandleErrorAsync(botClient, ex, HandleErrorSource.HandleUpdateError, cancellationToken);
                }
            }
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Error occured in {Source}", source);
            return Task.CompletedTask;
        }
    }
}