using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects
{
	[PdfClass(TypeName = "XObject", SubtypeProperty = "Subtype", SubtypeValue = "Form")]
	class XForm : XObject
	{
		public XForm(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.matrix = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Matrix"
			}, PdfArrayOld.CreateMatrixIdentity(contentManager));
			this.resources = base.CreateLoadOnDemandProperty<PdfResourceOld>(new PdfPropertyDescriptor
			{
				Name = "Resources"
			}, Converters.PdfDictionaryToPdfObjectConverter);
			this.bbox = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "BBox",
				IsRequired = true
			});
		}

		public Rect Clip
		{
			get
			{
				return this.BBox.ToRect();
			}
		}

		public PdfArrayOld Matrix
		{
			get
			{
				return this.matrix.GetValue();
			}
			set
			{
				this.matrix.SetValue(value);
			}
		}

		public PdfResourceOld Resources
		{
			get
			{
				return this.resources.GetValue();
			}
			set
			{
				this.resources.SetValue(value);
			}
		}

		public PdfArrayOld BBox
		{
			get
			{
				return this.bbox.GetValue();
			}
			set
			{
				this.bbox.SetValue(value);
			}
		}

		readonly LoadOnDemandProperty<PdfResourceOld> resources;

		readonly LoadOnDemandProperty<PdfArrayOld> matrix;

		readonly LoadOnDemandProperty<PdfArrayOld> bbox;
	}
}
