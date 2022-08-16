using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.FieldCode
{
	public class FieldArgument
	{
		internal FieldArgument(string value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			this.value = value;
		}

		public string Value
		{
			get
			{
				return this.value;
			}
		}

		readonly string value;
	}
}
