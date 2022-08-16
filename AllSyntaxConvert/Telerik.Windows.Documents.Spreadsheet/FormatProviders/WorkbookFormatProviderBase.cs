using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders
{
	public abstract class WorkbookFormatProviderBase : IWorkbookFormatProvider
	{
		public abstract string Name { get; }

		public virtual string FilesDescription
		{
			get
			{
				return this.SupportedExtensions.First<string>().Trim(new char[] { '.' }).ToUpper() + " Files";
			}
		}

		public abstract IEnumerable<string> SupportedExtensions { get; }

		public abstract bool CanImport { get; }

		public abstract bool CanExport { get; }

		public Workbook Import(Stream input)
		{
			if (!this.CanImport)
			{
				throw new InvalidOperationException("Import not supported.");
			}
			return this.ImportOverride(input);
		}

		protected virtual Workbook ImportOverride(Stream input)
		{
			throw new NotImplementedException();
		}

		public void Export(Workbook workbook, Stream output)
		{
			if (!this.CanExport)
			{
				throw new InvalidOperationException("Export not supported.");
			}
			this.ExportOverride(workbook, output);
		}

		protected virtual void ExportOverride(Workbook workbook, Stream output)
		{
			throw new NotImplementedException();
		}
	}
}
