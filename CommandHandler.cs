using System;
using System.IO;
using System.Collections.Generic;

using Telegram.Bot.Types;

namespace Fantamorto_Bot
{
	class CommandHandler
	{
		private readonly Message _message;
		private readonly string _fileName;
		private string _response;

		public CommandHandler(string filePath, Message message)
		{
			_message = message;
			_fileName = filePath + _message.From.Id.ToString();
			_response = "";
		}

		public string GetResponse()
		{
			return _response;
		}

		public void NewList()
		{
			if (System.IO.File.Exists(_fileName))
			{
				_response = $"List for {_message.From.FirstName} already exists, use /list to show list";
			}
			else
			{
				_response = $"Created a new list for {_message.From.FirstName}, use /add to add names";
				using FileStream fs = System.IO.File.Create(_fileName);
			}
		}

		public void PrintList()
		{
			if (System.IO.File.Exists(_fileName))
			{
				if (new FileInfo(_fileName).Length == 0)
				{
					_response = "Empty list, use /add to add names";
				}
				else
				{
					_response = System.IO.File.ReadAllText(_fileName);
				}
			}
			else
			{
				_response = $"No list for {_message.From.FirstName} was found, use /new to create a new list";
			}
		}

		public void AddName()
		{
			var args = _message.Text.Split(" ", 2);

			if (System.IO.File.Exists(_fileName))
			{
				if (args.Length != 2)
				{
					_response = "Argument missing, use /add <name>";
				}
				else
				{
					System.IO.File.AppendAllTextAsync(_fileName, $"{args[1]}\n");
					_response = $"Added {args[1]} to list, use /list to show list";
				}
			}
			else
			{
				_response = $"No list for {_message.From.FirstName} was found, use /new to create a new list";
			}
		}

		public void RemoveName()
		{
			List<string> newList = new List<string>();
			bool found = false;
			var args = _message.Text.Split(" ", 2);

			if (System.IO.File.Exists(_fileName))
			{
				if (args.Length != 2)
				{
					_response = "Argument missing, use /remove <name>";
				}
				else
				{
					var oldList = System.IO.File.ReadAllLines(_fileName);

					foreach (var name in oldList)
					{
						if (String.Compare(name, args[1], StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							found = true;
						}
						else
						{
							newList.Add(name);
						}
					}

					if (found)
					{
						System.IO.File.WriteAllLines(_fileName, newList);
						_response = $"Removed {args[1]} from list, use /list to show list";
					}
					else
					{
						_response = $"{args[1]} not found, use /list to show list";
					}
				}
			}
			else
			{
				_response = $"No list for {_message.From.FirstName} was found, use /new to create a new list";
			}
		}

		public void AddDead()
		{
			_response = "Command not implemented yet";
		}
	}
}
