using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class SignatureReferenceCollection : PdfPrimitive, IEnumerable<SignatureReferenceObject>, IEnumerable
	{
		public SignatureReferenceCollection(IEnumerable<SignatureReferenceObject> collection)
		{
			this.referenceElements.AddRange(collection);
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.Array;
			}
		}

		public void Add(SignatureReferenceObject node)
		{
			this.referenceElements.Add(node);
		}

		public void AddRange(IEnumerable<SignatureReferenceObject> collection)
		{
			this.referenceElements.AddRange(collection);
		}

		public IEnumerator<SignatureReferenceObject> GetEnumerator()
		{
			return this.referenceElements.GetEnumerator();
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.referenceElements.GetEnumerator();
		}

		List<SignatureReferenceObject> referenceElements = new List<SignatureReferenceObject>();
	}
}
