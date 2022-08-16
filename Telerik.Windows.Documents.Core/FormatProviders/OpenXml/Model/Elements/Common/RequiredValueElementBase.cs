using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common
{
	abstract class RequiredValueElementBase<T> : OpenXmlElementBase
	{
		public RequiredValueElementBase(OpenXmlPartsManager partsManager)
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
