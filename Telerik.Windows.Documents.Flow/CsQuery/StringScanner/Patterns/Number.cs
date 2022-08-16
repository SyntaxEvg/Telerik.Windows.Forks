using System;
using CsQuery.StringScanner.Implementation;

namespace CsQuery.StringScanner.Patterns
{
	class Number : ExpectPattern
	{
		public bool RequireWhitespaceTerminator { get; set; }

		public override void Initialize(int startIndex, char[] sourceText)
		{
			base.Initialize(startIndex, sourceText);
			this.decimalYet = false;
		}

		public override bool Validate()
		{
			int startIndex = this.StartIndex;
			while (startIndex < this.Source.Length && this.Expect(ref startIndex, this.Source[startIndex]))
			{
			}
			this.EndIndex = startIndex;
			if (this.EndIndex > this.Length || this.EndIndex == this.StartIndex || this.failed)
			{
				this.Result = "";
				return false;
			}
			this.Result = base.GetOuput(this.StartIndex, this.EndIndex, false, false);
			return true;
		}

		protected virtual bool Expect(ref int index, char current)
		{
			this.info.Target = current;
			if (index == this.StartIndex)
			{
				if (!this.info.Numeric && current != '-' && current != '+')
				{
					this.failed = true;
					return false;
				}
			}
			else
			{
				if (this.info.Whitespace || this.info.Operator)
				{
					return false;
				}
				if (current == '.')
				{
					if (this.decimalYet)
					{
						this.failed = true;
						return false;
					}
					this.decimalYet = true;
				}
				else if (!this.info.Numeric)
				{
					this.failed = this.RequireWhitespaceTerminator;
					return false;
				}
			}
			index++;
			return true;
		}

		protected bool failed;

		protected bool decimalYet;
	}
}
