using System;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts
{
	class StylesPart : PartBase<StyleSheetElement>
	{
		public StylesPart(PartContext context)
			: base(context)
		{
		}

		public override string ContentType
		{
			get
			{
				return XlsxContentTypeNames.StylesContentType;
			}
		}

		protected override string PartPath
		{
			get
			{
				return PartPaths.StylesPartPath;
			}
		}

		public void Write(StylesRepository stylesRepository)
		{
			base.RootElement.Write(stylesRepository);
		}

		public void Read(StylesRepository stylesRepository)
		{
			base.RootElement.Read(ref stylesRepository);
		}
	}
}
