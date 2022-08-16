using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class FormulaElement : DirectElementBase<string>
	{
		public FormulaElement()
		{
			this.formulaType = base.RegisterAttribute<string>("t", FormulaTypes.Normal, false);
			this.reference = base.RegisterAttribute<ConvertedOpenXmlAttribute<Ref>>(new ConvertedOpenXmlAttribute<Ref>("ref", Converters.RefConverter, false));
			this.sharedGroupIndex = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("si", false));
		}

		public override string ElementName
		{
			get
			{
				return "f";
			}
		}

		string FormulaType
		{
			set
			{
				this.formulaType.Value = value;
			}
		}

		Ref Reference
		{
			set
			{
				this.reference.Value = value;
			}
		}

		int SharedGroupIndex
		{
			set
			{
				this.sharedGroupIndex.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(string value)
		{
			base.InnerText = SpreadsheetCultureHelper.ClearFormulaValue(value);
		}

		protected override void WriteChildElementsOverride(string value)
		{
		}

		protected override void CopyAttributesOverride(ref string value)
		{
			value = base.InnerText;
		}

		protected override void ReadChildElementOverride(ElementBase element, ref string value)
		{
		}

		readonly OpenXmlAttribute<string> formulaType;

		readonly IntOpenXmlAttribute sharedGroupIndex;

		readonly ConvertedOpenXmlAttribute<Ref> reference;
	}
}
