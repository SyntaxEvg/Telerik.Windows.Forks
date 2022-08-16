using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class FutureFeatureDataStorageAreaElement : XlsxElementBase
	{
		public FutureFeatureDataStorageAreaElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "extLst";
			}
		}

		protected override OpenXmlElementBase CreateElement(string elementName)
		{
			if (elementName == "ext" && base.Part.Name == "/xl/worksheets/sheet{0}.xml")
			{
				return new ExtensionElement(base.PartsManager)
				{
					Part = base.Part
				};
			}
			return base.CreateElement(elementName);
		}
	}
}
