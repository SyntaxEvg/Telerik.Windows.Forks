using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class FormatSchemeElement : ThemeElementBase
	{
		public FormatSchemeElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.name = base.RegisterAttribute<string>("name", false);
			this.fillStyleList = base.RegisterChildElement<FillStyleListElement>("fillStyleLst");
			this.lineStyleList = base.RegisterChildElement<LineStyleListElement>("lnStyleLst");
			this.effectStyleList = base.RegisterChildElement<EffectStyleListElement>("effectStyleLst");
			this.backgroundFillStyleList = base.RegisterChildElement<BackgroundFillStyleListElement>("bgFillStyleLst");
		}

		public override string ElementName
		{
			get
			{
				return "fmtScheme";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public string Name
		{
			get
			{
				return this.name.Value;
			}
			set
			{
				this.name.Value = value;
			}
		}

		protected override void OnBeforeWrite(IOpenXmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			this.Name = context.Theme.Name;
			base.CreateElement(this.fillStyleList);
			base.CreateElement(this.lineStyleList);
			base.CreateElement(this.effectStyleList);
			base.CreateElement(this.backgroundFillStyleList);
		}

		protected override void OnAfterRead(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			base.ReleaseElement(this.fillStyleList);
			base.ReleaseElement(this.lineStyleList);
			base.ReleaseElement(this.effectStyleList);
			base.ReleaseElement(this.backgroundFillStyleList);
		}

		readonly OpenXmlAttribute<string> name;

		readonly OpenXmlChildElement<FillStyleListElement> fillStyleList;

		readonly OpenXmlChildElement<LineStyleListElement> lineStyleList;

		readonly OpenXmlChildElement<EffectStyleListElement> effectStyleList;

		readonly OpenXmlChildElement<BackgroundFillStyleListElement> backgroundFillStyleList;
	}
}
