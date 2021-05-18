using System;
using Telegram.Bot;

namespace Fantamorto_Bot
{
	class Program
	{
		static void Main(string[] args)
		{
			string accessToken = "1558692324:AAE-SlPNiopanwBJBgSDBOEZYz9BqEqwGrQ";
			var botClient = new TelegramBotClient(accessToken);
			var me = botClient.GetMeAsync().Result;
			Console.WriteLine($"Hello World! I am user {me.Id} and my name is {me.FirstName}.");
		}
	}
}
