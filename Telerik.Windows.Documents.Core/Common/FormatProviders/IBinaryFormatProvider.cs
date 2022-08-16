using System;

namespace Telerik.Windows.Documents.Common.FormatProviders
{
	public interface IBinaryFormatProvider<T>
	{
		T Import(byte[] input);

		byte[] Export(T document);
	}
}
