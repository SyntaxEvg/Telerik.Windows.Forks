using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class ExpressionTokenReader
	{
		public bool EndOfFile
		{
			get
			{
				return this.position >= this.input.Length;
			}
		}

		public int Position
		{
			get
			{
				return this.position;
			}
		}

		public ExpressionTokenReader(string input)
		{
			Guard.ThrowExceptionIfNull<string>(input, "input");
			this.input = input;
		}

		public int Peek()
		{
			if (this.EndOfFile)
			{
				return -1;
			}
			return (int)this.input[this.position];
		}

		public int Read()
		{
			if (this.EndOfFile)
			{
				return -1;
			}
			return (int)this.input[this.position++];
		}

		readonly string input;

		int position;
	}
}
