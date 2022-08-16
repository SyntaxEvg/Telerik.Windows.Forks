using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.ParagraphPropertiesElements
{
	class NumberingPropertiesElement : DocxElementBase
	{
		public NumberingPropertiesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.listLevel = base.RegisterChildElement<ParagraphListLevel>("ilvl");
			this.listId = base.RegisterChildElement<ListIdElement>("numId");
		}

		public override string ElementName
		{
			get
			{
				return "numPr";
			}
		}

		public void SetAssociatedFlowModelElement(ParagraphProperties paragraphProperties)
		{
			Guard.ThrowExceptionIfNull<ParagraphProperties>(paragraphProperties, "paragraphProperties");
			this.paragraphProperties = paragraphProperties;
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return (this.paragraphProperties != null && this.paragraphProperties.ListId.HasLocalValue) || base.ShouldExport(context);
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			if (this.listLevel.Element != null)
			{
				this.paragraphProperties.ListLevel.LocalValue = new int?(this.listLevel.Element.Value);
				base.ReleaseElement(this.listLevel);
			}
			if (this.listId.Element != null)
			{
				int? listIdByNumberedListId = context.ListsImportContext.GetListIdByNumberedListId(this.listId.Element.Value);
				if (listIdByNumberedListId != null)
				{
					this.paragraphProperties.ListId.LocalValue = listIdByNumberedListId;
				}
				base.ReleaseElement(this.listId);
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			if (this.paragraphProperties.ListId.HasLocalValue)
			{
				base.CreateElement(this.listId);
				this.listId.Element.Value = context.ListsExportContext.GetNumberedListIdForParagraphOrStyle(this.paragraphProperties);
			}
			if (this.paragraphProperties.ListLevel.HasLocalValue)
			{
				base.CreateElement(this.listLevel);
				this.listLevel.Element.Value = this.paragraphProperties.ListLevel.LocalValue.Value;
			}
		}

		readonly OpenXmlChildElement<ListIdElement> listId;

		readonly OpenXmlChildElement<ParagraphListLevel> listLevel;

		ParagraphProperties paragraphProperties;
	}
}
