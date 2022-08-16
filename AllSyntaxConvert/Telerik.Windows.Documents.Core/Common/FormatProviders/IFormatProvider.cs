using System;
using System.Collections.Generic;
using System.IO;

namespace Telerik.Windows.Documents.Common.FormatProviders
{
	public interface IFormatProvider<T>
	{
		IEnumerable<string> SupportedExtensions { get; }

		bool CanImport { get; }

		bool CanExport { get; }

		T Import(Stream input);

		void Export(T document, Stream output);
	}
}
