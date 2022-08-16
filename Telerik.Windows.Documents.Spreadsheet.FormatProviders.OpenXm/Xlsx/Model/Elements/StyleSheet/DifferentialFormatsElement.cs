using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class DifferentialFormatsElement : StyleSheetElementBase
	{
		public DifferentialFormatsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "dxfs";
			}
		}

		public int Count
		{
			get
			{
				return this.count.Value;
			}
			set
			{
				this.count.Value = value;
			}
		}

		protected override bool ShouldExport(IXlsxWorkbookExportContext context)
		{
			return base.ShouldExport(context) || context.DifferentialFormatsContext.Count > 0;
		}

		protected override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			this.Count = context.DifferentialFormatsContext.Count;
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			foreach (DifferentialFormatInfo info in context.DifferentialFormatsContext.GetRegisteredDifferentialFormats())
			{
				DifferentialFormatElement formatElement = base.CreateElement<DifferentialFormatElement>("dxf");
				formatElement.CopyPropertiesFrom(context, info);
				yield return formatElement;
			}
			yield break;
		}

		readonly OpenXmlCountAttribute count;
	}
}
