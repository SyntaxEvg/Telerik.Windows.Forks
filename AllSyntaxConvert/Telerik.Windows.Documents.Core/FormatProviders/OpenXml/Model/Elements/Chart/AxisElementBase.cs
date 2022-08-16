using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	abstract class AxisElementBase : ChartElementBase
	{
		public AxisElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.axisId = base.RegisterChildElement<AxisIdElement>("axId");
			this.scaling = base.RegisterChildElement<ScalingElement>("scaling");
			this.delete = base.RegisterChildElement<DeleteElement>("delete");
			this.axisPosition = base.RegisterChildElement<AxisPositionElement>("axPos");
			this.majorGridlines = base.RegisterChildElement<MajorGridlinesElement>("majorGridlines");
			this.numberFormat = base.RegisterChildElement<NumberFormatElement>("numFmt", "c:numFmt");
			this.shapeProperties = base.RegisterChildElement<ShapePropertiesElement>("spPr", "c:spPr");
			this.crossesAxis = base.RegisterChildElement<CrossesAxisElement>("crossAx");
		}

		public AxisIdElement AxisIdElement
		{
			get
			{
				return this.axisId.Element;
			}
			set
			{
				this.axisId.Element = value;
			}
		}

		public ScalingElement ScalingElement
		{
			get
			{
				return this.scaling.Element;
			}
			set
			{
				this.scaling.Element = value;
			}
		}

		public AxisPositionElement AxisPositionElement
		{
			get
			{
				return this.axisPosition.Element;
			}
			set
			{
				this.axisPosition.Element = value;
			}
		}

		public CrossesAxisElement CrossesAxisElement
		{
			get
			{
				return this.crossesAxis.Element;
			}
			set
			{
				this.crossesAxis.Element = value;
			}
		}

		public DeleteElement DeleteElement
		{
			get
			{
				return this.delete.Element;
			}
			set
			{
				this.delete.Element = value;
			}
		}

		public NumberFormatElement NumberFormatElement
		{
			get
			{
				return this.numberFormat.Element;
			}
			set
			{
				this.numberFormat.Element = value;
			}
		}

		public ShapePropertiesElement ShapePropertiesElement
		{
			get
			{
				return this.shapeProperties.Element;
			}
			set
			{
				this.shapeProperties.Element = value;
			}
		}

		public MajorGridlinesElement MajorGridlinesElement
		{
			get
			{
				return this.majorGridlines.Element;
			}
			set
			{
				this.majorGridlines.Element = value;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, Axis axis, int axisIndex, int crossingAxisIndex, AxisPosition position)
		{
			base.CreateElement(this.axisId);
			this.AxisIdElement.Value = axisIndex;
			base.CreateElement(this.scaling);
			this.ScalingElement.CopyPropertiesFrom(axis);
			base.CreateElement(this.delete);
			this.DeleteElement.Value = !axis.IsVisible;
			base.CreateElement(this.axisPosition);
			this.AxisPositionElement.Value = position;
			if (axis.MajorGridlines.Outline.Fill != null || axis.MajorGridlines.Outline.Width != null)
			{
				base.CreateElement(this.majorGridlines);
				this.MajorGridlinesElement.CopyPropertiesFrom(context, axis.MajorGridlines);
			}
			if (!string.IsNullOrEmpty(axis.NumberFormat))
			{
				base.CreateElement(this.numberFormat);
				this.NumberFormatElement.FormatCode = axis.NumberFormat;
			}
			base.CreateElement(this.shapeProperties);
			this.ShapePropertiesElement.CopyPropertiesFrom(context, axis);
			base.CreateElement(this.crossesAxis);
			this.CrossesAxisElement.Value = crossingAxisIndex;
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, Axis axis, AxisGroupName groupName)
		{
			if (this.AxisIdElement == null)
			{
				throw new InvalidOperationException("The axId element is required.");
			}
			int value = this.AxisIdElement.Value;
			base.ReleaseElement(this.axisId);
			if (this.ScalingElement != null)
			{
				this.ScalingElement.CopyPropertiesTo(axis);
				base.ReleaseElement(this.scaling);
			}
			if (this.AxisPositionElement != null)
			{
				base.ReleaseElement(this.axisPosition);
			}
			if (this.CrossesAxisElement != null)
			{
				int value2 = this.CrossesAxisElement.Value;
				context.RegisterAxisGroup(groupName, value, value2);
				base.ReleaseElement(this.crossesAxis);
				if (this.DeleteElement != null)
				{
					axis.IsVisible = !this.DeleteElement.Value;
					base.ReleaseElement(this.delete);
				}
				if (this.NumberFormatElement != null)
				{
					axis.NumberFormat = this.NumberFormatElement.FormatCode;
					base.ReleaseElement(this.numberFormat);
				}
				if (this.ShapePropertiesElement != null)
				{
					this.ShapePropertiesElement.CopyPropertiesTo(context, axis);
					base.ReleaseElement(this.shapeProperties);
				}
				if (this.MajorGridlinesElement != null)
				{
					this.MajorGridlinesElement.CopyPropertiesTo(context, axis.MajorGridlines);
					base.ReleaseElement(this.majorGridlines);
				}
				return;
			}
			throw new InvalidOperationException("The crossAx element determines how the axes cross one another and is required.");
		}

		readonly OpenXmlChildElement<AxisIdElement> axisId;

		readonly OpenXmlChildElement<ScalingElement> scaling;

		readonly OpenXmlChildElement<DeleteElement> delete;

		readonly OpenXmlChildElement<AxisPositionElement> axisPosition;

		readonly OpenXmlChildElement<MajorGridlinesElement> majorGridlines;

		readonly OpenXmlChildElement<NumberFormatElement> numberFormat;

		readonly OpenXmlChildElement<ShapePropertiesElement> shapeProperties;

		readonly OpenXmlChildElement<CrossesAxisElement> crossesAxis;
	}
}
