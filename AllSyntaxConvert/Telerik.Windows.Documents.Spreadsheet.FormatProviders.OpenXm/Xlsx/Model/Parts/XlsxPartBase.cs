using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts
{
	abstract class XlsxPartBase : OpenXmlPartBase
	{
		public XlsxPartBase(XlsxPartsManager partsManager, string name)
			: base(partsManager, name)
		{
		}

		public new XlsxPartsManager PartsManager
		{
			get
			{
				return (XlsxPartsManager)base.PartsManager;
			}
		}
	}
}
