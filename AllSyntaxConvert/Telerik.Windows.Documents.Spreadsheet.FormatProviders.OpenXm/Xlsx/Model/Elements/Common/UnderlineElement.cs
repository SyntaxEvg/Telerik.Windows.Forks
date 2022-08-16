using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common
{
	class UnderlineElement : XlsxElementBase
	{
		public UnderlineElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<string>("val", UnderlineValues.Single, false);
		}

		public override string ElementName
		{
			get
			{
				return "u";
			}
		}

		public string Value
		{
			get
			{
				return this.value.Value;
			}
			set
			{
				this.value.Value = value;
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		readonly OpenXmlAttribute<string> value;
	}
}
