using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Forms
{
	class FormFieldsCollectionOld : PdfObjectOld, IEnumerable<FormFieldNodeOld>, IEnumerable
	{
		public FormFieldsCollectionOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.fields = new List<FormFieldNodeOld>();
		}

		public void Add(FormFieldNodeOld node)
		{
			this.fields.Add(node);
		}

		public IEnumerator<FormFieldNodeOld> GetEnumerator()
		{
			return this.fields.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.fields.GetEnumerator();
		}

		readonly List<FormFieldNodeOld> fields;
	}
}
