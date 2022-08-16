using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class PageMarginsElement : DocxElementBase
	{
		public PageMarginsElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.topAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("top", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, true));
			this.rightAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("right", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, true));
			this.bottomAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("bottom", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, true));
			this.leftAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("left", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, true));
			this.headerAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("header", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, DocumentDefaultStyleSettings.SectionHeaderTopMargin, true));
			this.footerAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("footer", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, DocumentDefaultStyleSettings.SectionFooterBottomMargin, true));
			this.gutterAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("gutter", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, true));
		}

		public override string ElementName
		{
			get
			{
				return "pgMar";
			}
		}

		public double Top
		{
			get
			{
				return this.topAttribute.Value;
			}
			set
			{
				this.topAttribute.Value = value;
			}
		}

		public double Right
		{
			get
			{
				return this.rightAttribute.Value;
			}
			set
			{
				this.rightAttribute.Value = value;
			}
		}

		public double Bottom
		{
			get
			{
				return this.bottomAttribute.Value;
			}
			set
			{
				this.bottomAttribute.Value = value;
			}
		}

		public double Left
		{
			get
			{
				return this.leftAttribute.Value;
			}
			set
			{
				this.leftAttribute.Value = value;
			}
		}

		public double Header
		{
			get
			{
				return this.headerAttribute.Value;
			}
			set
			{
				this.headerAttribute.Value = value;
			}
		}

		public double Footer
		{
			get
			{
				return this.footerAttribute.Value;
			}
			set
			{
				this.footerAttribute.Value = value;
			}
		}

		public double Gutter
		{
			get
			{
				return this.gutterAttribute.Value;
			}
			set
			{
				this.gutterAttribute.Value = value;
			}
		}

		readonly ConvertedOpenXmlAttribute<double> topAttribute;

		readonly ConvertedOpenXmlAttribute<double> rightAttribute;

		readonly ConvertedOpenXmlAttribute<double> bottomAttribute;

		readonly ConvertedOpenXmlAttribute<double> leftAttribute;

		readonly ConvertedOpenXmlAttribute<double> gutterAttribute;

		readonly ConvertedOpenXmlAttribute<double> headerAttribute;

		readonly ConvertedOpenXmlAttribute<double> footerAttribute;
	}
}
