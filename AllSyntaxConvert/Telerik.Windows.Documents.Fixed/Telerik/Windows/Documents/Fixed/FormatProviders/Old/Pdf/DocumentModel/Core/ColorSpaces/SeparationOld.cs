using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Functions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	class SeparationOld : CachedColorSpaceOld, IMulticomponentColorSpace, IColorSpace
	{
		public SeparationOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public ColorSpaceOld AlternateColorSpace { get; set; }

		public FunctionOld TintTransform { get; set; }

		public override Brush DefaultBrush
		{
			get
			{
				return new SolidColorBrush(Color.Transparent);
			}
		}

		public override ColorSpace Type
		{
			get
			{
				return this.AlternateColorSpace.Type;
			}
		}

		public override int ComponentCount
		{
			get
			{
				return 1;
			}
		}

		public void Init(PdfArrayOld array)
		{
			this.AlternateColorSpace = array.GetElement<ColorSpaceOld>(2, Converters.ColorSpaceConverter);
			this.TintTransform = array.GetElement<FunctionOld>(3, Converters.FunctionConverter);
			base.IsLoaded = true;
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return Separation.GetDefaultDecodeArray();
		}

		protected override Color GetColorOverride(object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			double[] inputValues = SeparationOld.ToParams(pars);
			double[] array = this.TintTransform.Execute(inputValues);
			object[] pars2 = array.ToParams<double>();
			return this.AlternateColorSpace.GetColor(pars2);
		}

		public override Color GetColor(byte[] bytes, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(bytes, "bytes");
			return this.GetColor(new object[] { bytes[offset] });
		}

		protected override PixelContainer GetPixelsOverride(IImageDescriptor image, bool applyMask)
		{
			PixelContainer separationPixels = Separation.GetSeparationPixels(image, this, applyMask);
			if (this.AlternateColorSpace != null)
			{
				this.AlternateColorSpace.Clear();
			}
			return separationPixels;
		}

		public override Brush GetBrush(PdfResourceOld resources, object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			double[] inputValues = SeparationOld.ToParams(pars);
			double[] array = this.TintTransform.Execute(inputValues);
			object[] pars2 = array.ToParams<double>();
			return this.AlternateColorSpace.GetBrush(resources, pars2);
		}

		static double[] ToParams(object[] pars)
		{
			double[] array = new double[pars.Length];
			for (int i = 0; i < pars.Length; i++)
			{
				Helper.UnboxDouble(pars[i], out array[i]);
			}
			return array;
		}

		Color IMulticomponentColorSpace.GetColor(double[] components)
		{
			object[] pars = Helper.BoxDoubleParameters(components);
			return this.GetColor(pars);
		}
	}
}
