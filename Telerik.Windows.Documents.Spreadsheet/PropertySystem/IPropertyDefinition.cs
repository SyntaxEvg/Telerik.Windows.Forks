using System;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	public interface IPropertyDefinition
	{
		bool UseSameValueAsPreviousOnInsert { get; }

		string Name { get; }

		bool AffectsLayout { get; }

		StylePropertyGroup StylePropertyGroup { get; }
	}
}
