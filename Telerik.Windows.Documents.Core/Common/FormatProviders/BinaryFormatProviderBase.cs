using System;
using System.IO;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Common.FormatProviders
{
	public abstract class BinaryFormatProviderBase<T> : FormatProviderBase<T>, IBinaryFormatProvider<T>
	{
		internal virtual bool ShouldDisposeMemoryStreamOnImport
		{
			get
			{
				return true;
			}
		}

		public T Import(byte[] input)
		{
			Guard.ThrowExceptionIfNull<byte[]>(input, "input");
			MemoryStream memoryStream = new MemoryStream(input);
			if (this.ShouldDisposeMemoryStreamOnImport)
			{
				using (memoryStream)
				{
					return base.Import(memoryStream);
				}
			}
			return base.Import(memoryStream);
		}

		public byte[] Export(T document)
		{
			Guard.ThrowExceptionIfNull<T>(document, "document");
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				base.Export(document, memoryStream);
				result = memoryStream.ToArray();
			}
			return result;
		}
	}
}
