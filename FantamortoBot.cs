using System;
using System.IO;
using System.Collections.Generic;

using Telegram.Bot;
using Telegram.Bot.Args;

using Fantamorto_Bot;

namespace FantamortoBot
{
	class FantamortoBot
	{
		public static readonly string filePath = @"lists\";

		static ITelegramBotClient botClient;

		public FantamortoBot(string accessToken)
		{
			botClient = new TelegramBotClient(accessToken);
			botClient.OnMessage += Bot_OnMessage;

			Directory.CreateDirectory(filePath);
		}

		public void Start()
		{
			botClient.StartReceiving();
		}

		public void Stop()
		{
			botClient.StopReceiving();
		}

		private static async void Bot_OnMessage(object sender, MessageEventArgs e)
		{
			if (e.Message.Text != null)
			{
				Console.WriteLine($"Received message: {e.Message.Text} from user: {e.Message.From.FirstName}");

				var ch = new CommandHandler(filePath, e.Message);
				var commandsMap = new Dictionary<string, Action>(StringComparer.InvariantCultureIgnoreCase)
				{
					["/new"] = ch.NewList,
					["/list"] = ch.PrintList,
					["/add"] = ch.AddName,
					["/remove"] = ch.RemoveName,
					["/dead"] = ch.AddDead
				};

				var command = e.Message.Text.Split(" ")[0];

				if (commandsMap.ContainsKey(command))
				{
					commandsMap[command]();
					await botClient.SendTextMessageAsync(e.Message.Chat, ch.GetResponse());
				}
			}
		}
	}
}
