using System;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public abstract class FieldOptionBase
	{
		internal FieldOptionBase(string value)
		{
			this.Value = value;
		}

		public abstract string Value { get; set; }

		internal bool IsAdded { get; set; }
	}
}
