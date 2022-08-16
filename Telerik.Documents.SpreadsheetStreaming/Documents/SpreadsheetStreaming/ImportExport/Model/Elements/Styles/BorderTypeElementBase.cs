using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	abstract class BorderTypeElementBase : DirectElementBase<SpreadBorder>
	{
		public BorderTypeElementBase()
		{
			this.style = base.RegisterAttribute<string>("style", BorderStylesMapper.None, false);
		}

		string Style
		{
			get
			{
				return this.style.Value;
			}
			set
			{
				this.style.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(SpreadBorder value)
		{
			if (value != null)
			{
				this.Style = BorderStylesMapper.GetBorderStyleName(value.Style);
			}
		}

		protected override void WriteChildElementsOverride(SpreadBorder value)
		{
			if (value != null && value.Style != SpreadBorderStyle.None && value.Color != null)
			{
				ColorElement colorElement = base.CreateChildElement<ColorElement>();
				colorElement.Write(value.Color);
			}
		}

		protected override void CopyAttributesOverride(ref SpreadBorder value)
		{
			if (this.style.HasValue)
			{
				value = value ?? new SpreadBorder();
				value.Style = BorderStylesMapper.GetBorderStyle(this.Style);
			}
		}

		protected override void ReadChildElementOverride(ElementBase element, ref SpreadBorder value)
		{
			ColorElement colorElement = element as ColorElement;
			if (colorElement != null)
			{
				value = value ?? new SpreadBorder();
				SpreadThemableColor color = null;
				colorElement.Read(ref color);
				value.Color = color;
			}
		}

		readonly OpenXmlAttribute<string> style;
	}
}
