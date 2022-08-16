using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet.Styles;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class PatternFillElement : DirectElementBase<ISpreadFill>
	{
		public PatternFillElement()
		{
			this.patternType = base.RegisterAttribute<string>("patternType", PatternTypesMapper.None, false);
		}

		public override string ElementName
		{
			get
			{
				return "patternFill";
			}
		}

		string PatternType
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

		protected override void InitializeAttributesOverride(ISpreadFill value)
		{
			NoneFill noneFill = value as NoneFill;
			if (noneFill != null)
			{
				this.PatternType = PatternTypesMapper.None;
			}
			SpreadPatternFill spreadPatternFill = value as SpreadPatternFill;
			if (spreadPatternFill != null)
			{
				this.PatternType = PatternTypesMapper.GetPatternTypeName(spreadPatternFill.PatternType);
			}
		}

		protected override void WriteChildElementsOverride(ISpreadFill value)
		{
			SpreadPatternFill spreadPatternFill = value as SpreadPatternFill;
			if (spreadPatternFill == null)
			{
				return;
			}
			SpreadThemableColor spreadThemableColor = spreadPatternFill.BackgroundColor;
			SpreadThemableColor spreadThemableColor2 = spreadPatternFill.PatternColor;
			if (spreadPatternFill.PatternType == SpreadPatternType.Solid)
			{
				SpreadThemableColor spreadThemableColor3 = spreadThemableColor;
				spreadThemableColor = spreadThemableColor2;
				spreadThemableColor2 = spreadThemableColor3;
			}
			if (spreadThemableColor2 != null)
			{
				ForegroundColorElement foregroundColorElement = base.CreateChildElement<ForegroundColorElement>();
				foregroundColorElement.Write(spreadThemableColor2);
			}
			if (spreadThemableColor != null)
			{
				BackgroundColorElement backgroundColorElement = base.CreateChildElement<BackgroundColorElement>();
				backgroundColorElement.Write(spreadThemableColor);
			}
		}

		protected override void CopyAttributesOverride(ref ISpreadFill value)
		{
			if (this.PatternType == PatternTypesMapper.None)
			{
				value = NoneFill.Instance;
				return;
			}
			SpreadPatternType spreadPatternType = PatternTypesMapper.GetPatternType(this.PatternType);
			if (spreadPatternType == SpreadPatternType.Solid)
			{
				value = new SpreadPatternFill(spreadPatternType, this.backgroundColor, this.foregroundColor);
				return;
			}
			value = new SpreadPatternFill(spreadPatternType, this.foregroundColor, this.backgroundColor);
		}

		protected override void ReadChildElementOverride(ElementBase element, ref ISpreadFill value)
		{
			string elementName;
			if ((elementName = element.ElementName) != null)
			{
				if (elementName == "fgColor")
				{
					ForegroundColorElement foregroundColorElement = element as ForegroundColorElement;
					foregroundColorElement.Read(ref this.foregroundColor);
					return;
				}
				if (elementName == "bgColor")
				{
					BackgroundColorElement backgroundColorElement = element as BackgroundColorElement;
					backgroundColorElement.Read(ref this.backgroundColor);
					return;
				}
			}
			throw new InvalidOperationException();
		}

		readonly OpenXmlAttribute<string> patternType;

		SpreadThemableColor foregroundColor;

		SpreadThemableColor backgroundColor;
	}
}
