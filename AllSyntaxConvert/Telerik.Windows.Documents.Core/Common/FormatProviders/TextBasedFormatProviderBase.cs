using System;
using System.IO;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Common.FormatProviders
{
	public abstract class TextBasedFormatProviderBase<T> : FormatProviderBase<T>, ITextBasedFormatProvider<T>
	{
		public T Import(string input)
		{
			Guard.ThrowExceptionIfNull<string>(input, "input");
			T result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (StreamWriter streamWriter = new StreamWriter(memoryStream))
				{
					streamWriter.Write(input);
					streamWriter.Flush();
					memoryStream.Position = 0L;
					result = base.Import(memoryStream);
				}
			}
			return result;
		}

		public string Export(T document)
		{
			Guard.ThrowExceptionIfNull<T>(document, "document");
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				base.Export(document, memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				using (StreamReader streamReader = new StreamReader(memoryStream))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}
	}
}
