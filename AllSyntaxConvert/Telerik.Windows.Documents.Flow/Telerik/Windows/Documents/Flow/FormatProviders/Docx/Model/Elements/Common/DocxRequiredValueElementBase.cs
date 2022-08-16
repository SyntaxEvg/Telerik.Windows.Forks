using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common
{
	abstract class DocxRequiredValueElementBase<T> : DocxElementBase
	{
		public DocxRequiredValueElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<OpenXmlAttribute<T>>(new OpenXmlAttribute<T>("val", OpenXmlNamespaces.WordprocessingMLNamespace, true));
		}

		public T Value
		{
			get
			{
				return this.value.Value;
			}
			set
			{
				this.value.Value = value;
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		readonly OpenXmlAttribute<T> value;
	}
}
