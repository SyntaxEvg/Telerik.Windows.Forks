using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	abstract class SetPropertyCommandContextBase<T> : WorksheetCommandContextBase
	{
		public IPropertyDefinition<T> Property
		{
			get
			{
				return this.property;
			}
		}

		public ICompressedList<T> OldValues
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

		public SetPropertyCommandContextBase(Worksheet worksheet, IPropertyDefinition<T> property)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			this.property = property;
		}

		readonly IPropertyDefinition<T> property;

		ICompressedList<T> oldValues;
	}
}
