using Projects;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Place4_TelegramBot>("bot", launchProfileName: "https");

builder.Build().Run();