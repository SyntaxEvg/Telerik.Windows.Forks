using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements
{
	abstract class RequiredValueElementBase<T> : DirectElementBase<T>
	{
		public RequiredValueElementBase()
		{
			this.value = base.RegisterAttribute<T>("val", true);
		}

		protected T Value
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

		protected override void InitializeAttributesOverride(T value)
		{
			this.Value = value;
		}

		protected override void CopyAttributesOverride(ref T value)
		{
			if (this.value.HasValue)
			{
				value = this.Value;
			}
		}

		protected override void WriteChildElementsOverride(T value)
		{
		}

		readonly OpenXmlAttribute<T> value;
	}
}
