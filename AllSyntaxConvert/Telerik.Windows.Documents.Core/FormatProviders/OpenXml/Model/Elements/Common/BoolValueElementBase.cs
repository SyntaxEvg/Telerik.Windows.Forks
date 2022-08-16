using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common
{
	abstract class BoolValueElementBase : OpenXmlElementBase
	{
		public BoolValueElementBase(OpenXmlPartsManager partsManager)
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
