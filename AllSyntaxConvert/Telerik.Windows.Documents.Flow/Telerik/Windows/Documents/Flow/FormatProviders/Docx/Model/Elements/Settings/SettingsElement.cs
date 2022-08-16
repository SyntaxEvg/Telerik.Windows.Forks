using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Settings
{
	class SettingsElement : DocxPartRootElementBase
	{
		public SettingsElement(DocxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.evenAndOddHeadersFootersChildElement = base.RegisterChildElement<EvenAndOddHeadersFootersElement>("evenAndOddHeaders");
			this.updateFieldsElementChildElement = base.RegisterChildElement<UpdateFieldsElement>("updateFields");
			this.documentProtection = base.RegisterChildElement<DocumentProtectionElement>("documentProtection");
			this.defaultTabStopWidthChildElement = base.RegisterChildElement<DefaultTabStopWidthElement>("defaultTabStop");
			this.compatibilityElement = base.RegisterChildElement<CompatibilityElement>("compat");
			this.documentVariableCollectionElement = base.RegisterChildElement<DocumentVariableCollectionElement>("docVars");
		}

		public override string ElementName
		{
			get
			{
				return "settings";
			}
		}

		EvenAndOddHeadersFootersElement EvenAndOddHeadersFootersElement
		{
			get
			{
				return this.evenAndOddHeadersFootersChildElement.Element;
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			bool value = context.Document.Properties.HasDifferentEvenOddPageHeadersFooters.LocalValue.Value;
			if (!value.Equals(DocumentDefaultStyleSettings.HasDifferentEvenOddPageHeadersFooters))
			{
				base.CreateElement(this.evenAndOddHeadersFootersChildElement);
				this.EvenAndOddHeadersFootersElement.Value = value;
			}
			if (context.ExportSettings.AutoUpdateFields)
			{
				base.CreateElement(this.updateFieldsElementChildElement);
				this.updateFieldsElementChildElement.Element.Value = true;
			}
			base.CreateElement(this.compatibilityElement);
			base.CreateElement(this.documentVariableCollectionElement);
			if (context.Document.ProtectionSettings.Enforced || !string.IsNullOrEmpty(context.Document.ProtectionSettings.Hash))
			{
				base.CreateElement(this.documentProtection);
				this.documentProtection.Element.CopyPropertiesFrom(context.Document.ProtectionSettings);
			}
			base.CreateElement(this.defaultTabStopWidthChildElement);
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			if (this.EvenAndOddHeadersFootersElement != null)
			{
				context.Document.Properties.HasDifferentEvenOddPageHeadersFooters.LocalValue = new bool?(this.EvenAndOddHeadersFootersElement.Value);
				base.ReleaseElement(this.evenAndOddHeadersFootersChildElement);
			}
			if (this.documentProtection.Element != null)
			{
				this.documentProtection.Element.CopyPropertiesTo(context.Document.ProtectionSettings);
			}
		}

		readonly OpenXmlChildElement<EvenAndOddHeadersFootersElement> evenAndOddHeadersFootersChildElement;

		readonly OpenXmlChildElement<UpdateFieldsElement> updateFieldsElementChildElement;

		readonly OpenXmlChildElement<CompatibilityElement> compatibilityElement;

		readonly OpenXmlChildElement<DocumentVariableCollectionElement> documentVariableCollectionElement;

		readonly OpenXmlChildElement<DocumentProtectionElement> documentProtection;

		readonly OpenXmlChildElement<DefaultTabStopWidthElement> defaultTabStopWidthChildElement;
	}
}
