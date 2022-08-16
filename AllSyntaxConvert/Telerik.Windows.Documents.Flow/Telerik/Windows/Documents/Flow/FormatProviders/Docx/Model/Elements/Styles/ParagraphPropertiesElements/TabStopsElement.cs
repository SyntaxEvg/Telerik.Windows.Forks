using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.ParagraphPropertiesElements
{
	class TabStopsElement : DocxElementBase
	{
		public TabStopsElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "tabs";
			}
		}

		public void SetAssociatedFlowModelElement(ParagraphProperties paragraphProperties)
		{
			Guard.ThrowExceptionIfNull<ParagraphProperties>(paragraphProperties, "paragraphProperties");
			this.paragraphProperties = paragraphProperties;
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return this.paragraphProperties != null && this.paragraphProperties.TabStops.HasLocalValue;
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			base.OnAfterReadChildElement(context, childElement);
			TabElement tabElement = (TabElement)childElement;
			TabStop tabStop = new TabStop(Unit.TwipToDip(tabElement.Position), tabElement.Type, tabElement.Leader);
			this.paragraphProperties.TabStops.LocalValue = this.paragraphProperties.TabStops.GetActualValue().Insert(tabStop);
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			foreach (TabStop tabStop in this.paragraphProperties.TabStops.LocalValue)
			{
				yield return this.CreateTabStop(tabStop);
			}
			yield break;
		}

		OpenXmlElementBase CreateTabStop(TabStop tabStop)
		{
			TabElement tabElement = base.CreateElement<TabElement>("tab");
			tabElement.Type = tabStop.Type;
			tabElement.Position = Unit.DipToTwip(tabStop.Position);
			tabElement.Leader = tabStop.Leader;
			return tabElement;
		}

		ParagraphProperties paragraphProperties;
	}
}
