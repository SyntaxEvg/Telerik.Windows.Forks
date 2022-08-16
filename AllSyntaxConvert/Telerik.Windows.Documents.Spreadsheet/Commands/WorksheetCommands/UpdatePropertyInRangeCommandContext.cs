using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	abstract class UpdatePropertyInRangeCommandContext<T> : WorksheetCommandContextBase
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

		public Func<T, T> NewValueTransform
		{
			get
			{
				return this.newValueTransform;
			}
		}

		public UpdatePropertyInRangeCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, Func<T, T> newValueTransform)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			Guard.ThrowExceptionIfNull<Func<T, T>>(newValueTransform, "newValueTransform");
			this.property = property;
			this.newValueTransform = newValueTransform;
		}

		readonly IPropertyDefinition<T> property;

		ICompressedList<T> oldValues;

		readonly Func<T, T> newValueTransform;
	}
}
