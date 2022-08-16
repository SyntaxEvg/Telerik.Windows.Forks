using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Settings
{
	class CompatibilityElement : DocxElementBase
	{
		public CompatibilityElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "compat";
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			return context.ExportSettings.CompatibilityVersion != null;
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			if (context.ExportSettings.CompatibilityVersion != null)
			{
				yield return this.CreateCompatibilitySettingElement("compatibilityMode", "http://schemas.microsoft.com/office/word", context.ExportSettings.CompatibilityVersion.Value.ToString());
			}
			yield break;
		}

		CompatibilitySettingElement CreateCompatibilitySettingElement(string name, string uri, string value)
		{
			CompatibilitySettingElement compatibilitySettingElement = base.CreateElement<CompatibilitySettingElement>("compatSetting");
			compatibilitySettingElement.Name = name;
			compatibilitySettingElement.Uri = uri;
			compatibilitySettingElement.Value = value;
			return compatibilitySettingElement;
		}
	}
}
