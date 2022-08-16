using System;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils
{
	class DefaultValues
	{
		public DefaultValues(RadFlowDocument document)
		{
			this.run = new Run(document);
			this.paragraph = new Paragraph(document);
		}

		public CharacterProperties CharacterProperties
		{
			get
			{
				return this.run.Properties;
			}
		}

		public ParagraphProperties ParagraphProperties
		{
			get
			{
				return this.paragraph.Properties;
			}
		}

		readonly Run run;

		readonly Paragraph paragraph;
	}
}
