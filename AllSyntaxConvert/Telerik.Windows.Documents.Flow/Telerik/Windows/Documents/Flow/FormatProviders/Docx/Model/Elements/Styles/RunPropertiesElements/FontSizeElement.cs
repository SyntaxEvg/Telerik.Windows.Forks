using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.RunPropertiesElements
{
	class FontSizeElement : DocxElementBase
	{
		public FontSizeElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.valueAttribute = base.RegisterAttribute<OpenXmlAttribute<double>>(new OpenXmlAttribute<double>("val", OpenXmlNamespaces.WordprocessingMLNamespace, true));
		}

		public override string ElementName
		{
			get
			{
				return "sz";
			}
		}

		public double Value
		{
			get
			{
				return this.valueAttribute.Value;
			}
			set
			{
				this.valueAttribute.Value = value;
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		readonly OpenXmlAttribute<double> valueAttribute;
	}
}
