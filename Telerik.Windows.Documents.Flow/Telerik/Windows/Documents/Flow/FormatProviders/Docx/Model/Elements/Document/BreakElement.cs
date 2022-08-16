using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class BreakElement : DocumentElementBase
	{
		public BreakElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.typeAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<BreakType>>(new MappedOpenXmlAttribute<BreakType>("type", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.BreakTypeMapper, false));
			this.clearAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<TextWrappingRestartLocation>>(new MappedOpenXmlAttribute<TextWrappingRestartLocation>("clear", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.RestartLocationMapper, false));
		}

		public override string ElementName
		{
			get
			{
				return "br";
			}
		}

		public BreakType Type
		{
			get
			{
				return this.typeAttribute.Value;
			}
			set
			{
				this.typeAttribute.Value = value;
			}
		}

		public TextWrappingRestartLocation TextWrappingRestartLocation
		{
			get
			{
				return this.clearAttribute.Value;
			}
			set
			{
				this.clearAttribute.Value = value;
			}
		}

		internal void CopyPropertiesFrom(IDocxExportContext context, Break br)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			if (br.BreakType == BreakType.LineBreak && br.TextWrappingRestartLocation == TextWrappingRestartLocation.NextLine)
			{
				return;
			}
			this.Type = br.BreakType;
			if (br.BreakType == BreakType.LineBreak)
			{
				this.TextWrappingRestartLocation = br.TextWrappingRestartLocation;
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		readonly MappedOpenXmlAttribute<BreakType> typeAttribute;

		readonly MappedOpenXmlAttribute<TextWrappingRestartLocation> clearAttribute;
	}
}
