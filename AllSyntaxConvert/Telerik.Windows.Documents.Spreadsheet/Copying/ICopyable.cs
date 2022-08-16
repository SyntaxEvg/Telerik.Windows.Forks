using System;

namespace Telerik.Windows.Documents.Spreadsheet.Copying
{
	interface ICopyable<T>
	{
		T Copy(CopyContext context);
	}
}
