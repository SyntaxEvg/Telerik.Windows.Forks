using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class TextElement : DocumentElementBase
	{
		public TextElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			base.RegisterAttribute<SpaceAttribute>(new SpaceAttribute(this));
		}

		public override string ElementName
		{
			get
			{
				return "t";
			}
		}
	}
}
