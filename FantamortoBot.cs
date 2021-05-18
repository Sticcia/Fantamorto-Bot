using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Telegram.Bot;
using Telegram.Bot.Args;

namespace FantamortoBot
{
	class FantamortoBot
	{
		private static readonly string filePath = @"lists\";

		private static readonly Dictionary<string, Action<MessageEventArgs>> commandsMap = new Dictionary<string, Action<MessageEventArgs>>(StringComparer.InvariantCultureIgnoreCase)
		{
			["/new"] = NewList,
			["/list"] = PrintList,
			["/add"] = AddName,
			["/remove"] = RemoveName,
			["/death"] = NewDeath
		};

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

				var command = e.Message.Text.Split(" ")[0];

				if (commandsMap.ContainsKey(command))
					commandsMap[command](e);
			}
		}

		private static bool ListExists(MessageEventArgs e)
		{
			string fileName = filePath + e.Message.From.Id.ToString();
			string response;

			if (File.Exists(fileName))
			{
				return true;
			}
			else
			{
				response = $"No list for {e.Message.From.FirstName} was found, use /new to create a new list";
				botClient.SendTextMessageAsync(e.Message.Chat, response);
			}

			return false;
		}

		private static async void NewList(MessageEventArgs e)
		{
			string fileName = filePath + e.Message.From.Id.ToString();
			string response;

			if (File.Exists(fileName))
			{
				response = $"List for {e.Message.From.Id} already exists";
				PrintList(e);
			}
			else
			{
				response = $"Created new list for {e.Message.From.FirstName}, use /add to add names";
				using FileStream fs = File.Create(fileName);
			}

			await botClient.SendTextMessageAsync(e.Message.Chat, response);
		}

		private static async void PrintList(MessageEventArgs e)
		{
			string fileName = filePath + e.Message.From.Id.ToString();
			string response;

			if (ListExists(e))
			{
				response = await File.ReadAllTextAsync(fileName);

				if (response == "")
				{
					response = "Empty list, use /add to add names";
				}

				await botClient.SendTextMessageAsync(e.Message.Chat, response);
			}
		}

		private static async void AddName(MessageEventArgs e)
		{
			string fileName = filePath + e.Message.From.Id.ToString();
			var args = e.Message.Text.Split(" ", 2);
			string response;

			if (ListExists(e))
			{
				if (args.Length != 2)
				{
					response = "Argument missing, use /add <name>";
				}
				else
				{
					await File.AppendAllTextAsync(fileName, $"{args[1]}\n");
					response = $"Added {args[1]} to {fileName}";
				}
				
				await botClient.SendTextMessageAsync(e.Message.Chat, response);
			}
		}

		private static async void RemoveName(MessageEventArgs e)
		{
			string response = "Command not implemented yet";
			await botClient.SendTextMessageAsync(e.Message.Chat, response);
		}

		private static async void NewDeath(MessageEventArgs e)
		{
			string response = "Command not implemented yet";
			await botClient.SendTextMessageAsync(e.Message.Chat, response);
		}
	}
}
