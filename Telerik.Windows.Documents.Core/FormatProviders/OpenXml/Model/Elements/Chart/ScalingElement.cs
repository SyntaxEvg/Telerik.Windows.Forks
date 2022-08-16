using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class ScalingElement : ChartElementBase
	{
		public ScalingElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.min = base.RegisterChildElement<MinAxisValueElement>("min");
			this.max = base.RegisterChildElement<MaxAxisValueElement>("max");
		}

		public override string ElementName
		{
			get
			{
				return "scaling";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public MinAxisValueElement MinAxisValueElement
		{
			get
			{
				return this.min.Element;
			}
			set
			{
				this.min.Element = value;
			}
		}

		public MaxAxisValueElement MaxAxisValueElement
		{
			get
			{
				return this.max.Element;
			}
			set
			{
				this.max.Element = value;
			}
		}

		public void CopyPropertiesFrom(Axis axis)
		{
			if (axis.Min != null)
			{
				base.CreateElement(this.min);
				this.MinAxisValueElement.Value = axis.Min.Value;
			}
			if (axis.Max != null)
			{
				base.CreateElement(this.max);
				this.MaxAxisValueElement.Value = axis.Max.Value;
			}
		}

		public void CopyPropertiesTo(Axis axis)
		{
			if (this.MinAxisValueElement != null)
			{
				axis.Min = new double?(this.MinAxisValueElement.Value);
				base.ReleaseElement(this.min);
			}
			if (this.MaxAxisValueElement != null)
			{
				axis.Max = new double?(this.MaxAxisValueElement.Value);
				base.ReleaseElement(this.max);
			}
		}

		readonly OpenXmlChildElement<MinAxisValueElement> min;

		readonly OpenXmlChildElement<MaxAxisValueElement> max;
	}
}
