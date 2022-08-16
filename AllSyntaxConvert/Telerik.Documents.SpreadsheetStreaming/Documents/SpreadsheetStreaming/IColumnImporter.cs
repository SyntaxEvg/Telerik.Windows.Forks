using System;

namespace Telerik.Documents.SpreadsheetStreaming
{
	interface IColumnImporter
	{
		int FromIndex { get; }

		int ToIndex { get; }

		bool IsCustomWidth { get; }

		double WidthInPixels { get; }

		double WidthInCharacters { get; }

		int OutlineLevel { get; }

		bool IsHidden { get; }
	}
}
