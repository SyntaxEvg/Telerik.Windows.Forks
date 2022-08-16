using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.FieldCode
{
	class FieldToken
	{
		public FieldToken(FieldTokenType type, string value, bool isQuoted = false)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			this.tokenType = type;
			this.value = value;
			this.isQuoted = isQuoted;
		}

		public FieldTokenType TokenType
		{
			get
			{
				return this.tokenType;
			}
		}

		public string Value
		{
			get
			{
				return this.value;
			}
		}

		public bool IsQuoted
		{
			get
			{
				return this.isQuoted;
			}
		}

		readonly bool isQuoted;

		readonly string value;

		readonly FieldTokenType tokenType;
	}
}
