using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	class TokenStringReader
	{
		public TokenStringReader(FieldCharacter start, FieldCharacter end)
		{
			Guard.ThrowExceptionIfNull<FieldCharacter>(start, "start");
			Guard.ThrowExceptionIfNull<FieldCharacter>(end, "end");
			this.input = InlineRangeEditor.GetTextInRange(start, end, true, false);
		}

		public TokenStringReader(string input)
		{
			Guard.ThrowExceptionIfNull<string>(input, "input");
			this.input = input;
		}

		public bool EndOfFile
		{
			get
			{
				return this.position >= this.input.Length;
			}
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

		public void MoveToEnd()
		{
			this.position = this.input.Length;
		}

		readonly string input;

		int position;
	}
}
