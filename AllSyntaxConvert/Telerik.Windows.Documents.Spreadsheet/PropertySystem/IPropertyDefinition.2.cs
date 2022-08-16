using System;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	public interface IPropertyDefinition<T> : IPropertyDefinition
	{
		T DefaultValue { get; }
	}
}
