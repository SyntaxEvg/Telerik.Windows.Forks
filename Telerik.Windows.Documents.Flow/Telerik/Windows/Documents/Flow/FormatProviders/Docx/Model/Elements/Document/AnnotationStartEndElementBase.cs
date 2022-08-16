using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	abstract class AnnotationStartEndElementBase : DocumentElementBase
	{
		public AnnotationStartEndElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.idAttrubute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("id", OpenXmlNamespaces.WordprocessingMLNamespace, false));
		}

		public int Id
		{
			get
			{
				return this.idAttrubute.Value;
			}
			set
			{
				this.idAttrubute.Value = value;
			}
		}

		readonly IntOpenXmlAttribute idAttrubute;
	}
}
