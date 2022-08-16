using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts
{
	abstract class SheetPartBase : XlsxPartBase
	{
		public SheetPartBase(XlsxPartsManager partsManager, string name)
			: base(partsManager, name)
		{
		}

		public abstract SheetPartType SheetType { get; }
	}
}
