using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts
{
	abstract class HeaderFooterPartBase : DocxPartBase
	{
		public HeaderFooterPartBase(DocxPartsManager partsManager, string partName, int partNumber)
			: base(partsManager, string.Format(partName, partNumber))
		{
		}

		public HeaderFooterPartBase(DocxPartsManager partsManager, string name)
			: base(partsManager, name)
		{
		}

		public override int Level
		{
			get
			{
				return 4;
			}
		}

		public string RelationshipId { get; protected set; }
	}
}
