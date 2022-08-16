using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	class ClientDataElement : WorksheetDrawingElementBase
	{
		public ClientDataElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "clientData";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}
	}
}
