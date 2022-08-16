using System;

namespace Telerik.Windows.Documents.Common.FormatProviders
{
	public interface ITextBasedFormatProvider<T>
	{
		T Import(string input);

		string Export(T document);
	}
}
