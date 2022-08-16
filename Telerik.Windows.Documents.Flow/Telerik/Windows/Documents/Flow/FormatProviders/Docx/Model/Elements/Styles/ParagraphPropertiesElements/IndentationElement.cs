using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.ParagraphPropertiesElements
{
	class IndentationElement : DocxElementBase
	{
		public IndentationElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.firstLineIndentAttribute = new ConvertedOpenXmlAttribute<double>("firstLine", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, false);
			base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(this.firstLineIndentAttribute);
			this.hangingIndentIndentAttribute = new ConvertedOpenXmlAttribute<double>("hanging", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, false);
			base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(this.hangingIndentIndentAttribute);
			this.leftIndentIndentAttribute = new ConvertedOpenXmlAttribute<double>("left", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, false);
			base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(this.leftIndentIndentAttribute);
			this.rightIndentIndentAttribute = new ConvertedOpenXmlAttribute<double>("right", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, false);
			base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(this.rightIndentIndentAttribute);
		}

		public override string ElementName
		{
			get
			{
				return "ind";
			}
		}

		public void FillAttributes(ParagraphProperties paragraphProperties)
		{
			if (paragraphProperties.FirstLineIndent.HasLocalValue)
			{
				double value = paragraphProperties.FirstLineIndent.LocalValue.Value;
				this.firstLineIndentAttribute.Value = value;
			}
			if (paragraphProperties.HangingIndent.HasLocalValue)
			{
				double value2 = paragraphProperties.HangingIndent.LocalValue.Value;
				this.hangingIndentIndentAttribute.Value = value2;
			}
			if (paragraphProperties.LeftIndent.HasLocalValue)
			{
				double value3 = paragraphProperties.LeftIndent.LocalValue.Value;
				this.leftIndentIndentAttribute.Value = value3;
			}
			if (paragraphProperties.RightIndent.HasLocalValue)
			{
				double value4 = paragraphProperties.RightIndent.LocalValue.Value;
				this.rightIndentIndentAttribute.Value = value4;
			}
		}

		public void ReadAttributes(ParagraphProperties paragraphProperties)
		{
			if (this.firstLineIndentAttribute.HasValue)
			{
				double value = this.firstLineIndentAttribute.Value;
				paragraphProperties.FirstLineIndent.LocalValue = new double?(value);
			}
			if (this.hangingIndentIndentAttribute.HasValue)
			{
				double value2 = this.hangingIndentIndentAttribute.Value;
				paragraphProperties.HangingIndent.LocalValue = new double?(value2);
			}
			if (this.leftIndentIndentAttribute.HasValue)
			{
				double value3 = this.leftIndentIndentAttribute.Value;
				paragraphProperties.LeftIndent.LocalValue = new double?(value3);
			}
			if (this.rightIndentIndentAttribute.HasValue)
			{
				double value4 = this.rightIndentIndentAttribute.Value;
				paragraphProperties.RightIndent.LocalValue = new double?(value4);
			}
		}

		readonly ConvertedOpenXmlAttribute<double> firstLineIndentAttribute;

		readonly ConvertedOpenXmlAttribute<double> hangingIndentIndentAttribute;

		readonly ConvertedOpenXmlAttribute<double> leftIndentIndentAttribute;

		readonly ConvertedOpenXmlAttribute<double> rightIndentIndentAttribute;
	}
}
