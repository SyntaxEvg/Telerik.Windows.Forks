using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.Model;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
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

		public IEnumerable<SpreadCellRange> CellRanges
		{
			get
			{
				return this.GetCellRanges();
			}
		}

		IEnumerable<SpreadCellRange> GetCellRanges()
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
