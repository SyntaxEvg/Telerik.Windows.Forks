using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data
{
	class ReferencePropertyValueSource
	{
		public ReferencePropertyValueSource(PostScriptReader reader, IPdfImportContext context, IndirectReference source)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			this.reader = reader;
			this.context = context;
			this.source = source;
		}

		public IndirectReference Source
		{
			get
			{
				return this.source;
			}
		}

		public IPdfImportContext Context
		{
			get
			{
				return this.context;
			}
		}

		public PostScriptReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		readonly PostScriptReader reader;

		readonly IPdfImportContext context;

		readonly IndirectReference source;
	}
}
