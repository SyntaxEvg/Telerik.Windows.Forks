using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TableRowPropertiesElements
{
	class TableRowHeightElement : DocxElementBase
	{
		public TableRowHeightElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.heightAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("val", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.SignedTwipsMeasureToDipConverter, false));
			this.heightTypeAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<HeightType>>(new MappedOpenXmlAttribute<HeightType>("hRule", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.HeightTypeMapper, false));
		}

		public override string ElementName
		{
			get
			{
				return "trHeight";
			}
		}

		public TableRowHeight Value
		{
			get
			{
				double value = this.heightAttribute.Value;
				return new TableRowHeight(this.heightTypeAttribute.Value, value);
			}
			set
			{
				this.heightAttribute.Value = value.Value;
				this.heightTypeAttribute.Value = value.Type;
			}
		}

		readonly ConvertedOpenXmlAttribute<double> heightAttribute;

		readonly MappedOpenXmlAttribute<HeightType> heightTypeAttribute;
	}
}
