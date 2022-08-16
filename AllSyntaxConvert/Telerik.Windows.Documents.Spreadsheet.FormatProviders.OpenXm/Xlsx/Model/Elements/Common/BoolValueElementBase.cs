using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common
{
	abstract class BoolValueElementBase : XlsxElementBase
	{
		public BoolValueElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("val", true, false));
		}

		public bool Value
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

		readonly BoolOpenXmlAttribute value;
	}
}
