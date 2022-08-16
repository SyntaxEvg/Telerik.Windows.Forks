﻿using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Documents.SpreadsheetStreaming.Model;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class CellRefRangeSequenceConverter : IStringConverter<CellRefRangeSequence>
	{
		public CellRefRangeSequence ConvertFromString(string value)
		{
			List<CellRefRange> list = new List<CellRefRange>();
			string[] array = value.Split(new char[] { ' ' });
			foreach (string cellRefRange in array)
			{
				list.Add(new CellRefRange(cellRefRange));
			}
			return new CellRefRangeSequence(list);
		}

		public string ConvertToString(CellRefRangeSequence value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SpreadCellRange spreadCellRange in value.CellRanges)
			{
				string value2 = NameConverter.ConvertCellRangeToName(spreadCellRange.FromRowIndex, spreadCellRange.FromColumnIndex, spreadCellRange.ToRowIndex, spreadCellRange.ToColumnIndex);
				stringBuilder.Append(value2);
				stringBuilder.Append(' ');
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}
	}
}
