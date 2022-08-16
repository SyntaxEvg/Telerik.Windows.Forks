using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.ParagraphPropertiesElements
{
	class SpacingElement : DocxElementBase
	{
		public SpacingElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.afterAttribute = new ConvertedOpenXmlAttribute<double>("after", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, false);
			base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(this.afterAttribute);
			this.beforeAttribute = new ConvertedOpenXmlAttribute<double>("before", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, false);
			base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(this.beforeAttribute);
			this.automaticSpacingAfterAttribute = new BoolOpenXmlAttribute("afterAutospacing", OpenXmlNamespaces.WordprocessingMLNamespace);
			base.RegisterAttribute<BoolOpenXmlAttribute>(this.automaticSpacingAfterAttribute);
			this.automaticSpacingBeforeAttribute = new BoolOpenXmlAttribute("beforeAutospacing", OpenXmlNamespaces.WordprocessingMLNamespace);
			base.RegisterAttribute<BoolOpenXmlAttribute>(this.automaticSpacingBeforeAttribute);
			this.lineSpacingAttribute = new OpenXmlAttribute<double>("line", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			base.RegisterAttribute<OpenXmlAttribute<double>>(this.lineSpacingAttribute);
			this.lineSpacingTypeAttribute = new MappedOpenXmlAttribute<HeightType>("lineRule", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.HeightTypeMapper, false);
			base.RegisterAttribute<MappedOpenXmlAttribute<HeightType>>(this.lineSpacingTypeAttribute);
		}

		public override string ElementName
		{
			get
			{
				return "spacing";
			}
		}

		public void FillAttributes(ParagraphProperties paragraphProperties)
		{
			if (paragraphProperties.SpacingAfter.HasLocalValue)
			{
				double value = paragraphProperties.SpacingAfter.LocalValue.Value;
				this.afterAttribute.Value = value;
			}
			if (paragraphProperties.AutomaticSpacingAfter.HasLocalValue)
			{
				this.automaticSpacingAfterAttribute.Value = paragraphProperties.AutomaticSpacingAfter.LocalValue.Value;
			}
			if (paragraphProperties.SpacingBefore.HasLocalValue)
			{
				double value2 = paragraphProperties.SpacingBefore.LocalValue.Value;
				this.beforeAttribute.Value = value2;
			}
			if (paragraphProperties.AutomaticSpacingBefore.HasLocalValue)
			{
				this.automaticSpacingBeforeAttribute.Value = paragraphProperties.AutomaticSpacingBefore.LocalValue.Value;
			}
			if (paragraphProperties.LineSpacing.HasLocalValue)
			{
				double num = paragraphProperties.LineSpacing.LocalValue.Value;
				if (paragraphProperties.LineSpacingType.GetActualValue().Value == HeightType.Auto)
				{
					num *= 240.0;
				}
				else
				{
					num = Unit.DipToTwip(num);
				}
				this.lineSpacingAttribute.Value = num;
			}
			if (paragraphProperties.LineSpacingType.HasLocalValue)
			{
				this.lineSpacingTypeAttribute.Value = paragraphProperties.LineSpacingType.LocalValue.Value;
			}
		}

		public void ReadAttributes(ParagraphProperties paragraphProperties)
		{
			if (this.afterAttribute.HasValue)
			{
				double value = this.afterAttribute.Value;
				paragraphProperties.SpacingAfter.LocalValue = new double?(value);
			}
			if (this.automaticSpacingAfterAttribute.HasValue)
			{
				paragraphProperties.AutomaticSpacingAfter.LocalValue = new bool?(this.automaticSpacingAfterAttribute.Value);
			}
			if (this.beforeAttribute.HasValue)
			{
				double value2 = this.beforeAttribute.Value;
				paragraphProperties.SpacingBefore.LocalValue = new double?(value2);
			}
			if (this.automaticSpacingBeforeAttribute.HasValue)
			{
				paragraphProperties.AutomaticSpacingBefore.LocalValue = new bool?(this.automaticSpacingBeforeAttribute.Value);
			}
			if (this.lineSpacingAttribute.HasValue)
			{
				double num = this.lineSpacingAttribute.Value;
				if (!this.lineSpacingTypeAttribute.HasValue || (this.lineSpacingTypeAttribute.HasValue && this.lineSpacingTypeAttribute.Value == HeightType.Auto))
				{
					num /= 240.0;
				}
				else
				{
					num = Unit.TwipToDip(num);
				}
				paragraphProperties.LineSpacing.LocalValue = new double?(num);
			}
			if (this.lineSpacingTypeAttribute.HasValue)
			{
				paragraphProperties.LineSpacingType.LocalValue = new HeightType?(this.lineSpacingTypeAttribute.Value);
			}
		}

		readonly ConvertedOpenXmlAttribute<double> afterAttribute;

		readonly ConvertedOpenXmlAttribute<double> beforeAttribute;

		readonly BoolOpenXmlAttribute automaticSpacingAfterAttribute;

		readonly BoolOpenXmlAttribute automaticSpacingBeforeAttribute;

		readonly OpenXmlAttribute<double> lineSpacingAttribute;

		readonly MappedOpenXmlAttribute<HeightType> lineSpacingTypeAttribute;
	}
}
