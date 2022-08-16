using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class ReapplyCellStyleCommand : UndoableWorkbookCommandBase<ReapplyCellStyleCommandContext>
	{
		protected override bool AffectsLayoutOverride(ReapplyCellStyleCommandContext context)
		{
			return true;
		}

		protected override bool CanExecuteOverride(ReapplyCellStyleCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(ReapplyCellStyleCommandContext context)
		{
		}

		protected override void ExecuteOverride(ReapplyCellStyleCommandContext context)
		{
			this.ResetStyle(context.Workbook, context.NewValue);
		}

		protected override void UndoOverride(ReapplyCellStyleCommandContext context)
		{
			this.ResetStyle(context.Workbook, context.OldValue);
		}

		void ResetStyle(Workbook workbook, string styleName)
		{
			using (new UpdateScope(new Action(workbook.History.BeginUndoGroup), new Action(workbook.History.EndUndoGroup)))
			{
				foreach (Worksheet worksheet in workbook.Worksheets)
				{
					ICompressedList<string> propertyValueCollection = worksheet.Cells.PropertyBag.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty);
					IEnumerable<Range<long, string>> enumerable = from x in propertyValueCollection
						where x.Value == styleName
						select x;
					foreach (Range<long, string> range in enumerable)
					{
						CellRange cellRange = WorksheetPropertyBagBase.ConvertLongCellRangeToCellRange(range.Start, range.End);
						worksheet.Cells[cellRange].SetStyleNameAndExpandToFitNumberValuesWidth(range.Value);
					}
				}
			}
		}
	}
}
