using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class HeaderFooterElement : WorksheetElementBase
	{
		public HeaderFooterElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.differentFirst = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("differentFirst", false, false));
			this.differentOddEven = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("differentOddEven", false, false));
			this.scaleWithDocument = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("scaleWithDoc", true, false));
			this.alignWithMargins = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("alignWithMargins", true, false));
			this.oddHeader = base.RegisterChildElement<OddHeaderElement>("oddHeader");
			this.oddFooter = base.RegisterChildElement<OddFooterElement>("oddFooter");
			this.evenHeader = base.RegisterChildElement<EvenHeaderElement>("evenHeader");
			this.evenFooter = base.RegisterChildElement<EvenFooterElement>("evenFooter");
			this.firstHeader = base.RegisterChildElement<FirstHeaderElement>("firstHeader");
			this.firstFooter = base.RegisterChildElement<FirstFooterElement>("firstFooter");
		}

		public override string ElementName
		{
			get
			{
				return "headerFooter";
			}
		}

		public bool DifferentFirst
		{
			get
			{
				return this.differentFirst.Value;
			}
			set
			{
				this.differentFirst.Value = value;
			}
		}

		public bool DifferentOddEven
		{
			get
			{
				return this.differentOddEven.Value;
			}
			set
			{
				this.differentOddEven.Value = value;
			}
		}

		public bool ScaleWithDocument
		{
			get
			{
				return this.scaleWithDocument.Value;
			}
			set
			{
				this.scaleWithDocument.Value = value;
			}
		}

		public bool AlignWithMargins
		{
			get
			{
				return this.alignWithMargins.Value;
			}
			set
			{
				this.alignWithMargins.Value = value;
			}
		}

		public OddHeaderElement OddHeader
		{
			get
			{
				return this.oddHeader.Element;
			}
			set
			{
				this.oddHeader.Element = value;
			}
		}

		public OddFooterElement OddFooter
		{
			get
			{
				return this.oddFooter.Element;
			}
			set
			{
				this.oddFooter.Element = value;
			}
		}

		public EvenHeaderElement EvenHeader
		{
			get
			{
				return this.evenHeader.Element;
			}
			set
			{
				this.evenHeader.Element = value;
			}
		}

		public EvenFooterElement EvenFooter
		{
			get
			{
				return this.evenFooter.Element;
			}
			set
			{
				this.evenFooter.Element = value;
			}
		}

		public FirstHeaderElement FirstHeader
		{
			get
			{
				return this.firstHeader.Element;
			}
			set
			{
				this.firstHeader.Element = value;
			}
		}

		public FirstFooterElement FirstFooter
		{
			get
			{
				return this.firstFooter.Element;
			}
			set
			{
				this.firstFooter.Element = value;
			}
		}

		protected override void OnAfterReadAttributes(IXlsxWorksheetImportContext context)
		{
			base.OnAfterReadAttributes(context);
			HeaderFooterSettings headerFooterSettings = context.Worksheet.WorksheetPageSetup.HeaderFooterSettings;
			if (this.differentFirst.HasValue)
			{
				headerFooterSettings.DifferentFirstPage = this.DifferentFirst;
			}
			if (this.differentOddEven.HasValue)
			{
				headerFooterSettings.DifferentOddAndEvenPages = this.DifferentOddEven;
			}
			if (this.alignWithMargins.HasValue)
			{
				headerFooterSettings.AlignWithPageMargins = this.AlignWithMargins;
			}
			if (this.scaleWithDocument.HasValue)
			{
				headerFooterSettings.ScaleWithDocument = this.ScaleWithDocument;
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			base.OnAfterRead(context);
			HeaderFooterSettings headerFooterSettings = context.Worksheet.WorksheetPageSetup.HeaderFooterSettings;
			this.ReadChild(this.OddHeader, headerFooterSettings.Header);
			this.ReadChild(this.OddFooter, headerFooterSettings.Footer);
			if (headerFooterSettings.DifferentOddAndEvenPages)
			{
				this.ReadChild(this.EvenHeader, headerFooterSettings.EvenPageHeader);
				this.ReadChild(this.EvenFooter, headerFooterSettings.EvenPageFooter);
			}
			if (headerFooterSettings.DifferentFirstPage)
			{
				this.ReadChild(this.FirstHeader, headerFooterSettings.FirstPageHeader);
				this.ReadChild(this.FirstFooter, headerFooterSettings.FirstPageFooter);
			}
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			base.OnBeforeWrite(context);
			HeaderFooterSettings headerFooterSettings = context.Worksheet.WorksheetPageSetup.HeaderFooterSettings;
			this.WriteAttributes(headerFooterSettings);
			this.WriteChildren(headerFooterSettings);
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			bool flag = false;
			HeaderFooterSettings headerFooterSettings = context.Worksheet.WorksheetPageSetup.HeaderFooterSettings;
			flag |= !headerFooterSettings.Header.IsEmpty;
			flag |= !headerFooterSettings.Footer.IsEmpty;
			if (headerFooterSettings.DifferentOddAndEvenPages)
			{
				flag |= !headerFooterSettings.EvenPageHeader.IsEmpty;
				flag |= !headerFooterSettings.EvenPageFooter.IsEmpty;
			}
			if (headerFooterSettings.DifferentFirstPage)
			{
				flag |= !headerFooterSettings.FirstPageHeader.IsEmpty;
				flag |= !headerFooterSettings.FirstPageFooter.IsEmpty;
			}
			return flag;
		}

		void ReadChild(HeaderFooterChildElementBase child, HeaderFooterContent content)
		{
			if (child != null)
			{
				content.SetFromContentText(child.InnerText);
			}
		}

		void WriteChildren(HeaderFooterSettings settings)
		{
			this.WriteChild<OddHeaderElement>(this.oddHeader, settings.Header);
			this.WriteChild<OddFooterElement>(this.oddFooter, settings.Footer);
			if (settings.DifferentOddAndEvenPages)
			{
				this.WriteChild<EvenHeaderElement>(this.evenHeader, settings.EvenPageHeader);
				this.WriteChild<EvenFooterElement>(this.evenFooter, settings.EvenPageFooter);
			}
			if (settings.DifferentFirstPage)
			{
				this.WriteChild<FirstHeaderElement>(this.firstHeader, settings.FirstPageHeader);
				this.WriteChild<FirstFooterElement>(this.firstFooter, settings.FirstPageFooter);
			}
		}

		void WriteChild<T>(OpenXmlChildElement<T> child, HeaderFooterContent content) where T : HeaderFooterChildElementBase
		{
			string text = content.BuildContentText();
			if (!string.IsNullOrEmpty(text))
			{
				base.CreateElement(child);
				if (!content.HasValidLength)
				{
					text = text.Substring(0, 255);
				}
				T element = child.Element;
				element.InnerText = text;
			}
		}

		void WriteAttributes(HeaderFooterSettings settings)
		{
			if (settings.DifferentFirstPage != this.differentFirst.DefaultValue)
			{
				this.DifferentFirst = settings.DifferentFirstPage;
			}
			if (settings.DifferentOddAndEvenPages != this.differentOddEven.DefaultValue)
			{
				this.DifferentOddEven = settings.DifferentOddAndEvenPages;
			}
			if (settings.AlignWithPageMargins != this.alignWithMargins.DefaultValue)
			{
				this.AlignWithMargins = settings.AlignWithPageMargins;
			}
			if (settings.ScaleWithDocument != this.scaleWithDocument.DefaultValue)
			{
				this.ScaleWithDocument = settings.ScaleWithDocument;
			}
		}

		readonly BoolOpenXmlAttribute differentFirst;

		readonly BoolOpenXmlAttribute differentOddEven;

		readonly BoolOpenXmlAttribute scaleWithDocument;

		readonly BoolOpenXmlAttribute alignWithMargins;

		readonly OpenXmlChildElement<OddHeaderElement> oddHeader;

		readonly OpenXmlChildElement<OddFooterElement> oddFooter;

		readonly OpenXmlChildElement<EvenHeaderElement> evenHeader;

		readonly OpenXmlChildElement<EvenFooterElement> evenFooter;

		readonly OpenXmlChildElement<FirstHeaderElement> firstHeader;

		readonly OpenXmlChildElement<FirstFooterElement> firstFooter;
	}
}
