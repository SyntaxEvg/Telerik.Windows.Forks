using System;
using CsQuery.StringScanner.Implementation;

namespace CsQuery.StringScanner.Patterns
{
	class Quoted : ExpectPattern
	{
		public override bool Validate()
		{
			int startIndex = this.StartIndex;
			while (startIndex < this.Source.Length && this.Expect(ref startIndex, this.Source[startIndex]))
			{
			}
			this.EndIndex = startIndex;
			if (this.EndIndex > this.Length || this.EndIndex == this.StartIndex)
			{
				this.Result = "";
				return false;
			}
			return this.FinishValidate();
		}

		protected virtual bool FinishValidate()
		{
			this.Result = base.GetOuput(this.StartIndex, this.EndIndex, true, true);
			return true;
		}

		protected virtual bool Expect(ref int index, char current)
		{
			this.info.Target = current;
			if (index == this.StartIndex)
			{
				this.quoteChar = current;
				if (!this.info.Quote)
				{
					return false;
				}
			}
			else
			{
				bool flag = this.Source[index - 1] == '\\';
				if (current == this.quoteChar && !flag)
				{
					index++;
					return false;
				}
			}
			index++;
			return true;
		}

		char quoteChar;
	}
}
