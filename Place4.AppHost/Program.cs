var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.Place4_TelegramBot>("bot", launchProfileName: "https");

builder.Build().Run();