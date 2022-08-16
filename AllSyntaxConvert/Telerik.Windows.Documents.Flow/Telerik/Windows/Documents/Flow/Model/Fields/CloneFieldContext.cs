using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	class CloneFieldContext : FieldContext
	{
		public CloneFieldContext()
			: base(true)
		{
			this.unpairedFieldCharacters = new List<FieldCharacter>();
			this.strictMode = true;
		}

		public IEnumerable<FieldCharacter> GetUnpairedFieldCharacters()
		{
			List<FieldCharacter> list = new List<FieldCharacter>(this.unpairedFieldCharacters);
			list.AddRange(base.CurrentFieldCharacters);
			return list;
		}

		[Conditional("DEBUG")]
		public void DisableStrictMode()
		{
			this.strictMode = false;
		}

		protected override void OnInvalidSeparatorFieldCharacter(FieldCharacter fieldCharacter)
		{
			this.unpairedFieldCharacters.Add(fieldCharacter);
			if (this.strictMode)
			{
				base.OnInvalidSeparatorFieldCharacter(fieldCharacter);
			}
		}

		protected override void OnInvalidEndFieldCharacter(FieldCharacter fieldCharacter)
		{
			this.unpairedFieldCharacters.Add(fieldCharacter);
			if (this.strictMode)
			{
				base.OnInvalidEndFieldCharacter(fieldCharacter);
			}
		}

		readonly List<FieldCharacter> unpairedFieldCharacters;

		bool strictMode;
	}
}
