using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class GradientFillElement : StyleSheetElementBase
	{
		public GradientFillElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.type = base.RegisterAttribute<string>("type", GradientTypes.Linear, false);
			this.degree = base.RegisterAttribute<double>("degree", 0.0, false);
			this.left = base.RegisterAttribute<double>("left", 0.0, false);
			this.right = base.RegisterAttribute<double>("right", 0.0, false);
			this.top = base.RegisterAttribute<double>("top", 0.0, false);
			this.bottom = base.RegisterAttribute<double>("bottom", 0.0, false);
			this.gradientStops = new List<GradientStop>();
		}

		public string Type
		{
			get
			{
				return this.type.Value;
			}
			set
			{
				this.type.Value = value;
			}
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

		public double Degree
		{
			get
			{
				return this.degree.Value;
			}
			set
			{
				this.degree.Value = value;
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
				return "gradientFill";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, GradientFill gradientFill)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<GradientFill>(gradientFill, "gradientFill");
			GradientFillInfo gradientFillInfo = GradientFillInfo.Create(gradientFill);
			if (gradientFillInfo.InfoType != null)
			{
				this.Type = GradientTypes.GetGradientTypeName(gradientFillInfo.InfoType.Value);
			}
			if (gradientFillInfo.Degree != null)
			{
				this.Degree = gradientFillInfo.Degree.Value;
			}
			if (gradientFillInfo.Left != null)
			{
				this.Left = gradientFillInfo.Left.Value;
			}
			if (gradientFillInfo.Right != null)
			{
				this.Right = gradientFillInfo.Right.Value;
			}
			if (gradientFillInfo.Top != null)
			{
				this.Top = gradientFillInfo.Top.Value;
			}
			if (gradientFillInfo.Bottom != null)
			{
				this.Bottom = gradientFillInfo.Bottom.Value;
			}
			this.gradientStops = gradientFillInfo.Stops;
		}

		public IFill CreateFill(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			GradientFillInfo gradientFillInfo = new GradientFillInfo();
			if (this.type.HasValue)
			{
				gradientFillInfo.InfoType = new GradientInfoType?(GradientTypes.GetGradientType(this.Type));
			}
			if (this.degree.HasValue)
			{
				gradientFillInfo.Degree = new double?(this.Degree);
			}
			if (this.left.HasValue)
			{
				gradientFillInfo.Left = new double?(this.Left);
			}
			if (this.right.HasValue)
			{
				gradientFillInfo.Right = new double?(this.Right);
			}
			if (this.top.HasValue)
			{
				gradientFillInfo.Top = new double?(this.Top);
			}
			if (this.bottom.HasValue)
			{
				gradientFillInfo.Bottom = new double?(this.Bottom);
			}
			gradientFillInfo.Stops = new List<GradientStop>(this.gradientStops);
			this.gradientStops.Clear();
			return gradientFillInfo.ToFill();
		}

		protected override bool ShouldExport(IXlsxWorkbookExportContext context)
		{
			return this.gradientStops.Count > 0 || base.ShouldExport(context);
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			foreach (GradientStop stop in this.gradientStops)
			{
				StopElement stopElement = base.CreateElement<StopElement>("stop");
				stopElement.CopyPropertiesFrom(context, stop);
				yield return stopElement;
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IXlsxWorkbookImportContext context, OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			this.gradientStops.Add(((StopElement)element).ToGradientStop(context));
		}

		readonly OpenXmlAttribute<double> bottom;

		readonly OpenXmlAttribute<double> degree;

		readonly OpenXmlAttribute<double> left;

		readonly OpenXmlAttribute<double> right;

		readonly OpenXmlAttribute<double> top;

		readonly OpenXmlAttribute<string> type;

		List<GradientStop> gradientStops;
	}
}
