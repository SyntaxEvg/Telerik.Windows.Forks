using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class DifferentialFormatsElement : DirectElementBase<int>
	{
		public DifferentialFormatsElement()
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "dxfs";
			}
		}

		int Count
		{
			set
			{
				this.count.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(int value)
		{
			this.Count = value;
		}

		protected override void WriteChildElementsOverride(int value)
		{
		}

		protected override void CopyAttributesOverride(ref int value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref int value)
		{
		}

		readonly OpenXmlCountAttribute count;
	}
}
