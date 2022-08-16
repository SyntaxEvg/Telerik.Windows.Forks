using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common
{
	abstract class BoolValueElementBase : DocxElementBase
	{
		public BoolValueElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.valueAttribute = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("val", OpenXmlNamespaces.WordprocessingMLNamespace, true, false));
		}

		public bool Value
		{
			get
			{
				return this.valueAttribute.Value;
			}
			set
			{
				this.valueAttribute.Value = value;
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		readonly BoolOpenXmlAttribute valueAttribute;
	}
}
