using System;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders
{
	public abstract class BinaryWorkbookFormatProviderBase : WorkbookFormatProviderBase, IBinaryWorkbookFormatProvider, IWorkbookFormatProvider
	{
		public Workbook Import(byte[] input)
		{
			Workbook result;
			using (MemoryStream memoryStream = new MemoryStream(input))
			{
				result = base.Import(memoryStream);
			}
			return result;
		}

		public byte[] Export(Workbook workbook)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				base.Export(workbook, memoryStream);
				result = memoryStream.ToArray();
			}
			return result;
		}
	}
}
