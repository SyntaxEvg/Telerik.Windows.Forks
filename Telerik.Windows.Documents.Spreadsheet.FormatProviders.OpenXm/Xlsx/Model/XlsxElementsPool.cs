using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model
{
	class XlsxElementsPool : OpenXmlElementsPoolBase<XlsxPartsManager>
	{
		public XlsxElementsPool(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		protected override OpenXmlElementBase CreateInstance(string elementName)
		{
			if (XlsxElementsFactory.CanCreateInstance(elementName))
			{
				return XlsxElementsFactory.CreateInstance(elementName, base.PartsManager);
			}
			return base.CreateInstance(elementName);
		}
	}
}
