using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class TitleElement : ChartElementBase
	{
		public TitleElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.chartTextElement = base.RegisterChildElement<ChartTextElement>("tx", "charttx");
			this.overlay = base.RegisterChildElement<OverlayElement>("overlay");
		}

		public override string ElementName
		{
			get
			{
				return "title";
			}
		}

		public ChartTextElement ChartTextElement
		{
			get
			{
				return this.chartTextElement.Element;
			}
			set
			{
				this.chartTextElement.Element = value;
			}
		}

		public OverlayElement OverlayElement
		{
			get
			{
				return this.overlay.Element;
			}
			set
			{
				this.overlay.Element = value;
			}
		}

		public void CopyPropertiesFrom(Title chartTitle)
		{
			base.CreateElement(this.chartTextElement);
			this.ChartTextElement.CopyPropertiesFrom(chartTitle);
			base.CreateElement(this.overlay);
			this.OverlayElement.Value = false;
		}

		public Title CreateChartTitle(IOpenXmlImportContext context)
		{
			Title result = null;
			if (this.ChartTextElement != null)
			{
				result = this.ChartTextElement.CreateChartTitle(context);
				base.ReleaseElement(this.chartTextElement);
			}
			if (this.OverlayElement != null)
			{
				base.ReleaseElement(this.overlay);
			}
			return result;
		}

		readonly OpenXmlChildElement<ChartTextElement> chartTextElement;

		readonly OpenXmlChildElement<OverlayElement> overlay;
	}
}
