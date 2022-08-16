using System;

namespace HtmlParserSharp.Common
{
	class ParserErrorEventArgs : EventArgs
	{
		public string Message { get; set; }

		public bool IsWarning { get; set; }

		public ParserErrorEventArgs(string message, bool isWarning)
		{
			this.Message = message;
			this.IsWarning = isWarning;
		}
	}
}
