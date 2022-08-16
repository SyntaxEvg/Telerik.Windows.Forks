using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet;
using Telerik.Documents.SpreadsheetStreaming.Model;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx
{
	class XlsxMergedCellsExporter : EntityBase, IDisposable
	{
		public XlsxMergedCellsExporter(MergedCellsElementWriter mergedCellsCollectionWriter, List<SpreadCellRange> mergedCellsRanges)
		{
			this.mergedCellsCollectionWriter = mergedCellsCollectionWriter;
			this.mergedCellsRanges = mergedCellsRanges;
		}

		public void MergeCells(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, DefaultValues.RowCount - 1, fromRowIndex, "fromRowIndex");
			Guard.ThrowExceptionIfOutOfRange<int>(0, DefaultValues.ColumnCount - 1, fromColumnIndex, "fromColumnIndex");
			Guard.ThrowExceptionIfOutOfRange<int>(0, DefaultValues.RowCount - 1, toRowIndex, "toRowIndex");
			Guard.ThrowExceptionIfOutOfRange<int>(0, DefaultValues.ColumnCount - 1, toColumnIndex, "toColumnIndex");
			Guard.ThrowExceptionIfLessThan<int>(fromRowIndex, toRowIndex, "toRowIndex");
			Guard.ThrowExceptionIfLessThan<int>(fromColumnIndex, toColumnIndex, "toColumnIndex");
			this.ValidateMergeRegion(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex);
			MergedCellElementWriter mergedCellElementWriter = this.mergedCellsCollectionWriter.CreateMergedCellElementWriter();
			mergedCellElementWriter.Write(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex);
		}

		static bool ValidateRange(int firstFromIndex, int firstToIndex, int secondFromIndex, int secondToIndex)
		{
			return (firstFromIndex >= secondFromIndex && firstFromIndex <= secondToIndex) || (firstToIndex >= secondFromIndex && firstToIndex <= secondToIndex) || (firstFromIndex <= secondFromIndex && firstToIndex >= secondFromIndex);
		}

		void ValidateMergeRegion(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			foreach (SpreadCellRange spreadCellRange in this.mergedCellsRanges)
			{
				bool flag = XlsxMergedCellsExporter.ValidateRange(fromRowIndex, toRowIndex, spreadCellRange.FromRowIndex, spreadCellRange.ToRowIndex);
				bool flag2 = XlsxMergedCellsExporter.ValidateRange(fromColumnIndex, toColumnIndex, spreadCellRange.FromColumnIndex, spreadCellRange.ToColumnIndex);
				if (flag2 && flag)
				{
					string message = string.Format("There is an intersection with previously merged cells range. FromRowIdex: {0}, fromColumnIndex:{1}, toRowIndex:{2}, toColumnIndex:{3} ", new object[] { spreadCellRange.FromRowIndex, spreadCellRange.FromColumnIndex, spreadCellRange.ToRowIndex, spreadCellRange.ToColumnIndex });
					throw new InvalidOperationException(message);
				}
			}
			this.mergedCellsRanges.Add(new SpreadCellRange(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex));
		}

		readonly List<SpreadCellRange> mergedCellsRanges;

		readonly MergedCellsElementWriter mergedCellsCollectionWriter;
	}
}
