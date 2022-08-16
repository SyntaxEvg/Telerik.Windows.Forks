using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Settings
{
	class DefaultTabStopWidthElement : DocxElementBase
	{
		public DefaultTabStopWidthElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.valueAttrubite = base.RegisterAttribute<double>("val", OpenXmlNamespaces.WordprocessingMLNamespace, true);
		}

		public override string ElementName
		{
			get
			{
				return "defaultTabStop";
			}
		}

		public double Value
		{
			get
			{
				return this.valueAttrubite.Value;
			}
			set
			{
				this.valueAttrubite.Value = value;
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			return context.Document.DefaultTabStopWidth != DocumentDefaultStyleSettings.DefaultTabStopWidth;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			base.OnAfterRead(context);
			context.Document.DefaultTabStopWidth = Unit.TwipToDip(this.Value);
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			base.OnBeforeWrite(context);
			this.Value = Unit.DipToTwip(context.Document.DefaultTabStopWidth);
		}

		readonly OpenXmlAttribute<double> valueAttrubite;
	}
}
