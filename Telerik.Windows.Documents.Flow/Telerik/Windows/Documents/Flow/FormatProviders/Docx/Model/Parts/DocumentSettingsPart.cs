using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Settings;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts
{
	class DocumentSettingsPart : DocxPartBase
	{
		public DocumentSettingsPart(DocxPartsManager partsManager)
			: this(partsManager, "/word/settings.xml")
		{
			base.PartsManager.CreateDocumentRelationship(base.Name, DocxRelationshipTypes.SettingsRelationshipType, null);
		}

		public DocumentSettingsPart(DocxPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.settingsElement = new SettingsElement(base.PartsManager, this);
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.settingsElement;
			}
		}

		public override int Level
		{
			get
			{
				return 3;
			}
		}

		public override string ContentType
		{
			get
			{
				return DocxContentTypeNames.DocumentSettingsContentType;
			}
		}

		readonly SettingsElement settingsElement;
	}
}
