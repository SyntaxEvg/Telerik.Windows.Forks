using System;

namespace Telerik.Windows.Documents.Spreadsheet.Measurement
{
	static class LineBreak
	{
		public static bool IsLineBreak(char ch)
		{
			return ch == LineBreak.NewLine;
		}

		public static readonly char NewLine = '\n';

		public static readonly string NewLineString = LineBreak.NewLine.ToString();
	}
}
