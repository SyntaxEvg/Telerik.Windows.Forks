using System;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders
{
	public abstract class TextBasedWorkbookFormatProviderBase : WorkbookFormatProviderBase, ITextBasedWorkbookFormatProvider, IWorkbookFormatProvider
	{
		public Workbook Import(string input)
		{
			Workbook result;
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

		public string Export(Workbook workbook)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				base.Export(workbook, memoryStream);
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
