using System;

namespace FantamortoBot
{
	class Program
	{
		static void Main(string[] args)
		{
			string accessToken = "1558692324:AAE-SlPNiopanwBJBgSDBOEZYz9BqEqwGrQ";
			var bot = new FantamortoBot(accessToken);

			bot.Start();

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();

			bot.Stop();
		}
	}
}
