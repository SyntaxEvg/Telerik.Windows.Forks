using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
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
			foreach (CellRange cellRange in value.CellRanges)
			{
				string value2 = NameConverter.ConvertCellRangeToName(cellRange.FromIndex, cellRange.ToIndex);
				stringBuilder.Append(value2);
				stringBuilder.Append(' ');
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}
	}
}
