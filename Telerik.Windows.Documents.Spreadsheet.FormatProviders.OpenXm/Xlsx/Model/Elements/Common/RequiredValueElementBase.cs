using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common
{
	abstract class RequiredValueElementBase<T> : XlsxElementBase
	{
		public RequiredValueElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<T>("val", true);
		}

		public T Value
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

		readonly OpenXmlAttribute<T> value;
	}
}
