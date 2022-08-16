using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TablePropertiesElements
{
	class TableBordersElement : BordersElement
	{
		public TableBordersElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.topBorder = base.RegisterChildElement<BorderElement>("top");
			this.leftBorder = base.RegisterChildElement<BorderElement>("left");
			this.bottomBorder = base.RegisterChildElement<BorderElement>("bottom");
			this.rightBorder = base.RegisterChildElement<BorderElement>("right");
			this.insideHorizontalBorder = base.RegisterChildElement<BorderElement>("insideH");
			this.insideVerticalBorder = base.RegisterChildElement<BorderElement>("insideV");
		}

		public override string ElementName
		{
			get
			{
				return "tblBorders";
			}
		}

		public void SetAssociatedFlowModelElement(TableProperties tableProperties)
		{
			Guard.ThrowExceptionIfNull<TableProperties>(tableProperties, "tableProperties");
			this.tableProperties = tableProperties;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			bool flag = false;
			TableBorders borders = new TableBorders();
			base.SetBorderValue(this.leftBorder, (Border b) => new TableBorders(borders, b, null, null, null, null, null), ref borders, ref flag);
			base.SetBorderValue(this.topBorder, (Border b) => new TableBorders(borders, null, b, null, null, null, null), ref borders, ref flag);
			base.SetBorderValue(this.rightBorder, (Border b) => new TableBorders(borders, null, null, b, null, null, null), ref borders, ref flag);
			base.SetBorderValue(this.bottomBorder, (Border b) => new TableBorders(borders, null, null, null, b, null, null), ref borders, ref flag);
			base.SetBorderValue(this.insideHorizontalBorder, (Border b) => new TableBorders(borders, null, null, null, null, b, null), ref borders, ref flag);
			base.SetBorderValue(this.insideVerticalBorder, (Border b) => new TableBorders(borders, null, null, null, null, null, b), ref borders, ref flag);
			if (flag)
			{
				this.tableProperties.Borders.LocalValue = borders;
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			if (!this.tableProperties.Borders.HasLocalValue)
			{
				return;
			}
			TableBorders localValue = this.tableProperties.Borders.LocalValue;
			base.SetBorderElementProperties(this.leftBorder, localValue.Left, this.tableProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.topBorder, localValue.Top, this.tableProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.rightBorder, localValue.Right, this.tableProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.bottomBorder, localValue.Bottom, this.tableProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.insideHorizontalBorder, localValue.InsideHorizontal, this.tableProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.insideVerticalBorder, localValue.InsideVertical, this.tableProperties, Border.DefaultBorder.Style);
		}

		readonly OpenXmlChildElement<BorderElement> leftBorder;

		readonly OpenXmlChildElement<BorderElement> topBorder;

		readonly OpenXmlChildElement<BorderElement> rightBorder;

		readonly OpenXmlChildElement<BorderElement> bottomBorder;

		readonly OpenXmlChildElement<BorderElement> insideHorizontalBorder;

		readonly OpenXmlChildElement<BorderElement> insideVerticalBorder;

		TableProperties tableProperties;
	}
}
