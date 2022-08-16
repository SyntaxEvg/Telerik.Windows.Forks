using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	public class CellPropertyChangedEventArgs : EventArgs
	{
		public IPropertyDefinition Property
		{
			get
			{
				return this.property;
			}
		}

		public CellRange CellRange
		{
			get
			{
				return this.cellRange;
			}
		}

		internal bool ShouldRespectAffectLayoutProperty { get; set; }

		public CellPropertyChangedEventArgs(IPropertyDefinition property, CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition>(property, "property");
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			this.property = property;
			this.cellRange = cellRange;
			this.ShouldRespectAffectLayoutProperty = true;
		}

		readonly IPropertyDefinition property;

		readonly CellRange cellRange;
	}
}
