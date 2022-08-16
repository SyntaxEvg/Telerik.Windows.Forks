using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TablePropertiesElements;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class TablePropertiesElement : DocumentElementBase
	{
		public TablePropertiesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.styleIdElement = base.RegisterChildElement<StyleIdElement>("tblStyle");
			this.overlapElement = base.RegisterChildElement<OverlapElement>("tblOverlap");
			this.flowDirectionElement = base.RegisterChildElement<FlowDirectionElement>("bidiVisual");
			this.rowBandingElement = base.RegisterChildElement<RowBandingElement>("tblStyleRowBandSize");
			this.columnBandingElement = base.RegisterChildElement<ColumnBandingElement>("tblStyleColBandSize");
			this.preferredWidthElement = base.RegisterChildElement<TableWidthElement>("tblW");
			this.alignmentElement = base.RegisterChildElement<AlignmentElement>("jc");
			this.tableCellSpacingElement = base.RegisterChildElement<TableWidthElement>("tblCellSpacing");
			this.tableIndentElement = base.RegisterChildElement<TableWidthElement>("tblInd");
			this.bordersElement = base.RegisterChildElement<TableBordersElement>("tblBorders");
			this.shadingChildElement = base.RegisterChildElement<ShadingElement>("shd");
			this.layoutTypeElement = base.RegisterChildElement<TableLayoutTypeElement>("tblLayout");
			this.tableCellPaddingElement = base.RegisterChildElement<TableCellPaddingElement>("tblCellMar");
			this.looksElement = base.RegisterChildElement<TableLooksElement>("tblLook");
		}

		public override string ElementName
		{
			get
			{
				return "tblPr";
			}
		}

		public TableBordersElement TableBordersElement
		{
			get
			{
				return this.bordersElement.Element;
			}
		}

		public TableCellPaddingElement TableCellPaddingElement
		{
			get
			{
				return this.tableCellPaddingElement.Element;
			}
		}

		public TableProperties TableProperties
		{
			get
			{
				return this.tableProperties;
			}
		}

		public void SetAssociatedFlowModelElement(TableProperties tableProperties)
		{
			Guard.ThrowExceptionIfNull<TableProperties>(tableProperties, "tableProperties");
			this.tableProperties = tableProperties;
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return this.tableProperties != null || base.ShouldExport(context);
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			if (this.alignmentElement.Element != null)
			{
				this.tableProperties.Alignment.LocalValue = new Alignment?(this.alignmentElement.Element.Value);
				base.ReleaseElement(this.alignmentElement);
			}
			if (this.shadingChildElement.Element != null)
			{
				this.shadingChildElement.Element.ReadAttributes(this.tableProperties);
				base.ReleaseElement(this.shadingChildElement);
			}
			if (this.TableBordersElement != null)
			{
				base.ReleaseElement(this.bordersElement);
			}
			if (this.tableCellSpacingElement.Element != null)
			{
				this.tableProperties.TableCellSpacing.LocalValue = new double?(this.tableCellSpacingElement.Element.ToDouble());
				base.ReleaseElement(this.tableCellSpacingElement);
			}
			if (this.tableIndentElement.Element != null)
			{
				this.tableProperties.Indent.LocalValue = new double?(this.tableIndentElement.Element.ToDouble());
				base.ReleaseElement(this.tableIndentElement);
			}
			if (this.columnBandingElement.Element != null)
			{
				this.tableProperties.ColumnBanding.LocalValue = new int?(this.columnBandingElement.Element.Value);
				base.ReleaseElement(this.columnBandingElement);
			}
			if (this.rowBandingElement.Element != null)
			{
				this.tableProperties.RowBanding.LocalValue = new int?(this.rowBandingElement.Element.Value);
				base.ReleaseElement(this.rowBandingElement);
			}
			if (this.TableCellPaddingElement != null)
			{
				base.ReleaseElement(this.tableCellPaddingElement);
			}
			if (this.preferredWidthElement.Element != null)
			{
				this.tableProperties.PreferredWidth.LocalValue = this.preferredWidthElement.Element.Value;
				base.ReleaseElement(this.preferredWidthElement);
			}
			if (this.flowDirectionElement.Element != null)
			{
				this.tableProperties.FlowDirection.LocalValue = new FlowDirection?(this.flowDirectionElement.Element.Value ? FlowDirection.RightToLeft : FlowDirection.LeftToRight);
				base.ReleaseElement(this.flowDirectionElement);
			}
			if (this.layoutTypeElement.Element != null)
			{
				this.layoutTypeElement.Element.ReadAttributes(this.tableProperties);
				base.ReleaseElement(this.layoutTypeElement);
			}
			if (this.looksElement.Element != null)
			{
				this.looksElement.Element.ReadAttributes(this.tableProperties);
				base.ReleaseElement(this.looksElement);
			}
			if (this.overlapElement.Element != null)
			{
				this.tableProperties.Overlap.LocalValue = new bool?(this.overlapElement.Element.Value);
				base.ReleaseElement(this.overlapElement);
			}
			if (this.styleIdElement.Element != null)
			{
				this.TableProperties.StyleId = this.styleIdElement.Element.Value;
				base.ReleaseElement(this.styleIdElement);
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			if (this.tableProperties.Alignment.HasLocalValue)
			{
				base.CreateElement(this.alignmentElement);
				this.alignmentElement.Element.Value = this.tableProperties.Alignment.LocalValue.Value;
			}
			if (this.tableProperties.ShadingPattern.HasLocalValue || this.tableProperties.ShadingPatternColor.HasLocalValue || this.tableProperties.BackgroundColor.HasLocalValue)
			{
				base.CreateElement(this.shadingChildElement);
				this.shadingChildElement.Element.FillAttributes(this.tableProperties);
			}
			if (this.tableProperties.Borders.HasLocalValue)
			{
				base.CreateElement(this.bordersElement);
				this.TableBordersElement.SetAssociatedFlowModelElement(this.tableProperties);
			}
			if (this.tableProperties.TableCellSpacing.HasLocalValue)
			{
				base.CreateElement(this.tableCellSpacingElement);
				this.tableCellSpacingElement.Element.Value = new TableWidthUnit(TableWidthUnitType.Fixed, this.tableProperties.TableCellSpacing.LocalValue.Value);
			}
			if (this.tableProperties.Indent.HasLocalValue)
			{
				base.CreateElement(this.tableIndentElement);
				this.tableIndentElement.Element.Value = new TableWidthUnit(TableWidthUnitType.Fixed, this.tableProperties.Indent.LocalValue.Value);
			}
			if (this.tableProperties.ColumnBanding.HasLocalValue)
			{
				base.CreateElement(this.columnBandingElement);
				this.columnBandingElement.Element.Value = this.tableProperties.ColumnBanding.LocalValue.Value;
			}
			if (this.tableProperties.RowBanding.HasLocalValue)
			{
				base.CreateElement(this.rowBandingElement);
				this.rowBandingElement.Element.Value = this.tableProperties.RowBanding.LocalValue.Value;
			}
			if (this.tableProperties.TableCellPadding.HasLocalValue)
			{
				base.CreateElement(this.tableCellPaddingElement);
				this.TableCellPaddingElement.SetAssociatedFlowModelElement(this.tableProperties);
			}
			if (!this.tableProperties.PreferredWidth.LocalValue.Equals(DocumentDefaultStyleSettings.TablePreferredWidth))
			{
				base.CreateElement(this.preferredWidthElement);
				TableWidthUnit localValue = this.tableProperties.PreferredWidth.LocalValue;
				this.preferredWidthElement.Element.Value = new TableWidthUnit(localValue.Type, localValue.Value);
			}
			if (!this.tableProperties.FlowDirection.LocalValue.Value.Equals(DocumentDefaultStyleSettings.TableFlowDirection))
			{
				base.CreateElement(this.flowDirectionElement);
				FlowDirection value = this.tableProperties.FlowDirection.LocalValue.Value;
				this.flowDirectionElement.Element.Value = value == FlowDirection.RightToLeft;
			}
			if (!this.tableProperties.LayoutType.LocalValue.Equals(DocumentDefaultStyleSettings.TableLayoutType))
			{
				base.CreateElement(this.layoutTypeElement);
				this.layoutTypeElement.Element.FillAttributes(this.tableProperties);
			}
			if (!this.tableProperties.Looks.LocalValue.Equals(DocumentDefaultStyleSettings.TableLooks))
			{
				base.CreateElement(this.looksElement);
				this.looksElement.Element.FillAttributes(this.tableProperties);
			}
			if (!this.tableProperties.Overlap.LocalValue.Value.Equals(DocumentDefaultStyleSettings.TableOverlap))
			{
				base.CreateElement(this.overlapElement);
				this.overlapElement.Element.Value = this.tableProperties.Overlap.LocalValue.Value;
			}
			if (!string.IsNullOrEmpty(this.TableProperties.StyleId))
			{
				base.CreateElement(this.styleIdElement);
				this.styleIdElement.Element.Value = this.TableProperties.StyleId;
			}
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase element)
		{
			string elementName;
			if ((elementName = element.ElementName) != null)
			{
				if (elementName == "tblBorders")
				{
					this.TableBordersElement.SetAssociatedFlowModelElement(this.tableProperties);
					return;
				}
				if (!(elementName == "tblCellMar"))
				{
					return;
				}
				this.TableCellPaddingElement.SetAssociatedFlowModelElement(this.tableProperties);
			}
		}

		readonly OpenXmlChildElement<AlignmentElement> alignmentElement;

		readonly OpenXmlChildElement<ShadingElement> shadingChildElement;

		readonly OpenXmlChildElement<TableBordersElement> bordersElement;

		readonly OpenXmlChildElement<TableWidthElement> tableCellSpacingElement;

		readonly OpenXmlChildElement<TableWidthElement> tableIndentElement;

		readonly OpenXmlChildElement<RowBandingElement> rowBandingElement;

		readonly OpenXmlChildElement<ColumnBandingElement> columnBandingElement;

		readonly OpenXmlChildElement<TableCellPaddingElement> tableCellPaddingElement;

		readonly OpenXmlChildElement<TableWidthElement> preferredWidthElement;

		readonly OpenXmlChildElement<FlowDirectionElement> flowDirectionElement;

		readonly OpenXmlChildElement<TableLayoutTypeElement> layoutTypeElement;

		readonly OpenXmlChildElement<TableLooksElement> looksElement;

		readonly OpenXmlChildElement<OverlapElement> overlapElement;

		readonly OpenXmlChildElement<StyleIdElement> styleIdElement;

		TableProperties tableProperties;
	}
}
