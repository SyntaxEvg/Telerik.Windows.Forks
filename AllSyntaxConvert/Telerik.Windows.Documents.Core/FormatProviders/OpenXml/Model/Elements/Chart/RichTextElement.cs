using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class RichTextElement : ChartElementBase
	{
		public RichTextElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.bodyProperties = base.RegisterChildElement<BodyPropertiesElement>("bodyPr");
		}

		public override string ElementName
		{
			get
			{
				return "rich";
			}
		}

		public BodyPropertiesElement BodyPropertiesElement
		{
			get
			{
				return this.bodyProperties.Element;
			}
			set
			{
				this.bodyProperties.Element = value;
			}
		}

		public void CopyPropertiesFrom(TextTitle textTitle)
		{
			base.CreateElement(this.bodyProperties);
			this.exportedPlainText = textTitle.Text;
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			ParagraphElement paragraphElement = base.CreateElement<ParagraphElement>("a:p");
			paragraphElement.CopyPropertiesFrom(this.exportedPlainText);
			yield return paragraphElement;
			yield break;
		}

		protected override void OnAfterReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase childElement)
		{
			if (this.importedPlainText == null)
			{
				this.importedPlainText = new StringBuilder();
			}
			if (childElement.ElementName == "p")
			{
				ParagraphElement paragraphElement = childElement as ParagraphElement;
				string plainText = paragraphElement.GetPlainText();
				this.importedPlainText.AppendLine(plainText);
			}
		}

		public string GetPlainText()
		{
			this.importedPlainText.Remove(this.importedPlainText.Length - Environment.NewLine.Length, Environment.NewLine.Length);
			return this.importedPlainText.ToString();
		}

		protected override void ClearOverride()
		{
			this.importedPlainText = null;
		}

		readonly OpenXmlChildElement<BodyPropertiesElement> bodyProperties;

		string exportedPlainText;

		StringBuilder importedPlainText;
	}
}
