using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Vector
{
	class FillElement : VectorElementBase
	{
		public FillElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.opacity = base.RegisterAttribute<double>("opacity", false);
		}

		public override string ElementName
		{
			get
			{
				return "fill";
			}
		}

		public double Opacity
		{
			get
			{
				return this.opacity.Value;
			}
			set
			{
				this.opacity.Value = value;
			}
		}

		readonly OpenXmlAttribute<double> opacity;
	}
}
