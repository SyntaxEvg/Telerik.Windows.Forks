using System;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public interface ICellExporter : IDisposable
	{
		void SetValue(string value);

		void SetValue(double value);

		void SetValue(bool value);

		void SetValue(DateTime value);

		void SetFormula(string value);

		void SetFormat(SpreadCellFormat cellFormat);
	}
}
