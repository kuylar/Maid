using DSharpPlus;
using Maid;

DiscordClient client = new(new DiscordConfiguration
{
	Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN") ?? throw new Exception("set env var DISCORD_TOKEN"),
	Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents | DiscordIntents.GuildMessages |
	          DiscordIntents.GuildMembers
});

client.SessionCreated += (sender, _) =>
{
	Console.WriteLine($"Connected as {sender.CurrentUser.Username}#{sender.CurrentUser.Discriminator}");
	return Task.CompletedTask;
};
client.MessageCreated += EventHandlers.MessageCreated;
client.GuildMemberAdded += EventHandlers.GuildMemberAdded;


await client.ConnectAsync();
await Task.Delay(-1);