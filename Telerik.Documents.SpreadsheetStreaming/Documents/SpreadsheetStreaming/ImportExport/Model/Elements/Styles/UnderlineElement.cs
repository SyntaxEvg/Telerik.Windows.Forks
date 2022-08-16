using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class UnderlineElement : DirectElementBase<string>
	{
		public UnderlineElement()
		{
			this.value = base.RegisterAttribute<string>("val", UnderlineValuesMapper.Single, false);
		}

		public override string ElementName
		{
			get
			{
				return "u";
			}
		}

		string Value
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

		protected override void InitializeAttributesOverride(string value)
		{
			this.Value = value;
		}

		protected override void CopyAttributesOverride(ref string value)
		{
			value = this.Value;
		}

		protected override void WriteChildElementsOverride(string value)
		{
		}

		readonly OpenXmlAttribute<string> value;
	}
}
