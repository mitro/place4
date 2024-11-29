namespace Place4.TelegramBot.Tests.UpdateHandlers
{
    using FakeItEasy;
    using Microsoft.Extensions.Logging;
    using Telegram.Bot;
    using Telegram.Bot.Requests;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using TelegramBot.UpdateHandlers;

    public class NewUserUpdateHandlerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task WhenNewChatMemberIsReceived_SendsCorrectReply()
        {
            // Arrange
            var logger = A.Fake<ILogger<NewUserUpdateHandler>>();
            var newUserUpdateHandler = new NewUserUpdateHandler(logger);
            var telegramBotClient = A.Fake<ITelegramBotClient>();
            var update = new Update
            {
                ChatMember = new ChatMemberUpdated
                {
                    NewChatMember = new ChatMemberMember
                    {
                        User = new User
                        {
                            Id = 123,
                            FirstName = "John", 
                            LastName = "Doe"
                        }
                    },
                    Chat = new Chat
                    {
                        Id = 456
                    }
                }
            };

            // Act
            await newUserUpdateHandler.HandleUpdateAsync(telegramBotClient, update, CancellationToken.None);

            // Assert
            A.CallTo(() => telegramBotClient.SendRequest(
                A<SendMessageRequest>.That.Matches(
                    x => x.ChatId == 456
                         && x.Text == $"Welcome, [John Doe](tg://user?id={123})"
                         && x.ParseMode == ParseMode.Markdown
                ),
                A<CancellationToken>.Ignored
            )).MustHaveHappenedOnceExactly();
        }


        [Test]
        public async Task WhenNonMessageUpdateIsReceived_DoesntDoAnything()
        {
            // Arrange
            var logger = A.Fake<ILogger<NewUserUpdateHandler>>();
            var parrotUpdateHandler = new NewUserUpdateHandler(logger);
            var telegramBotClient = A.Fake<ITelegramBotClient>();
            var update = new Update
            {
                Message = new Message
                {
                    Text = "Hi!",
                    From = new User
                    {
                        Id = 123,
                        FirstName = "John",
                        LastName = "Doe"
                    },
                    Chat = new Chat
                    {
                        Id = 456
                    }
                }
            };

            // Act
            await parrotUpdateHandler.HandleUpdateAsync(telegramBotClient, update, CancellationToken.None);

            // Assert
            A.CallTo(() => telegramBotClient.SendRequest(
                A<SendMessageRequest>.Ignored,
                A<CancellationToken>.Ignored
            )).MustNotHaveHappened();
        }
    }
}