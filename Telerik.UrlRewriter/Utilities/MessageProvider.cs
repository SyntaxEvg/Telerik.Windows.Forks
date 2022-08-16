using System;
using System.Collections;
using System.Reflection;
using System.Resources;

namespace Telerik.UrlRewriter.Utilities
{
	sealed class MessageProvider
	{
		MessageProvider()
		{
		}

		public static string FormatString(Message message, params object[] args)
		{
			string text;
			lock (MessageProvider._messageCache)
			{
				if (MessageProvider._messageCache.ContainsKey(message))
				{
					text = (string)MessageProvider._messageCache[message];
				}
				else
				{
					text = MessageProvider._resources.GetString(message.ToString());
					MessageProvider._messageCache.Add(message, text);
				}
			}
			return string.Format(text, args);
		}

		static Hashtable _messageCache = new Hashtable();

		static ResourceManager _resources = new ResourceManager("Telerik.UrlRewriter.Messages", Assembly.GetExecutingAssembly());
	}
}
