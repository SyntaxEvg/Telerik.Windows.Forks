using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure
{
	class PdfResource : PdfObject
	{
		public PdfResource()
		{
			this.fonts = base.RegisterReferenceProperty<PdfDictionary>(new PdfPropertyDescriptor("Font"));
			this.xObjects = base.RegisterReferenceProperty<PdfDictionary>(new PdfPropertyDescriptor("XObject"));
			this.patterns = base.RegisterReferenceProperty<PdfDictionary>(new PdfPropertyDescriptor("Pattern"));
			this.extGState = base.RegisterReferenceProperty<PdfDictionary>(new PdfPropertyDescriptor("ExtGState"));
			this.colorSpaces = base.RegisterReferenceProperty<PdfDictionary>(new PdfPropertyDescriptor("ColorSpace"));
			this.shadings = base.RegisterReferenceProperty<PdfDictionary>(new PdfPropertyDescriptor("Shading"));
			this.markedContentProperties = base.RegisterReferenceProperty<PdfDictionary>(new PdfPropertyDescriptor("Properties"));
			this.procSets = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("ProcSet"));
		}

		public PdfDictionary Fonts
		{
			get
			{
				return this.fonts.GetValue();
			}
			set
			{
				this.fonts.SetValue(value);
			}
		}

		public PdfDictionary XObjects
		{
			get
			{
				return this.xObjects.GetValue();
			}
			set
			{
				this.xObjects.SetValue(value);
			}
		}

		public PdfDictionary Patterns
		{
			get
			{
				return this.patterns.GetValue();
			}
			set
			{
				this.patterns.SetValue(value);
			}
		}

		public PdfDictionary ExtGState
		{
			get
			{
				return this.extGState.GetValue();
			}
			set
			{
				this.extGState.SetValue(value);
			}
		}

		public PdfDictionary ColorSpaces
		{
			get
			{
				return this.colorSpaces.GetValue();
			}
			set
			{
				this.colorSpaces.SetValue(value);
			}
		}

		public PdfDictionary Shadings
		{
			get
			{
				return this.shadings.GetValue();
			}
			set
			{
				this.shadings.SetValue(value);
			}
		}

		public PdfDictionary MarkedContentProperties
		{
			get
			{
				return this.markedContentProperties.GetValue();
			}
			set
			{
				this.markedContentProperties.SetValue(value);
			}
		}

		public PdfArray ProcSets
		{
			get
			{
				return this.procSets.GetValue();
			}
			set
			{
				this.procSets.SetValue(value);
			}
		}

		readonly ReferenceProperty<PdfDictionary> fonts;

		readonly ReferenceProperty<PdfDictionary> xObjects;

		readonly ReferenceProperty<PdfDictionary> patterns;

		readonly ReferenceProperty<PdfDictionary> extGState;

		readonly ReferenceProperty<PdfDictionary> colorSpaces;

		readonly ReferenceProperty<PdfDictionary> shadings;

		readonly ReferenceProperty<PdfDictionary> markedContentProperties;

		readonly ReferenceProperty<PdfArray> procSets;
	}
}
