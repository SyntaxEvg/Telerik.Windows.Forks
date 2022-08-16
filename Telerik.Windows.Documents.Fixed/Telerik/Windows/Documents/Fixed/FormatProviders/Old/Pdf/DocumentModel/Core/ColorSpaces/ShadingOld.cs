using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	abstract class ShadingOld : PdfObjectOld
	{
		public ShadingOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.colorSpace = base.CreateInstantLoadProperty<ColorSpaceOld>(new PdfPropertyDescriptor("ColorSpace", true), Converters.ColorSpaceConverter);
			this.background = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Background"));
			this.boundingBox = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("BoundingBox"));
		}

		public ColorSpaceOld ColorSpace
		{
			get
			{
				return this.colorSpace.GetValue();
			}
			set
			{
				this.colorSpace.SetValue(value);
			}
		}

		public PdfArrayOld Background
		{
			get
			{
				return this.background.GetValue();
			}
			set
			{
				this.background.SetValue(value);
			}
		}

		public PdfArrayOld BoundingBox
		{
			get
			{
				return this.boundingBox.GetValue();
			}
			set
			{
				this.boundingBox.SetValue(value);
			}
		}

		public static ShadingOld CreateShading(PdfContentManager contentManager, PdfDictionaryOld dict)
		{
			ShadingOld shadingOld = ShadingOld.CreateShadingInternal(contentManager, dict);
			shadingOld.Load(dict);
			return shadingOld;
		}

		public static ShadingOld CreateShading(PdfContentManager contentManager, PdfDataStream pdfStream)
		{
			ShadingOld shadingOld = ShadingOld.CreateShadingInternal(contentManager, pdfStream.Dictionary);
			shadingOld.Load(pdfStream);
			return shadingOld;
		}

		public static ShadingOld CreateShadingInternal(PdfContentManager contentManager, PdfDictionaryOld dict)
		{
			int shadingType;
			dict.TryGetInt("ShadingType", out shadingType);
			ShadingOld result;
			switch (shadingType)
			{
			case 2:
				result = new AxialShadingOld(contentManager);
				break;
			case 3:
				result = new RadialShadingOld(contentManager);
				break;
			default:
				throw new NotSupportedShadingTypeException(shadingType);
			}
			return result;
		}

		public virtual void Load(PdfDataStream stream)
		{
			this.Load(stream.Dictionary);
		}

		public abstract GradientBrush CreateBrush(Matrix transform, object[] pars);

		protected Color? GetBackgroundColor()
		{
			if (this.Background == null)
			{
				return null;
			}
			return new Color?(this.ColorSpace.GetColor(this.Background.ToArray()));
		}

		public const int AxialShadingType = 2;

		public const int RadialShadingType = 3;

		readonly InstantLoadProperty<ColorSpaceOld> colorSpace;

		readonly InstantLoadProperty<PdfArrayOld> background;

		readonly InstantLoadProperty<PdfArrayOld> boundingBox;
	}
}
