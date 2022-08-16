using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class ParagraphElement : DrawingElementBase
	{
		public ParagraphElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.run = base.RegisterChildElement<RichTextRunElement>("r", "a:r");
		}

		public override string ElementName
		{
			get
			{
				return "p";
			}
		}

		protected override bool ShouldExport(IOpenXmlExportContext context)
		{
			return this.exportedPlainText != null;
		}

		public RichTextRunElement TextRunElement
		{
			get
			{
				return this.run.Element;
			}
			set
			{
				this.run.Element = value;
			}
		}

		public void CopyPropertiesFrom(string text)
		{
			this.exportedPlainText = text;
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			RichTextRunElement runElement = base.CreateElement<RichTextRunElement>("a:r");
			runElement.CopyPropertiesFrom(this.exportedPlainText);
			yield return runElement;
			yield break;
		}

		protected override void OnAfterReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase childElement)
		{
			if (this.importedPlainText == null)
			{
				this.importedPlainText = new StringBuilder();
			}
			if (childElement.ElementName == "r")
			{
				RichTextRunElement richTextRunElement = childElement as RichTextRunElement;
				string plainText = richTextRunElement.GetPlainText();
				this.importedPlainText.Append(plainText);
				base.ReleaseElement(this.run);
			}
		}

		public string GetPlainText()
		{
			return this.importedPlainText.ToString();
		}

		protected override void ClearOverride()
		{
			this.importedPlainText = null;
			this.exportedPlainText = null;
		}

		string exportedPlainText;

		StringBuilder importedPlainText;

		readonly OpenXmlChildElement<RichTextRunElement> run;
	}
}
