using System;
using System.IO;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	interface IEncoder
	{
		int Encode(byte[] data, int off, int length, Stream outStream);

		int Decode(byte[] data, int off, int length, Stream outStream);

		int DecodeString(string data, Stream outStream);
	}
}
