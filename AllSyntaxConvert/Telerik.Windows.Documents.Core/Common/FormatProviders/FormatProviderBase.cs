using System;
using System.Collections.Generic;
using System.IO;

namespace Telerik.Windows.Documents.Common.FormatProviders
{
	public abstract class FormatProviderBase<T> : IFormatProvider<T>
	{
		public abstract IEnumerable<string> SupportedExtensions { get; }

		public abstract bool CanImport { get; }

		public abstract bool CanExport { get; }

		public T Import(Stream input)
		{
			if (!this.CanImport)
			{
				throw new InvalidOperationException("Import not supported.");
			}
			return this.ImportOverride(input);
		}

		public void Export(T document, Stream output)
		{
			if (!this.CanExport)
			{
				throw new InvalidOperationException("Export not supported.");
			}
			this.ExportOverride(document, output);
		}

		protected virtual T ImportOverride(Stream input)
		{
			throw new NotImplementedException();
		}

		protected virtual void ExportOverride(T document, Stream output)
		{
			throw new NotImplementedException();
		}
	}
}
