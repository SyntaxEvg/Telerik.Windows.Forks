using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class ProtectionElement : DirectElementBase<DiferentialFormat>
	{
		public ProtectionElement()
		{
			this.locked = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("locked"));
		}

		public override string ElementName
		{
			get
			{
				return "protection";
			}
		}

		protected override bool AlwaysExport
		{
			get
			{
				return false;
			}
		}

		bool Locked
		{
			get
			{
				return this.locked.Value;
			}
			set
			{
				this.locked.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(DiferentialFormat value)
		{
			if (value.IsLocked != null)
			{
				this.Locked = value.IsLocked.Value;
			}
		}

		protected override void CopyAttributesOverride(ref DiferentialFormat value)
		{
			if (this.locked.HasValue)
			{
				value.IsLocked = new bool?(this.Locked);
			}
		}

		protected override void WriteChildElementsOverride(DiferentialFormat value)
		{
		}

		readonly BoolOpenXmlAttribute locked;
	}
}
