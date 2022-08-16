using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Charts
{
	public class WorkbookFormulaChartData : FormulaChartData
	{
		public WorkbookFormulaChartData(Worksheet worksheet, params CellRange[] ranges)
			: base(NameConverter.ConvertCellRangesToName(worksheet, ranges, true))
		{
			this.workbook = worksheet.Workbook;
		}

		public WorkbookFormulaChartData(Workbook workbook, string formula)
			: base(formula)
		{
			this.workbook = workbook;
		}

		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		public override IChartData Clone()
		{
			return new WorkbookFormulaChartData(this.workbook, base.Formula);
		}

		public IEnumerable<CellRange> EnumerateCellRanges(out Worksheet worksheet)
		{
			IEnumerable<CellRange> result;
			if (this.TryEnumerateCellRanges(out result, out worksheet))
			{
				return result;
			}
			throw new FormatException("The formula cannot be parsed to cell ranges.");
		}

		public bool TryEnumerateCellRanges(out IEnumerable<CellRange> resultCellRanges, out Worksheet worksheet)
		{
			resultCellRanges = Enumerable.Empty<CellRange>();
			return WorkbookFormulaChartData.TryGetCellRangesFromFormula(base.Formula, this.workbook, out resultCellRanges, out worksheet);
		}

		static bool TryGetCellRangesFromFormula(string formula, Workbook workbook, out IEnumerable<CellRange> resultCellRanges, out Worksheet worksheet)
		{
			RadExpression expression;
			InputStringCollection inputStringCollection;
			RadExpression.TryParse("=" + formula, workbook.Worksheets.First<Worksheet>(), 0, 0, out expression, out inputStringCollection, false);
			RadExpression valueAsConstantOrCellReference = expression.GetValueAsConstantOrCellReference();
			CellReferenceRangeExpression cellReferenceRangeExpression = valueAsConstantOrCellReference as CellReferenceRangeExpression;
			resultCellRanges = Enumerable.Empty<CellRange>();
			worksheet = null;
			if (cellReferenceRangeExpression != null && cellReferenceRangeExpression.IsValid)
			{
				worksheet = cellReferenceRangeExpression.Worksheet;
				List<CellRange> list = new List<CellRange>();
				foreach (CellReferenceRange cellReferenceRange in cellReferenceRangeExpression.CellReferenceRanges)
				{
					list.Add(cellReferenceRange.ToCellRange());
				}
				resultCellRanges = list;
				return true;
			}
			return false;
		}

		readonly Workbook workbook;
	}
}
