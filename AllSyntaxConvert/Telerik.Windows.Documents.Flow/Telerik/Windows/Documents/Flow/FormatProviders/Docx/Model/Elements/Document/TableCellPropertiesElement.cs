using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Common.Tables;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TableCellPropertiesElements;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class TableCellPropertiesElement : DocumentElementBase
	{
		public TableCellPropertiesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.preferredWidthElement = base.RegisterChildElement<TableWidthElement>("tcW");
			this.gridSpanChildElement = base.RegisterChildElement<GridSpanElement>("gridSpan");
			this.verticalMergeChildElement = base.RegisterChildElement<VerticalMergeElement>("vMerge");
			this.shadingChildElement = base.RegisterChildElement<ShadingElement>("shd");
			this.bordersChildElement = base.RegisterChildElement<TableCellBordersElement>("tcBorders");
			this.canWrapContentElement = base.RegisterChildElement<CanWrapContentElement>("noWrap");
			this.paddingElement = base.RegisterChildElement<TableCellPaddingElement>("tcMar");
			this.textDirectionElement = base.RegisterChildElement<TextDirectionElement>("textDirection");
			this.horizontalMergeChildElement = base.RegisterChildElement<HorizontalMergeElement>("hMerge");
			this.verticalAlignmentElement = base.RegisterChildElement<VerticalAlignmentElement>("vAlign");
			this.ignoreCellMarkerInRowHeightCalculationElement = base.RegisterChildElement<IgnoreCellMarkerInRowHeightCalculationElement>("hideMark");
		}

		public override string ElementName
		{
			get
			{
				return "tcPr";
			}
		}

		public TableCellType TableCellType { get; set; }

		public TableCellBordersElement TableCellBordersElement
		{
			get
			{
				return this.bordersChildElement.Element;
			}
		}

		public TableCellPaddingElement TableCellPaddingElement
		{
			get
			{
				return this.paddingElement.Element;
			}
		}

		public TableCellProperties TableCellProperties
		{
			get
			{
				return this.tableCellProperties;
			}
		}

		GridSpanElement GridSpanElement
		{
			get
			{
				return this.gridSpanChildElement.Element;
			}
		}

		VerticalMergeElement VerticalMergeElement
		{
			get
			{
				return this.verticalMergeChildElement.Element;
			}
		}

		HorizontalMergeElement HorizontalMergeElement
		{
			get
			{
				return this.horizontalMergeChildElement.Element;
			}
		}

		public void SetAssociatedFlowModelElement(TableCellProperties tableCellProperties)
		{
			Guard.ThrowExceptionIfNull<TableCellProperties>(tableCellProperties, "tableCellProperties");
			this.tableCellProperties = tableCellProperties;
			this.TableCellType = TableCellType.Cell;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			if (this.GridSpanElement != null)
			{
				this.tableCellProperties.ColumnSpan.LocalValue = new int?(this.GridSpanElement.Value);
				base.ReleaseElement(this.gridSpanChildElement);
			}
			if (this.VerticalMergeElement != null)
			{
				if (this.VerticalMergeElement.TableCellMergeType == TableCellMergeType.Continue)
				{
					this.TableCellType = TableCellType.VerticallyMerged;
				}
				base.ReleaseElement(this.verticalMergeChildElement);
			}
			if (this.HorizontalMergeElement != null)
			{
				if (this.HorizontalMergeElement.TableCellMergeType == TableCellMergeType.Continue)
				{
					this.TableCellType = TableCellType.HorizontalyMerged;
				}
				base.ReleaseElement(this.horizontalMergeChildElement);
			}
			if (this.shadingChildElement.Element != null)
			{
				this.shadingChildElement.Element.ReadAttributes(this.tableCellProperties);
				base.ReleaseElement(this.shadingChildElement);
			}
			if (this.TableCellBordersElement != null)
			{
				base.ReleaseElement(this.bordersChildElement);
			}
			if (this.TableCellPaddingElement != null)
			{
				base.ReleaseElement(this.paddingElement);
			}
			if (this.ignoreCellMarkerInRowHeightCalculationElement.Element != null)
			{
				this.tableCellProperties.IgnoreCellMarkerInRowHeightCalculation.LocalValue = new bool?(this.ignoreCellMarkerInRowHeightCalculationElement.Element.Value);
				base.ReleaseElement(this.ignoreCellMarkerInRowHeightCalculationElement);
			}
			if (this.canWrapContentElement.Element != null)
			{
				this.tableCellProperties.CanWrapContent.LocalValue = new bool?(!this.canWrapContentElement.Element.Value);
				base.ReleaseElement(this.canWrapContentElement);
			}
			if (this.preferredWidthElement.Element != null)
			{
				this.tableCellProperties.PreferredWidth.LocalValue = this.preferredWidthElement.Element.Value;
				base.ReleaseElement(this.preferredWidthElement);
			}
			if (this.verticalAlignmentElement.Element != null)
			{
				this.tableCellProperties.VerticalAlignment.LocalValue = new VerticalAlignment?(this.verticalAlignmentElement.Element.Value);
				base.ReleaseElement(this.verticalAlignmentElement);
			}
			if (this.textDirectionElement.Element != null)
			{
				this.tableCellProperties.TextDirection.LocalValue = this.textDirectionElement.Element.Value;
				base.ReleaseElement(this.textDirectionElement);
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			if (this.tableCellProperties.ColumnSpan.LocalValue > DocumentDefaultStyleSettings.ColumnSpan)
			{
				base.CreateElement(this.gridSpanChildElement);
				this.GridSpanElement.Value = this.tableCellProperties.ColumnSpan.LocalValue.Value;
			}
			if (this.TableCellType == TableCellType.VerticallyMerged)
			{
				base.CreateElement(this.verticalMergeChildElement);
				this.VerticalMergeElement.TableCellMergeType = TableCellMergeType.Continue;
			}
			else
			{
				IStyleProperty<int?> rowSpan = this.tableCellProperties.RowSpan;
				if (rowSpan.LocalValue.Value > DocumentDefaultStyleSettings.RowSpan)
				{
					base.CreateElement(this.verticalMergeChildElement);
					this.VerticalMergeElement.TableCellMergeType = TableCellMergeType.Restart;
				}
			}
			if (this.tableCellProperties.ShadingPattern.HasLocalValue || this.tableCellProperties.ShadingPatternColor.HasLocalValue || this.tableCellProperties.BackgroundColor.HasLocalValue)
			{
				base.CreateElement(this.shadingChildElement);
				this.shadingChildElement.Element.FillAttributes(this.tableCellProperties);
			}
			if (this.tableCellProperties.Borders.HasLocalValue)
			{
				base.CreateElement(this.bordersChildElement);
				this.TableCellBordersElement.SetAssociatedFlowModelElement(this.tableCellProperties);
			}
			if (this.tableCellProperties.Padding.HasLocalValue)
			{
				base.CreateElement(this.paddingElement);
				this.TableCellPaddingElement.SetAssociatedFlowModelElement(this.tableCellProperties);
			}
			if (!this.tableCellProperties.IgnoreCellMarkerInRowHeightCalculation.LocalValue.Value.Equals(DocumentDefaultStyleSettings.IgnoreCellMarkerInRowHeightCalculation))
			{
				base.CreateElement(this.ignoreCellMarkerInRowHeightCalculationElement);
				this.ignoreCellMarkerInRowHeightCalculationElement.Element.Value = this.tableCellProperties.IgnoreCellMarkerInRowHeightCalculation.LocalValue.Value;
			}
			if (!this.tableCellProperties.CanWrapContent.LocalValue.Value.Equals(DocumentDefaultStyleSettings.CanWrapContent))
			{
				base.CreateElement(this.canWrapContentElement);
				this.canWrapContentElement.Element.Value = !this.tableCellProperties.CanWrapContent.LocalValue.Value;
			}
			if (!this.tableCellProperties.PreferredWidth.LocalValue.Equals(DocumentDefaultStyleSettings.TableCellPreferredWidth))
			{
				base.CreateElement(this.preferredWidthElement);
				TableWidthUnit localValue = this.tableCellProperties.PreferredWidth.LocalValue;
				this.preferredWidthElement.Element.Value = new TableWidthUnit(localValue.Type, localValue.Value);
			}
			if (!this.tableCellProperties.VerticalAlignment.LocalValue.Value.Equals(DocumentDefaultStyleSettings.TableCellVerticalAlignment))
			{
				base.CreateElement(this.verticalAlignmentElement);
				this.verticalAlignmentElement.Element.Value = this.tableCellProperties.VerticalAlignment.LocalValue.Value;
			}
			if (!this.tableCellProperties.TextDirection.LocalValue.Equals(DocumentDefaultStyleSettings.TableCellTextDirection))
			{
				base.CreateElement(this.textDirectionElement);
				this.textDirectionElement.Element.Value = this.tableCellProperties.TextDirection.LocalValue;
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return this.tableCellProperties != null || base.ShouldExport(context);
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "tcBorders")
				{
					this.TableCellBordersElement.SetAssociatedFlowModelElement(this.tableCellProperties);
					return;
				}
				if (!(elementName == "tcMar"))
				{
					return;
				}
				this.TableCellPaddingElement.SetAssociatedFlowModelElement(this.tableCellProperties);
			}
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.tableCellProperties = null;
		}

		readonly OpenXmlChildElement<GridSpanElement> gridSpanChildElement;

		readonly OpenXmlChildElement<VerticalMergeElement> verticalMergeChildElement;

		readonly OpenXmlChildElement<HorizontalMergeElement> horizontalMergeChildElement;

		readonly OpenXmlChildElement<ShadingElement> shadingChildElement;

		readonly OpenXmlChildElement<TableCellBordersElement> bordersChildElement;

		readonly OpenXmlChildElement<TableCellPaddingElement> paddingElement;

		readonly OpenXmlChildElement<IgnoreCellMarkerInRowHeightCalculationElement> ignoreCellMarkerInRowHeightCalculationElement;

		readonly OpenXmlChildElement<CanWrapContentElement> canWrapContentElement;

		readonly OpenXmlChildElement<TableWidthElement> preferredWidthElement;

		readonly OpenXmlChildElement<VerticalAlignmentElement> verticalAlignmentElement;

		readonly OpenXmlChildElement<TextDirectionElement> textDirectionElement;

		TableCellProperties tableCellProperties;
	}
}
