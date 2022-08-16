using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.StylePropertiesElements;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	class StyleElement : DocxElementBase
	{
		public StyleElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.styleIdAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("styleId", OpenXmlNamespaces.WordprocessingMLNamespace, true));
			this.typeAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<StyleType>>(new MappedOpenXmlAttribute<StyleType>("type", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.StyleTypeMapper, true));
			this.defaultAttribute = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("default", OpenXmlNamespaces.WordprocessingMLNamespace, true));
			this.customAttribute = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("customStyle", OpenXmlNamespaces.WordprocessingMLNamespace, true));
			this.nameElement = base.RegisterChildElement<StyleNameElement>("name");
			this.basedOnStyleIdElement = base.RegisterChildElement<BasedOnStyleIdElement>("basedOn");
			this.nextStyleIdElement = base.RegisterChildElement<NextStyleIdElement>("next");
			this.linkedStyleIdElement = base.RegisterChildElement<LinkedStyleIdElement>("link");
			this.elementUIPriority = base.RegisterChildElement<UIPriorityElement>("uiPriority");
			this.isPrimaryElement = base.RegisterChildElement<IsPrimaryElement>("qFormat");
			this.paragraphPropertiesChildElement = base.RegisterChildElement<ParagraphPropertiesElement>("pPr");
			this.runPropertiesChildElement = base.RegisterChildElement<RunPropertiesElement>("rPr");
			this.tablePropertiesChildElement = base.RegisterChildElement<TablePropertiesElement>("tblPr");
			this.tableRowPropertiesChildElement = base.RegisterChildElement<TableRowPropertiesElement>("trPr");
			this.tableCellPropertiesChildElement = base.RegisterChildElement<TableCellPropertiesElement>("tcPr");
		}

		public override string ElementName
		{
			get
			{
				return "style";
			}
		}

		public Style Style
		{
			get
			{
				return this.style;
			}
			set
			{
				Guard.ThrowExceptionIfNull<Style>(value, "style");
				this.style = value;
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			if (!this.styleIdAttribute.HasValue)
			{
				this.styleIdAttribute.Value = UndefinedStyleIdGenerator.GetNext();
			}
			if (!this.typeAttribute.HasValue)
			{
				this.typeAttribute.Value = StyleType.Paragraph;
			}
			this.Style = new Style(this.styleIdAttribute.Value, this.typeAttribute.Value);
			if (this.defaultAttribute.HasValue)
			{
				this.Style.IsDefault = this.defaultAttribute.Value;
			}
			if (this.customAttribute.HasValue)
			{
				this.Style.IsCustom = this.customAttribute.Value;
			}
			if (this.nameElement.Element != null)
			{
				this.Style.Name = this.nameElement.Element.Value;
			}
			if (this.isPrimaryElement.Element != null)
			{
				this.Style.IsPrimary = this.isPrimaryElement.Element.Value;
			}
			if (this.basedOnStyleIdElement.Element != null)
			{
				this.Style.BasedOnStyleId = this.basedOnStyleIdElement.Element.Value;
			}
			if (this.nextStyleIdElement.Element != null)
			{
				this.Style.NextStyleId = this.nextStyleIdElement.Element.Value;
			}
			if (this.linkedStyleIdElement.Element != null)
			{
				this.Style.LinkedStyleId = this.linkedStyleIdElement.Element.Value;
			}
			if (this.elementUIPriority.Element != null)
			{
				this.Style.UIPriority = this.elementUIPriority.Element.Value;
			}
			if (this.runPropertiesChildElement.Element != null && this.Style.CharacterProperties != null)
			{
				this.runPropertiesChildElement.Element.CopyPropertiesTo(this.Style.CharacterProperties);
			}
			if (this.paragraphPropertiesChildElement.Element != null && this.Style.ParagraphProperties != null)
			{
				this.Style.ParagraphProperties.CopyPropertiesFrom(this.paragraphPropertiesChildElement.Element.ParagraphProperties);
			}
			if (this.tablePropertiesChildElement.Element != null && this.Style.TableProperties != null)
			{
				this.Style.TableProperties.CopyPropertiesFrom(this.tablePropertiesChildElement.Element.TableProperties);
			}
			if (this.tableRowPropertiesChildElement.Element != null && this.Style.TableRowProperties != null)
			{
				this.Style.TableRowProperties.CopyPropertiesFrom(this.tableRowPropertiesChildElement.Element.TableRowProperties);
			}
			if (this.tableCellPropertiesChildElement.Element != null && this.Style.TableCellProperties != null)
			{
				this.Style.TableCellProperties.CopyPropertiesFrom(this.tableCellPropertiesChildElement.Element.TableCellProperties);
			}
			this.ReleaseElements();
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			this.styleIdAttribute.Value = this.Style.Id;
			this.typeAttribute.Value = this.Style.StyleType;
			if (this.Style.IsDefault)
			{
				this.defaultAttribute.Value = this.Style.IsDefault;
			}
			if (this.Style.IsCustom)
			{
				this.customAttribute.Value = this.Style.IsCustom;
			}
			base.CreateElement(this.nameElement);
			this.nameElement.Element.Value = this.style.Name;
			if (this.Style.IsPrimary)
			{
				base.CreateElement(this.isPrimaryElement);
				this.isPrimaryElement.Element.Value = this.Style.IsPrimary;
			}
			if (!string.IsNullOrEmpty(this.Style.BasedOnStyleId))
			{
				base.CreateElement(this.basedOnStyleIdElement);
				this.basedOnStyleIdElement.Element.Value = this.Style.BasedOnStyleId;
			}
			if (!string.IsNullOrEmpty(this.Style.NextStyleId))
			{
				base.CreateElement(this.nextStyleIdElement);
				this.nextStyleIdElement.Element.Value = this.Style.NextStyleId;
			}
			if (!string.IsNullOrEmpty(this.Style.LinkedStyleId))
			{
				base.CreateElement(this.linkedStyleIdElement);
				this.linkedStyleIdElement.Element.Value = this.Style.LinkedStyleId;
			}
			if (this.Style.UIPriority != DocumentDefaultStyleSettings.UIPriority)
			{
				base.CreateElement(this.elementUIPriority);
				this.elementUIPriority.Element.Value = this.Style.UIPriority;
			}
			if (this.Style.CharacterProperties.HasLocalValues())
			{
				base.CreateElement(this.runPropertiesChildElement);
				this.runPropertiesChildElement.Element.SetAssociatedFlowModelElement(this.Style.CharacterProperties);
			}
			if (this.Style.ParagraphProperties != null && this.Style.ParagraphProperties.HasLocalValues())
			{
				base.CreateElement(this.paragraphPropertiesChildElement);
				this.paragraphPropertiesChildElement.Element.SetAssociatedFlowModelElement(this.Style.ParagraphProperties);
			}
			if (this.Style.TableProperties != null && this.Style.TableProperties.HasLocalValues())
			{
				base.CreateElement(this.tablePropertiesChildElement);
				this.tablePropertiesChildElement.Element.SetAssociatedFlowModelElement(this.Style.TableProperties);
			}
			if (this.Style.TableRowProperties != null && this.Style.TableRowProperties.HasLocalValues())
			{
				base.CreateElement(this.tableRowPropertiesChildElement);
				this.tableRowPropertiesChildElement.Element.SetAssociatedFlowModelElement(this.Style.TableRowProperties);
			}
			if (this.Style.TableCellProperties != null && this.Style.TableCellProperties.HasLocalValues())
			{
				base.CreateElement(this.tableCellPropertiesChildElement);
				this.tableCellPropertiesChildElement.Element.SetAssociatedFlowModelElement(this.Style.TableCellProperties);
			}
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "pPr")
				{
					(childElement as ParagraphPropertiesElement).SetAssociatedFlowModelElement(new Style("paragraphStyle", StyleType.Table).ParagraphProperties);
					return;
				}
				if (elementName == "tblPr")
				{
					(childElement as TablePropertiesElement).SetAssociatedFlowModelElement(new Style("tableStyle", StyleType.Table).TableProperties);
					return;
				}
				if (elementName == "trPr")
				{
					(childElement as TableRowPropertiesElement).SetAssociatedFlowModelElement(new Style("tableRowStyle", StyleType.Table).TableRowProperties);
					return;
				}
				if (!(elementName == "tcPr"))
				{
					return;
				}
				(childElement as TableCellPropertiesElement).SetAssociatedFlowModelElement(new Style("tableCellStyle", StyleType.Table).TableCellProperties);
			}
		}

		void ReleaseElements()
		{
			if (this.nameElement.Element != null)
			{
				base.ReleaseElement(this.nameElement);
			}
			if (this.isPrimaryElement.Element != null)
			{
				base.ReleaseElement(this.isPrimaryElement);
			}
			if (this.basedOnStyleIdElement.Element != null)
			{
				base.ReleaseElement(this.basedOnStyleIdElement);
			}
			if (this.nextStyleIdElement.Element != null)
			{
				base.ReleaseElement(this.nextStyleIdElement);
			}
			if (this.linkedStyleIdElement.Element != null)
			{
				base.ReleaseElement(this.linkedStyleIdElement);
			}
			if (this.elementUIPriority.Element != null)
			{
				base.ReleaseElement(this.elementUIPriority);
			}
			if (this.runPropertiesChildElement.Element != null)
			{
				base.ReleaseElement(this.runPropertiesChildElement);
			}
			if (this.paragraphPropertiesChildElement.Element != null)
			{
				base.ReleaseElement(this.paragraphPropertiesChildElement);
			}
			if (this.tablePropertiesChildElement.Element != null)
			{
				base.ReleaseElement(this.tablePropertiesChildElement);
			}
			if (this.tableRowPropertiesChildElement.Element != null)
			{
				base.ReleaseElement(this.tableRowPropertiesChildElement);
			}
			if (this.tableCellPropertiesChildElement.Element != null)
			{
				base.ReleaseElement(this.tableCellPropertiesChildElement);
			}
		}

		readonly OpenXmlAttribute<string> styleIdAttribute;

		readonly MappedOpenXmlAttribute<StyleType> typeAttribute;

		readonly BoolOpenXmlAttribute defaultAttribute;

		readonly BoolOpenXmlAttribute customAttribute;

		readonly OpenXmlChildElement<StyleNameElement> nameElement;

		readonly OpenXmlChildElement<IsPrimaryElement> isPrimaryElement;

		readonly OpenXmlChildElement<NextStyleIdElement> nextStyleIdElement;

		readonly OpenXmlChildElement<LinkedStyleIdElement> linkedStyleIdElement;

		readonly OpenXmlChildElement<BasedOnStyleIdElement> basedOnStyleIdElement;

		readonly OpenXmlChildElement<UIPriorityElement> elementUIPriority;

		readonly OpenXmlChildElement<RunPropertiesElement> runPropertiesChildElement;

		readonly OpenXmlChildElement<ParagraphPropertiesElement> paragraphPropertiesChildElement;

		readonly OpenXmlChildElement<TablePropertiesElement> tablePropertiesChildElement;

		readonly OpenXmlChildElement<TableRowPropertiesElement> tableRowPropertiesChildElement;

		readonly OpenXmlChildElement<TableCellPropertiesElement> tableCellPropertiesChildElement;

		Style style;
	}
}
