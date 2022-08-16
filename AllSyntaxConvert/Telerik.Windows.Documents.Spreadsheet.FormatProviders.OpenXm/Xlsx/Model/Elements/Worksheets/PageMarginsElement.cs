using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class PageMarginsElement : WorksheetElementBase
	{
		public PageMarginsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.bottom = base.RegisterAttribute<double>("bottom", true);
			this.footer = base.RegisterAttribute<double>("footer", true);
			this.header = base.RegisterAttribute<double>("header", true);
			this.left = base.RegisterAttribute<double>("left", true);
			this.right = base.RegisterAttribute<double>("right", true);
			this.top = base.RegisterAttribute<double>("top", true);
		}

		public double Bottom
		{
			get
			{
				return this.bottom.Value;
			}
			set
			{
				this.bottom.Value = value;
			}
		}

		public double Footer
		{
			get
			{
				return this.footer.Value;
			}
			set
			{
				this.footer.Value = value;
			}
		}

		public double Header
		{
			get
			{
				return this.header.Value;
			}
			set
			{
				this.header.Value = value;
			}
		}

		public double Left
		{
			get
			{
				return this.left.Value;
			}
			set
			{
				this.left.Value = value;
			}
		}

		public double Right
		{
			get
			{
				return this.right.Value;
			}
			set
			{
				this.right.Value = value;
			}
		}

		public double Top
		{
			get
			{
				return this.top.Value;
			}
			set
			{
				this.top.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "pageMargins";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			context.ApplyPageMargins(new PageMarginsInfo
			{
				Bottom = this.Bottom,
				Footer = this.Footer,
				Header = this.Header,
				Left = this.Left,
				Right = this.Right,
				Top = this.Top
			});
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			PageMarginsInfo pageMarginsInfo = context.GetPageMarginsInfo();
			this.Bottom = pageMarginsInfo.Bottom;
			this.Footer = pageMarginsInfo.Footer;
			this.Header = pageMarginsInfo.Header;
			this.Left = pageMarginsInfo.Left;
			this.Right = pageMarginsInfo.Right;
			this.Top = pageMarginsInfo.Top;
		}

		readonly OpenXmlAttribute<double> bottom;

		readonly OpenXmlAttribute<double> footer;

		readonly OpenXmlAttribute<double> header;

		readonly OpenXmlAttribute<double> left;

		readonly OpenXmlAttribute<double> right;

		readonly OpenXmlAttribute<double> top;
	}
}
