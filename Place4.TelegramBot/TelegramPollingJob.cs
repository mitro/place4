namespace Place4.TelegramBot
{
    using Telegram.Bot;
    using Telegram.Bot.Polling;
    using Telegram.Bot.Types.Enums;

    public class TelegramPollingJob(
        ITelegramBotClient telegramBotClient,
        IUpdateHandler updateHandler,
        ILogger<TelegramPollingJob> logger) : 
        BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting polling service");
            await this.DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message, UpdateType.ChatMember],
                DropPendingUpdates = true
            };
            // Make sure we receive updates until Cancellation Requested
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await telegramBotClient.ReceiveAsync(updateHandler, receiverOptions, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError("Polling failed with exception: {Exception}", ex);
                    // Cooldown if something goes wrong
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }
}