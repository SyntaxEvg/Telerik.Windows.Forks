using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.RunPropertiesElements
{
	class BaselineAlignmentElement : DocxElementBase
	{
		public BaselineAlignmentElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.baselineAlignmentAttribute = new MappedOpenXmlAttribute<BaselineAlignment>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.BaselineAlignmentMapper, false);
			base.RegisterAttribute<MappedOpenXmlAttribute<BaselineAlignment>>(this.baselineAlignmentAttribute);
		}

		public override string ElementName
		{
			get
			{
				return "vertAlign";
			}
		}

		public BaselineAlignment Value
		{
			get
			{
				return this.baselineAlignmentAttribute.Value;
			}
			set
			{
				this.baselineAlignmentAttribute.Value = value;
			}
		}

		readonly MappedOpenXmlAttribute<BaselineAlignment> baselineAlignmentAttribute;
	}
}
