using System;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts
{
	class ContentTypesPart : PartBase<TypesElement>
	{
		public ContentTypesPart(PartContext context)
			: base(context)
		{
		}

		public override string ContentType
		{
			get
			{
				throw new InvalidOperationException("This part don't have content type.");
			}
		}

		protected override string PartPath
		{
			get
			{
				return PartPaths.ContentTypesPath;
			}
		}

		public void Write(ContentTypesRepository contentTypesRepository)
		{
			base.RootElement.Write(contentTypesRepository);
		}

		public void Read(ContentTypesRepository contentTypesRepository)
		{
			base.RootElement.Read(ref contentTypesRepository);
		}
	}
}
