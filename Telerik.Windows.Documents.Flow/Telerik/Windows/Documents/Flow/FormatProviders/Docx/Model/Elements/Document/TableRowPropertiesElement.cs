using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TableRowPropertiesElements;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class TableRowPropertiesElement : DocumentElementBase
	{
		public TableRowPropertiesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.tableCellSpacingElement = base.RegisterChildElement<TableWidthElement>("tblCellSpacing");
			this.repeatOnEveryPageElement = base.RegisterChildElement<RepeatOnEveryPageElement>("tblHeader");
			this.repeatOnEveryPageElement = base.RegisterChildElement<RepeatOnEveryPageElement>("tblHeader");
			this.canSplitElement = base.RegisterChildElement<CanSplitElement>("cantSplit");
			this.tableRowHeightElement = base.RegisterChildElement<TableRowHeightElement>("trHeight");
		}

		public override string ElementName
		{
			get
			{
				return "trPr";
			}
		}

		public TableRowProperties TableRowProperties
		{
			get
			{
				return this.tableRowProperties;
			}
		}

		public void SetAssociatedFlowModelElement(TableRowProperties tableRowProperties)
		{
			Guard.ThrowExceptionIfNull<TableRowProperties>(tableRowProperties, "tableRowProperties");
			this.tableRowProperties = tableRowProperties;
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return this.tableRowProperties != null || base.ShouldExport(context);
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			if (this.tableCellSpacingElement.Element != null)
			{
				this.tableRowProperties.TableCellSpacing.LocalValue = new double?(this.tableCellSpacingElement.Element.ToDouble());
				base.ReleaseElement(this.tableCellSpacingElement);
			}
			if (this.repeatOnEveryPageElement.Element != null)
			{
				this.tableRowProperties.RepeatOnEveryPage.LocalValue = new bool?(this.repeatOnEveryPageElement.Element.Value);
				base.ReleaseElement(this.repeatOnEveryPageElement);
			}
			if (this.canSplitElement.Element != null)
			{
				this.tableRowProperties.CanSplit.LocalValue = new bool?(!this.canSplitElement.Element.Value);
				base.ReleaseElement(this.canSplitElement);
			}
			if (this.tableRowHeightElement.Element != null)
			{
				this.tableRowProperties.Height.LocalValue = this.tableRowHeightElement.Element.Value;
				base.ReleaseElement(this.tableRowHeightElement);
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			if (this.tableRowProperties.TableCellSpacing.HasLocalValue)
			{
				base.CreateElement(this.tableCellSpacingElement);
				this.tableCellSpacingElement.Element.Value = new TableWidthUnit(TableWidthUnitType.Fixed, this.tableRowProperties.TableCellSpacing.LocalValue.Value);
			}
			if (!this.tableRowProperties.RepeatOnEveryPage.LocalValue.Value.Equals(DocumentDefaultStyleSettings.TableRowRepeatOnEveryPage))
			{
				base.CreateElement(this.repeatOnEveryPageElement);
				this.repeatOnEveryPageElement.Element.Value = this.tableRowProperties.RepeatOnEveryPage.LocalValue.Value;
			}
			if (!this.tableRowProperties.CanSplit.LocalValue.Value.Equals(DocumentDefaultStyleSettings.TableRowCanSplit))
			{
				base.CreateElement(this.canSplitElement);
				this.canSplitElement.Element.Value = !this.tableRowProperties.CanSplit.LocalValue.Value;
			}
			if (!this.tableRowProperties.Height.LocalValue.Equals(DocumentDefaultStyleSettings.TableRowHeight))
			{
				base.CreateElement(this.tableRowHeightElement);
				this.tableRowHeightElement.Element.Value = this.tableRowProperties.Height.LocalValue;
			}
		}

		readonly OpenXmlChildElement<TableWidthElement> tableCellSpacingElement;

		readonly OpenXmlChildElement<RepeatOnEveryPageElement> repeatOnEveryPageElement;

		readonly OpenXmlChildElement<CanSplitElement> canSplitElement;

		readonly OpenXmlChildElement<TableRowHeightElement> tableRowHeightElement;

		TableRowProperties tableRowProperties;
	}
}
