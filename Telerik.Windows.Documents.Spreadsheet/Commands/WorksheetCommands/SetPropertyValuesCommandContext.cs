using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetPropertyValuesCommandContext : WorksheetCommandContextBase
	{
		public ICompressedList NewValues
		{
			get
			{
				return this.newValues;
			}
		}

		public IPropertyDefinition Property
		{
			get
			{
				return this.property;
			}
		}

		public ICompressedList OldValues
		{
			get
			{
				return this.oldValues;
			}
			internal set
			{
				this.oldValues = value;
			}
		}

		public SetPropertyValuesCommandContext(Worksheet worksheet, IPropertyDefinition property, ICompressedList newValues)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition>(property, "property");
			Guard.ThrowExceptionIfNull<ICompressedList>(newValues, "newValues");
			this.property = property;
			this.newValues = newValues;
		}

		readonly IPropertyDefinition property;

		ICompressedList oldValues;

		readonly ICompressedList newValues;
	}
}
