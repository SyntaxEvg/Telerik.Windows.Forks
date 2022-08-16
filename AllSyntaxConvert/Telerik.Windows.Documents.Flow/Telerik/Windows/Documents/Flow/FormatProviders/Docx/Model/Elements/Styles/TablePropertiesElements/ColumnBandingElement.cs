using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TablePropertiesElements
{
	class ColumnBandingElement : DecimalNumberBase
	{
		public ColumnBandingElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "tblStyleColBandSize";
			}
		}
	}
}
