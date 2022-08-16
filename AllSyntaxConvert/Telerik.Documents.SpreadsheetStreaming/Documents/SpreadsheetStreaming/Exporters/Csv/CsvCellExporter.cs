using System;
using System.Globalization;
using System.IO;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Csv
{
	class CsvCellExporter : EntityBase, ICellExporter, IDisposable
	{
		internal CsvCellExporter(StreamWriter writer)
		{
			this.writer = writer;
		}

		public void SetValue(string value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			this.value = value;
		}

		public void SetValue(double value)
		{
			Guard.ThrowExceptionIfNaN(value, "value");
			Guard.ThrowExceptionIfInfinity(value, "value");
			this.SetValue(value.ToString(CultureInfo.InvariantCulture));
		}

		public void SetValue(bool value)
		{
			string text = value.ToString().ToUpperInvariant();
			this.SetValue(text);
		}

		public void SetValue(DateTime value)
		{
			this.SetValue(value.ToString());
		}

		public void SetFormula(string value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			this.SetValue(value);
		}

		public void SetFormat(SpreadCellFormat cellFormat)
		{
		}

		internal sealed override void CompleteWriteOverride()
		{
			if (this.value == null)
			{
				return;
			}
			string text = CsvCellExporter.Escape(this.value);
			this.writer.Write(text);
		}

		static string Escape(string text)
		{
			if (text.Contains("\""))
			{
				text = text.Replace("\"", "\"\"");
			}
			if (text.IndexOfAny(CsvCellExporter.charactersToEscape) > -1)
			{
				text = "\"" + text + "\"";
			}
			return text;
		}

		const string Quote = "\"";

		const string EscapedQoute = "\"\"";

		static char[] charactersToEscape = new char[] { ',', '"', '\n' };

		readonly StreamWriter writer;

		string value;
	}
}
