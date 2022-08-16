using System;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	abstract class StylePropertyBase
	{
		public abstract bool AffectsLayout { get; }

		public abstract object GetValueAsObject();

		public abstract void SetValue(object value);

		internal abstract void SetValueInternal(object value);
	}
}
