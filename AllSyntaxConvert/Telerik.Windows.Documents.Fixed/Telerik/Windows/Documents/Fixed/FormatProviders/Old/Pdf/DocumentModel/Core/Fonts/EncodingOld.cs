using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	class EncodingOld : PdfObjectOld, ISimpleEncoding, IEncoding
	{
		public EncodingOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.isInitialized = false;
			this.baseEncoding = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "BaseEncoding"
			});
			this.differences = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Differences"
			});
		}

		public PdfNameOld BaseEncoding
		{
			get
			{
				return this.baseEncoding.GetValue();
			}
			set
			{
				this.baseEncoding.SetValue(value);
			}
		}

		public PdfArrayOld Differences
		{
			get
			{
				return this.differences.GetValue();
			}
			set
			{
				this.differences.SetValue(value);
			}
		}

		public string BaseEncodingName
		{
			get
			{
				if (this.BaseEncoding != null)
				{
					return this.BaseEncoding.Value;
				}
				return this.namedEncoding;
			}
		}

		public bool IsNamedEncoding
		{
			get
			{
				return this.BaseEncodingName != null && this.Differences == null;
			}
		}

		public string GetName(byte b)
		{
			this.Initialize();
			return this.names[(int)b];
		}

		public global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Core.Data.Tuple<char, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>> Encode(byte[] bytes)
		{
			if (bytes != null)
			{
				this.Initialize();
				foreach (byte b in bytes)
				{
					yield return new global::Telerik.Windows.Documents.Core.Data.Tuple<char, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>(global::Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding.AdobeGlyphList.GetUnicode(this.names[(int)b]), new global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld(b));
				}
			}
			yield break;
		}

		public void Load(PdfNameOld name)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(name, "name");
			PredefinedSimpleEncoding predefinedEncoding = PredefinedSimpleEncoding.GetPredefinedEncoding(name.Value);
			this.namedEncoding = ((predefinedEncoding != null) ? predefinedEncoding.Name : null);
			base.IsLoaded = true;
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			PdfNameOld pdfNameOld = indirectObject.Value as PdfNameOld;
			if (pdfNameOld != null)
			{
				this.Load(pdfNameOld);
			}
			base.Load(indirectObject);
		}

		void CalcDifferentsNames()
		{
			if (this.Differences == null)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < this.Differences.Count; i++)
			{
				object obj = this.Differences[i];
				if (obj is PdfIntOld)
				{
					num = (int)((byte)((PdfIntOld)obj).Value);
				}
				else if (obj is PdfNameOld)
				{
					if (num < 256)
					{
						PdfNameOld pdfNameOld = (PdfNameOld)obj;
						this.names[num] = pdfNameOld.Value;
					}
					num++;
				}
			}
		}

		void Initialize()
		{
			if (this.isInitialized)
			{
				return;
			}
			this.names = SimpleEncoding.GetNamesWithoutDifferences(this.BaseEncodingName);
			this.CalcDifferentsNames();
			this.isInitialized = true;
		}

		readonly InstantLoadProperty<PdfNameOld> baseEncoding;

		readonly LoadOnDemandProperty<PdfArrayOld> differences;

		string namedEncoding;

		string[] names;

		bool isInitialized;
	}
}
