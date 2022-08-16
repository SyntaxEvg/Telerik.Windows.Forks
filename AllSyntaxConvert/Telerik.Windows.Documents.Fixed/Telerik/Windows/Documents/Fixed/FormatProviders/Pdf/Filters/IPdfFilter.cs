using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	public interface IPdfFilter
	{
		byte[] Encode(PdfObject encodedObject, byte[] inputData);

		byte[] Decode(PdfObject decodedObject, byte[] inputData, DecodeParameters decodeParameters);

		string Name { get; }
	}
}
