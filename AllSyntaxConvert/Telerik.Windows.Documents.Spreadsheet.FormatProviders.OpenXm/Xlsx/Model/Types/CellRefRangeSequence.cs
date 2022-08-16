using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class CellRefRangeSequence
	{
		public CellRefRangeSequence(string sequenceCellRefRanges)
		{
			this.cellRangesSequences = new List<CellRefRange>();
			this.InitializeCellRangeSequence(sequenceCellRefRanges.Trim());
		}

		public CellRefRangeSequence(IEnumerable<CellRefRange> cellRangesSequences)
		{
			this.cellRangesSequences = new List<CellRefRange>(cellRangesSequences);
		}

		public IEnumerable<CellRange> CellRanges
		{
			get
			{
				return this.GetCellRanges();
			}
		}

		IEnumerable<CellRange> GetCellRanges()
		{
			foreach (CellRefRange cellRefRange in this.cellRangesSequences)
			{
				yield return cellRefRange.ToCellRange();
			}
			yield break;
		}

		void InitializeCellRangeSequence(string sequenceCellReffRanges)
		{
			string[] array = sequenceCellReffRanges.Split(new char[] { ' ' });
			foreach (string cellRefRange in array)
			{
				this.cellRangesSequences.Add(new CellRefRange(cellRefRange));
			}
		}

		readonly List<CellRefRange> cellRangesSequences;
	}
}
