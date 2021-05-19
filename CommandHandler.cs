using System.IO;

using Telegram.Bot.Args;

namespace Fantamorto_Bot
{
	class CommandHandler
	{
		private readonly MessageEventArgs e;
		private readonly string fileName;
		private string response = "";

		public CommandHandler(string filePath, MessageEventArgs messageEvent)
		{
			e = messageEvent;
			fileName = filePath + e.Message.From.Id.ToString();
			response = "";
		}

		public string GetResponse()
		{
			return response;
		}

		public void NewList()
		{
			if (File.Exists(fileName))
			{
				response = $"List for {e.Message.From.Id} already exists";
				PrintList();
			}
			else
			{
				response = $"Created new list for {e.Message.From.FirstName}, use /add to add names";
				using FileStream fs = File.Create(fileName);
			}
		}

		public void PrintList()
		{
			if (File.Exists(fileName))
			{
				if (new FileInfo(fileName).Length == 0)
				{
					response = "Empty list, use /add to add names";
				}
				else
				{
					response = File.ReadAllText(fileName);
				}
			}
			else
			{
				response = $"No list for {e.Message.From.FirstName} was found, use /new to create a new list";
			}
		}

		public void AddName()
		{
			var args = e.Message.Text.Split(" ", 2);

			if (File.Exists(fileName))
			{
				if (args.Length != 2)
				{
					response = "Argument missing, use /add <name>";
				}
				else
				{
					File.AppendAllTextAsync(fileName, $"{args[1]}\n");
					response = $"Added {args[1]} to {fileName}";
				}
			}
			else
			{
				response = $"No list for {e.Message.From.FirstName} was found, use /new to create a new list";
			}
		}

		public void RemoveName()
		{
			response = "Command not implemented yet";
		}

		public void AddDead()
		{
			response = "Command not implemented yet";
		}
	}
}
