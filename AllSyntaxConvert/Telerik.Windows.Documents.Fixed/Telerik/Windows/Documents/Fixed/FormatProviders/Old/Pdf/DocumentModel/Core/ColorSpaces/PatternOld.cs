using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	abstract class PatternOld : PdfObjectOld
	{
		public PatternOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.matrix = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Matrix"), PdfArrayOld.CreateMatrixIdentity(contentManager));
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

		public ColorSpaceOld UnderlyingColorSpace { get; set; }

		public static PatternOld CreatePattern(PdfContentManager contentManager, PdfDictionaryOld pdfDictionary)
		{
			PatternOld patternOld = PatternOld.CreatePatternInternal(contentManager, pdfDictionary);
			patternOld.Load(pdfDictionary);
			return patternOld;
		}

		public static PatternOld CreatePattern(PdfContentManager contentManager, PdfDataStream pdfDataStream)
		{
			PatternOld patternOld = PatternOld.CreatePatternInternal(contentManager, pdfDataStream.Dictionary);
			patternOld.Load(pdfDataStream);
			return patternOld;
		}

		public virtual void Load(PdfDataStream stream)
		{
			this.Load(stream.Dictionary);
		}

		public Brush CreateBrush(object[] pars)
		{
			return this.CreateBrushOverride(this.Matrix.ToMatrix(), pars);
		}

		protected abstract PatternBrush CreateBrushOverride(Matrix matrix, object[] pars);

		static PatternOld CreatePatternInternal(PdfContentManager contentManager, PdfDictionaryOld pdfDictionary)
		{
			int num;
			pdfDictionary.TryGetInt("PatternType", out num);
			PatternOld result;
			switch (num)
			{
			case 1:
				result = new TilingPatternOld(contentManager);
				break;
			case 2:
				result = new ShadingPatternOld(contentManager);
				break;
			default:
				throw new InvalidOperationException("Pattern type is not supported.");
			}
			return result;
		}

		readonly InstantLoadProperty<PdfArrayOld> matrix;
	}
}
