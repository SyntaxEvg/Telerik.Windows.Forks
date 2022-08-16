using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class FillElement : StyleSheetElementBase
	{
		public FillElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.gradientFill = base.RegisterChildElement<GradientFillElement>("gradientFill");
			this.patternFill = base.RegisterChildElement<PatternFillElement>("patternFill");
		}

		public IFill Fill
		{
			get
			{
				return this.fill;
			}
		}

		public override string ElementName
		{
			get
			{
				return "fill";
			}
		}

		public GradientFillElement GradientFillElement
		{
			get
			{
				return this.gradientFill.Element;
			}
			set
			{
				this.gradientFill.Element = value;
			}
		}

		public PatternFillElement PatternFillElement
		{
			get
			{
				return this.patternFill.Element;
			}
			set
			{
				this.patternFill.Element = value;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, IFill fill)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IFill>(fill, "fill");
			GradientFill gradientFill = fill as GradientFill;
			PatternFill patternFill = fill as PatternFill;
			NoneFill noneFill = fill as NoneFill;
			if (gradientFill != null)
			{
				base.CreateElement(this.gradientFill);
				this.GradientFillElement.CopyPropertiesFrom(context, gradientFill);
			}
			if (patternFill != null)
			{
				base.CreateElement(this.patternFill);
				this.PatternFillElement.CopyPropertiesFrom(context, patternFill);
			}
			if (noneFill != null)
			{
				base.CreateElement(this.patternFill);
				this.PatternFillElement.CopyPropertiesFrom(context, noneFill);
			}
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			this.fill = this.GetFill(context);
			base.ReleaseElement(this.gradientFill);
			base.ReleaseElement(this.patternFill);
		}

		protected override void ClearOverride()
		{
			this.fill = null;
		}

		IFill GetFill(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			IFill result = NoneFill.Instance;
			if (this.GradientFillElement != null)
			{
				result = this.GradientFillElement.CreateFill(context);
			}
			if (this.PatternFillElement != null)
			{
				result = this.PatternFillElement.CreateFill(context);
			}
			return result;
		}

		readonly OpenXmlChildElement<GradientFillElement> gradientFill;

		readonly OpenXmlChildElement<PatternFillElement> patternFill;

		IFill fill;
	}
}
