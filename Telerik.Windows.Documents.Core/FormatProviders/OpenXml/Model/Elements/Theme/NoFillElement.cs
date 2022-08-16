using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class NoFillElement : ThemeElementBase
	{
		public NoFillElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "noFill";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		protected override bool ShouldExport(IOpenXmlExportContext context)
		{
			return true;
		}
	}
}
