using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.ParagraphPropertiesElements
{
	class ParagraphBordersElement : BordersElement
	{
		public ParagraphBordersElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.topBorder = base.RegisterChildElement<BorderElement>("top");
			this.leftBorder = base.RegisterChildElement<BorderElement>("left");
			this.bottomBorder = base.RegisterChildElement<BorderElement>("bottom");
			this.rightBorder = base.RegisterChildElement<BorderElement>("right");
			this.betweenBorder = base.RegisterChildElement<BorderElement>("between");
		}

		public override string ElementName
		{
			get
			{
				return "pBdr";
			}
		}

		public void SetAssociatedFlowModelElement(ParagraphProperties paragraphProperties)
		{
			Guard.ThrowExceptionIfNull<ParagraphProperties>(paragraphProperties, "paragraphProperties");
			this.paragraphProperties = paragraphProperties;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			bool flag = false;
			ParagraphBorders paragraphBorders = new ParagraphBorders();
			base.SetBorderValue(this.leftBorder, new Func<Border, ParagraphBorders>(paragraphBorders.SetLeft), ref paragraphBorders, ref flag);
			base.SetBorderValue(this.topBorder, new Func<Border, ParagraphBorders>(paragraphBorders.SetTop), ref paragraphBorders, ref flag);
			base.SetBorderValue(this.rightBorder, new Func<Border, ParagraphBorders>(paragraphBorders.SetRight), ref paragraphBorders, ref flag);
			base.SetBorderValue(this.bottomBorder, new Func<Border, ParagraphBorders>(paragraphBorders.SetBottom), ref paragraphBorders, ref flag);
			base.SetBorderValue(this.betweenBorder, new Func<Border, ParagraphBorders>(paragraphBorders.SetBetween), ref paragraphBorders, ref flag);
			if (flag)
			{
				this.paragraphProperties.Borders.LocalValue = paragraphBorders;
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			if (!this.paragraphProperties.Borders.HasLocalValue)
			{
				return;
			}
			ParagraphBorders localValue = this.paragraphProperties.Borders.LocalValue;
			base.SetBorderElementProperties(this.leftBorder, localValue.Left, this.paragraphProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.topBorder, localValue.Top, this.paragraphProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.rightBorder, localValue.Right, this.paragraphProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.bottomBorder, localValue.Bottom, this.paragraphProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.betweenBorder, localValue.Between, this.paragraphProperties, Border.DefaultBorder.Style);
		}

		readonly OpenXmlChildElement<BorderElement> leftBorder;

		readonly OpenXmlChildElement<BorderElement> topBorder;

		readonly OpenXmlChildElement<BorderElement> rightBorder;

		readonly OpenXmlChildElement<BorderElement> bottomBorder;

		readonly OpenXmlChildElement<BorderElement> betweenBorder;

		ParagraphProperties paragraphProperties;
	}
}
