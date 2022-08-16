using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet.Styles;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class FillElement : DirectElementBase<ISpreadFill>
	{
		public override string ElementName
		{
			get
			{
				return "fill";
			}
		}

		protected override void InitializeAttributesOverride(ISpreadFill value)
		{
		}

		protected override void WriteChildElementsOverride(ISpreadFill value)
		{
			SpreadPatternFill spreadPatternFill = value as SpreadPatternFill;
			if (spreadPatternFill != null)
			{
				PatternFillElement patternFillElement = base.CreateChildElement<PatternFillElement>();
				patternFillElement.Write(spreadPatternFill);
				return;
			}
			SpreadGradientFill spreadGradientFill = value as SpreadGradientFill;
			if (spreadGradientFill != null)
			{
				GradientFillElement gradientFillElement = base.CreateChildElement<GradientFillElement>();
				gradientFillElement.Write(spreadGradientFill);
				return;
			}
			NoneFill noneFill = value as NoneFill;
			if (noneFill != null)
			{
				PatternFillElement patternFillElement2 = base.CreateChildElement<PatternFillElement>();
				patternFillElement2.Write(noneFill);
			}
		}

		protected override void CopyAttributesOverride(ref ISpreadFill value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref ISpreadFill value)
		{
			string elementName;
			if ((elementName = element.ElementName) != null)
			{
				if (elementName == "patternFill")
				{
					PatternFillElement patternFillElement = element as PatternFillElement;
					patternFillElement.Read(ref value);
					return;
				}
				if (elementName == "gradientFill")
				{
					GradientFillElement gradientFillElement = element as GradientFillElement;
					gradientFillElement.Read(ref value);
					return;
				}
			}
			throw new InvalidOperationException();
		}
	}
}
