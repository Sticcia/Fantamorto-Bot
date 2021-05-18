using System;

using Telegram.Bot;
using Telegram.Bot.Args;

namespace FantamortoBot
{
	class FantamortoBot
	{
		static ITelegramBotClient botClient;
		public FantamortoBot(string accessToken)
		{
			botClient = new TelegramBotClient(accessToken);
			botClient.OnMessage += Bot_OnMessage;
		}

		public void Start()
		{
			botClient.StartReceiving();
		}

		public void Stop()
		{
			botClient.StopReceiving();
		}

		public static async void Bot_OnMessage(object sender, MessageEventArgs e)
		{
			if (e.Message.Text != null)
			{
				Console.WriteLine($"Received message: {e.Message.Text} from chat: {e.Message.Chat.Id}");

				await botClient.SendTextMessageAsync(e.Message.Chat, e.Message.Text);
			}
		}
	}
}
