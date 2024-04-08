using System.Text;
using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Maid;

public static partial class EventHandlers
{
	private static Regex issueRegex = IssueRegex();
	private static Regex channelConfigRegex = ChannelConfigRegex();

	private static Dictionary<string, string> repositories = new()
	{
		["lt"] = "https://github.com/lighttube-org/LightTube",
		["lta"] = "https://github.com/lighttube-org/LightTube-Android",
		["ltp"] = "https://github.com/lighttube-org/LTPlayer",
		["it"] = "https://github.com/lighttube-org/InnerTube",

		["deg"] = "https://github.com/kuylar/discord-embedded-godot",
		["godotcord-sdk"] = "https://github.com/kuylar/discord-embedded-godot",
		["godotcord"] = "https://github.com/kuylar/discord-embedded-godot",
		["that one library that has a stupid name"] = "https://github.com/kuylar/discord-embedded-godot",

		["cr"] =
			"CurseRinth is not supported anymore, and bugs will not be fixed. Here you go, though: https://github.com/kuylar/curserinth",
	};

	public static async Task MessageCreated(DiscordClient sender, MessageCreateEventArgs args)
	{
		if (args.Message.Author.IsBot) return;

		// check if an issue ref
		MatchCollection issueRefMatches = issueRegex.Matches(args.Message.Content);
		if (issueRefMatches.Count > 0)
		{
			StringBuilder message = new();
			foreach (Match match in issueRefMatches)
			{
				string repo = match.Groups[1].Value;
				string issue = match.Groups[2].Value;
				if (string.IsNullOrEmpty(repo))
				{
					Match channelConfigMatch = channelConfigRegex.Match(args.Message.Channel.Topic);
					string[] config = channelConfigMatch.Groups[1].Value.Split(":");

					if (config.Length == 0)
						// Channel doesnt have a config, skip
						continue;

					if (string.IsNullOrEmpty(config[0]))
						// Channel doesnt specify a default repo, skip
						continue;

					repo = config[0];
				}

				if (repositories.TryGetValue(repo, out string? url))
					message.AppendLine($"{url}/issues/{issue}");
				else
					message.AppendLine($"Unknown project: `{repo}`");
			}

			await args.Channel.SendMessageAsync(message.ToString());
		}
	}

	[GeneratedRegex(@"(\w+)?##?(\d+)")]
	private static partial Regex IssueRegex();

	[GeneratedRegex(@"\|\|<@\d+>:(.+?)\|\|")]
	private static partial Regex ChannelConfigRegex();
}