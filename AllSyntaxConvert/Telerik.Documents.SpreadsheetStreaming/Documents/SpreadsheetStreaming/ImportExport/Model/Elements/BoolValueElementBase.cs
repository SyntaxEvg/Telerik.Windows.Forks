using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements
{
	abstract class BoolValueElementBase : DirectElementBase<bool>
	{
		public BoolValueElementBase()
		{
			this.value = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("val", true, false));
		}

		bool Value
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

		protected override void InitializeAttributesOverride(bool value)
		{
			this.Value = value;
		}

		protected override void CopyAttributesOverride(ref bool value)
		{
			value = this.Value;
		}

		protected override void WriteChildElementsOverride(bool value)
		{
		}

		readonly BoolOpenXmlAttribute value;
	}
}
