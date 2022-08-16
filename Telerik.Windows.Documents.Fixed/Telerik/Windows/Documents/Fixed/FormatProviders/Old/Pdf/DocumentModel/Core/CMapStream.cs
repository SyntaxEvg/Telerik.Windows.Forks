using System;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	[PdfClass(TypeName = "CMap")]
	class CMapStream : PdfStreamOld
	{
		public CMapStream(PdfContentManager contentManager)
			: this(contentManager, false, false)
		{
		}

		public CMapStream(PdfContentManager contentManager, bool isIdentityH = false, bool isIdentityV = false)
			: base(contentManager)
		{
			this.useCMap = base.CreateInstantLoadProperty<CMapStream>(new PdfPropertyDescriptor
			{
				Name = "UseCMap"
			}, Converters.CMapStreamConverter);
			this.isIdentityH = isIdentityH;
			this.isIdentityV = isIdentityV;
		}

		public bool IsIdentityV
		{
			get
			{
				return this.isIdentityV;
			}
		}

		public bool IsIdentityH
		{
			get
			{
				return this.isIdentityH;
			}
		}

		public CMapStream UseCMap
		{
			get
			{
				return this.useCMap.GetValue();
			}
			set
			{
				this.useCMap.SetValue(value);
			}
		}

		public static CMapStream CreateCMap(PdfContentManager contentManager, PdfNameOld name)
		{
			string value;
			if ((value = name.Value) != null)
			{
				if (value == "Identity-H")
				{
					return new CMapStream(contentManager, true, false);
				}
				if (value == "Identity-V")
				{
					return new CMapStream(contentManager, false, true);
				}
			}
			throw new NotSupportedPredefinedCMapException(name.Value);
		}

		readonly InstantLoadProperty<CMapStream> useCMap;

		readonly bool isIdentityH;

		readonly bool isIdentityV;
	}
}
