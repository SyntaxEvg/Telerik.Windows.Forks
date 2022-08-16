using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet.Styles
{
	class StopElement : DirectElementBase<GradientStop>
	{
		public StopElement()
		{
			this.position = base.RegisterAttribute<double>("position", true);
		}

		public override string ElementName
		{
			get
			{
				return "stop";
			}
		}

		double Position
		{
			get
			{
				return this.position.Value;
			}
			set
			{
				this.position.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(GradientStop value)
		{
			this.Position = value.Position;
		}

		protected override void WriteChildElementsOverride(GradientStop value)
		{
			ColorElement colorElement = base.CreateChildElement<ColorElement>();
			colorElement.Write(value.ThemableColor);
		}

		protected override void CopyAttributesOverride(ref GradientStop value)
		{
			value.Position = this.Position;
		}

		protected override void ReadChildElementOverride(ElementBase element, ref GradientStop value)
		{
			string elementName;
			if ((elementName = element.ElementName) != null && elementName == "color")
			{
				ColorElement colorElement = (ColorElement)element;
				SpreadThemableColor themableColor = null;
				colorElement.Read(ref themableColor);
				value.ThemableColor = themableColor;
				return;
			}
			throw new InvalidOperationException();
		}

		readonly OpenXmlAttribute<double> position;
	}
}
