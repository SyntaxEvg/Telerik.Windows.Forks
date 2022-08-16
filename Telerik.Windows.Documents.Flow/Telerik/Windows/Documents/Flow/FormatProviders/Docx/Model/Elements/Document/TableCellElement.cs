using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Common.Tables;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class TableCellElement : BlockLevelElementsContainerElementBase<TableCell>
	{
		public TableCellElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.tableCellPropertiesChildElement = base.RegisterChildElement<TableCellPropertiesElement>("tcPr");
		}

		public override string ElementName
		{
			get
			{
				return "tc";
			}
		}

		public TableCellData TableCellData { get; set; }

		TableCellPropertiesElement TableCellPropertiesElement
		{
			get
			{
				return this.tableCellPropertiesChildElement.Element;
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			if (base.BlockContainer.Properties.HasLocalValues())
			{
				base.CreateElement(this.tableCellPropertiesChildElement);
				this.TableCellPropertiesElement.SetAssociatedFlowModelElement(base.BlockContainer.Properties);
				this.TableCellPropertiesElement.TableCellType = this.TableCellData.Type;
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			TableCellType type = TableCellType.Cell;
			if (this.TableCellPropertiesElement != null)
			{
				type = this.TableCellPropertiesElement.TableCellType;
				base.ReleaseElement(this.tableCellPropertiesChildElement);
			}
			this.TableCellData = new TableCellData(type, base.BlockContainer);
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			base.OnBeforeReadChildElement(context, childElement);
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (!(elementName == "tcPr"))
				{
					return;
				}
				this.TableCellPropertiesElement.SetAssociatedFlowModelElement(base.BlockContainer.Properties);
			}
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			DocxHelper.AddHangingAnnotation(context, childElement);
		}

		readonly OpenXmlChildElement<TableCellPropertiesElement> tableCellPropertiesChildElement;
	}
}
