using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	class TableCellPaddingElement : DocumentElementBase
	{
		public TableCellPaddingElement(DocxPartsManager partsManager, string elementName)
			: base(partsManager)
		{
			this.elementName = elementName;
			this.top = base.RegisterChildElement<TableWidthElement>("top");
			this.left = base.RegisterChildElement<TableWidthElement>("left");
			this.bottom = base.RegisterChildElement<TableWidthElement>("bottom");
			this.right = base.RegisterChildElement<TableWidthElement>("right");
		}

		public override string ElementName
		{
			get
			{
				return this.elementName;
			}
		}

		public void SetAssociatedFlowModelElement(IPropertiesWithPadding properties)
		{
			Guard.ThrowExceptionIfNull<IPropertiesWithPadding>(properties, "properties");
			this.properties = properties;
		}

		protected override OpenXmlElementBase CreateElement(string elementName)
		{
			if (elementName != null && (elementName == "left" || elementName == "top" || elementName == "right" || elementName == "bottom"))
			{
				return new TableWidthElement(base.PartsManager, elementName)
				{
					Part = base.Part
				};
			}
			return base.CreateElement(elementName);
		}

		protected override void ReleaseElementOverride(OpenXmlElementBase element)
		{
			string a;
			if ((a = element.ElementName) == null || (!(a == "left") && !(a == "top") && !(a == "right") && !(a == "bottom")))
			{
				base.ReleaseElementOverride(element);
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			bool flag = false;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			if (this.left.Element != null)
			{
				flag = true;
				num = this.left.Element.ToDouble();
				base.ReleaseElement(this.left);
			}
			if (this.top.Element != null)
			{
				flag = true;
				num2 = this.top.Element.ToDouble();
				base.ReleaseElement(this.top);
			}
			if (this.right.Element != null)
			{
				flag = true;
				num3 = this.right.Element.ToDouble();
				base.ReleaseElement(this.right);
			}
			if (this.bottom.Element != null)
			{
				flag = true;
				num4 = this.bottom.Element.ToDouble();
				base.ReleaseElement(this.bottom);
			}
			if (flag)
			{
				this.properties.Padding.LocalValue = new Padding(num, num2, num3, num4);
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			if (this.properties.Padding.HasLocalValue)
			{
				Padding localValue = this.properties.Padding.LocalValue;
				base.CreateElement(this.left);
				this.left.Element.Value = new TableWidthUnit(TableWidthUnitType.Fixed, localValue.Left);
				base.CreateElement(this.top);
				this.top.Element.Value = new TableWidthUnit(TableWidthUnitType.Fixed, localValue.Top);
				base.CreateElement(this.right);
				this.right.Element.Value = new TableWidthUnit(TableWidthUnitType.Fixed, localValue.Right);
				base.CreateElement(this.bottom);
				this.bottom.Element.Value = new TableWidthUnit(TableWidthUnitType.Fixed, localValue.Bottom);
			}
		}

		readonly string elementName;

		readonly OpenXmlChildElement<TableWidthElement> left;

		readonly OpenXmlChildElement<TableWidthElement> top;

		readonly OpenXmlChildElement<TableWidthElement> right;

		readonly OpenXmlChildElement<TableWidthElement> bottom;

		IPropertiesWithPadding properties;
	}
}
