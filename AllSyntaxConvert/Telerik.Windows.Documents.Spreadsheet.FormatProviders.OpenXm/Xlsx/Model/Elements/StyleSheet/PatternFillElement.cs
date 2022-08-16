using System;
using System.Windows.Media;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class PatternFillElement : StyleSheetElementBase
	{
		public PatternFillElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.patternType = base.RegisterAttribute<string>("patternType", PatternTypes.None, false);
			this.foregroundColor = base.RegisterChildElement<ForegroundColorElement>("fgColor");
			this.backgroundColor = base.RegisterChildElement<BackgroundColorElement>("bgColor");
		}

		public string PatternType
		{
			get
			{
				return this.patternType.Value;
			}
			set
			{
				this.patternType.Value = value;
			}
		}

		public BackgroundColorElement BackgroundColorElement
		{
			get
			{
				return this.backgroundColor.Element;
			}
			set
			{
				this.backgroundColor.Element = value;
			}
		}

		public ForegroundColorElement ForegroundColorElement
		{
			get
			{
				return this.foregroundColor.Element;
			}
			set
			{
				this.foregroundColor.Element = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "patternFill";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, NoneFill fill)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<NoneFill>(fill, "fill");
			this.PatternType = PatternTypes.None;
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, PatternFill fill)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PatternFill>(fill, "fill");
			this.PatternType = PatternTypes.GetPatternTypeName(fill.PatternType);
			ThemableColor themableColor = fill.BackgroundColor;
			ThemableColor patternColor = fill.PatternColor;
			if (themableColor != null)
			{
				base.CreateElement(this.backgroundColor);
				this.BackgroundColorElement.CopyPropertiesFrom(context, themableColor);
			}
			if (patternColor != null)
			{
				base.CreateElement(this.foregroundColor);
				this.ForegroundColorElement.CopyPropertiesFrom(context, patternColor);
			}
		}

		public IFill CreateFill(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			if (this.PatternType == PatternTypes.None)
			{
				return NoneFill.Instance;
			}
			PatternType patternType = PatternTypes.GetPatternType(this.PatternType);
			ThemableColor themableColor;
			if (this.BackgroundColorElement != null)
			{
				themableColor = this.BackgroundColorElement.CreateThemableColor(context);
				base.ReleaseElement(this.backgroundColor);
			}
			else
			{
				themableColor = new ThemableColor(Colors.Transparent);
			}
			ThemableColor patternColor;
			if (this.ForegroundColorElement != null)
			{
				patternColor = this.ForegroundColorElement.CreateThemableColor(context);
				base.ReleaseElement(this.foregroundColor);
			}
			else
			{
				patternColor = new ThemableColor(Colors.Black);
			}
			return new PatternFill(patternType, patternColor, themableColor);
		}

		readonly OpenXmlAttribute<string> patternType;

		readonly OpenXmlChildElement<BackgroundColorElement> backgroundColor;

		readonly OpenXmlChildElement<ForegroundColorElement> foregroundColor;
	}
}
