using System;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.StringScanner.Patterns
{
	class OptionallyQuoted : Quoted
	{
		public OptionallyQuoted()
		{
			this.SetDefaultTerminators();
		}

		public OptionallyQuoted(IEnumerable<char> terminators)
		{
			if (terminators != null && terminators.Any<char>())
			{
				this.Terminators = terminators;
				return;
			}
			this.SetDefaultTerminators();
		}

		void SetDefaultTerminators()
		{
			this.Terminators = "])}";
		}

		public override void Initialize(int startIndex, char[] sourceText)
		{
			base.Initialize(startIndex, sourceText);
			this.isQuoted = false;
		}

		public IEnumerable<char> Terminators { get; set; }

		public override bool Validate()
		{
			this.isQuoted = CharacterData.IsType(this.Source[this.StartIndex], CharacterType.Quote);
			return base.Validate();
		}

		protected override bool FinishValidate()
		{
			if (this.isQuoted)
			{
				return base.FinishValidate();
			}
			this.Result = base.GetOuput(this.StartIndex, this.EndIndex, false);
			return true;
		}

		protected override bool Expect(ref int index, char current)
		{
			if (this.isQuoted)
			{
				return base.Expect(ref index, current);
			}
			if ((index <= 0 || this.Source[index - 1] != '\\') && this.Terminators.Contains(current))
			{
				return false;
			}
			index++;
			return true;
		}

		bool isQuoted;
	}
}
