using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.Encoders
{
	interface IEncoder
	{
		int Encode(byte[] data, int off, int length, Stream outStream);

		int Decode(byte[] data, int off, int length, Stream outStream);

		int DecodeString(string data, Stream outStream);
	}
}
