using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TableCellPropertiesElements
{
	class TableCellBordersElement : BordersElement
	{
		public TableCellBordersElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.topBorder = base.RegisterChildElement<BorderElement>("top");
			this.leftBorder = base.RegisterChildElement<BorderElement>("left");
			this.bottomBorder = base.RegisterChildElement<BorderElement>("bottom");
			this.rightBorder = base.RegisterChildElement<BorderElement>("right");
			this.insideHorizontalBorder = base.RegisterChildElement<BorderElement>("insideH");
			this.insideVerticalBorder = base.RegisterChildElement<BorderElement>("insideV");
			this.diagonalDownBorder = base.RegisterChildElement<BorderElement>("tl2br");
			this.diagonalUpBorder = base.RegisterChildElement<BorderElement>("tr2bl");
		}

		public override string ElementName
		{
			get
			{
				return "tcBorders";
			}
		}

		public void SetAssociatedFlowModelElement(TableCellProperties tableCellProperties)
		{
			Guard.ThrowExceptionIfNull<TableCellProperties>(tableCellProperties, "tableCellProperties");
			this.tableCellProperties = tableCellProperties;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			bool flag = false;
			TableCellBorders borders = new TableCellBorders();
			base.SetBorderValue(this.leftBorder, (Border b) => new TableCellBorders(borders, b, null, null, null, null, null, null, null), ref borders, ref flag);
			base.SetBorderValue(this.topBorder, (Border b) => new TableCellBorders(borders, null, b, null, null, null, null, null, null), ref borders, ref flag);
			base.SetBorderValue(this.rightBorder, (Border b) => new TableCellBorders(borders, null, null, b, null, null, null, null, null), ref borders, ref flag);
			base.SetBorderValue(this.bottomBorder, (Border b) => new TableCellBorders(borders, null, null, null, b, null, null, null, null), ref borders, ref flag);
			base.SetBorderValue(this.insideHorizontalBorder, (Border b) => new TableCellBorders(borders, null, null, null, null, b, null, null, null), ref borders, ref flag);
			base.SetBorderValue(this.insideVerticalBorder, (Border b) => new TableCellBorders(borders, null, null, null, null, null, b, null, null), ref borders, ref flag);
			base.SetBorderValue(this.diagonalDownBorder, (Border b) => new TableCellBorders(borders, null, null, null, null, null, null, b, null), ref borders, ref flag);
			base.SetBorderValue(this.diagonalUpBorder, (Border b) => new TableCellBorders(borders, null, null, null, null, null, null, null, b), ref borders, ref flag);
			if (flag)
			{
				this.tableCellProperties.Borders.LocalValue = borders;
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			if (!this.tableCellProperties.Borders.HasLocalValue)
			{
				return;
			}
			TableCellBorders localValue = this.tableCellProperties.Borders.LocalValue;
			base.SetBorderElementProperties(this.leftBorder, localValue.Left, this.tableCellProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.topBorder, localValue.Top, this.tableCellProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.rightBorder, localValue.Right, this.tableCellProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.bottomBorder, localValue.Bottom, this.tableCellProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.insideHorizontalBorder, localValue.InsideHorizontal, this.tableCellProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.insideVerticalBorder, localValue.InsideVertical, this.tableCellProperties, Border.DefaultBorder.Style);
			base.SetBorderElementProperties(this.diagonalDownBorder, localValue.DiagonalDown, this.tableCellProperties, Border.EmptyBorder.Style);
			base.SetBorderElementProperties(this.diagonalUpBorder, localValue.DiagonalUp, this.tableCellProperties, Border.EmptyBorder.Style);
		}

		readonly OpenXmlChildElement<BorderElement> leftBorder;

		readonly OpenXmlChildElement<BorderElement> topBorder;

		readonly OpenXmlChildElement<BorderElement> rightBorder;

		readonly OpenXmlChildElement<BorderElement> bottomBorder;

		readonly OpenXmlChildElement<BorderElement> insideHorizontalBorder;

		readonly OpenXmlChildElement<BorderElement> insideVerticalBorder;

		readonly OpenXmlChildElement<BorderElement> diagonalDownBorder;

		readonly OpenXmlChildElement<BorderElement> diagonalUpBorder;

		TableCellProperties tableCellProperties;
	}
}
