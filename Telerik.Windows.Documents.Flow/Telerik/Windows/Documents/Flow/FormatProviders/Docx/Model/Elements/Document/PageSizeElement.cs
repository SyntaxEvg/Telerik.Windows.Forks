using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class PageSizeElement : DocxElementBase
	{
		public PageSizeElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.heightAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("h", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, false));
			this.widthAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("w", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, false));
			this.orientationAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<PageOrientation>>(new MappedOpenXmlAttribute<PageOrientation>("orient", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.PageOrientationMapper, false));
		}

		public override string ElementName
		{
			get
			{
				return "pgSz";
			}
		}

		public double Width
		{
			get
			{
				return this.widthAttribute.Value;
			}
			set
			{
				this.widthAttribute.Value = value;
			}
		}

		public double Height
		{
			get
			{
				return this.heightAttribute.Value;
			}
			set
			{
				this.heightAttribute.Value = value;
			}
		}

		public PageOrientation PageOrientation
		{
			get
			{
				return this.orientationAttribute.Value;
			}
			set
			{
				this.orientationAttribute.Value = value;
			}
		}

		readonly ConvertedOpenXmlAttribute<double> heightAttribute;

		readonly ConvertedOpenXmlAttribute<double> widthAttribute;

		readonly MappedOpenXmlAttribute<PageOrientation> orientationAttribute;
	}
}
