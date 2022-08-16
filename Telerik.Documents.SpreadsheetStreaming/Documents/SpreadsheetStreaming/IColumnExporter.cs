using System;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public interface IColumnExporter : IDisposable
	{
		void SetWidthInPixels(double value);

		void SetWidthInCharacters(double count);

		void SetOutlineLevel(int value);

		void SetHidden(bool value);
	}
}
