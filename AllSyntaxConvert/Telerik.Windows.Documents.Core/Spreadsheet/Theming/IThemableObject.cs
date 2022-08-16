using System;

namespace Telerik.Windows.Documents.Spreadsheet.Theming
{
	public interface IThemableObject<T>
	{
		bool IsFromTheme { get; }

		T LocalValue { get; }

		T GetActualValue(DocumentTheme theme);
	}
}
