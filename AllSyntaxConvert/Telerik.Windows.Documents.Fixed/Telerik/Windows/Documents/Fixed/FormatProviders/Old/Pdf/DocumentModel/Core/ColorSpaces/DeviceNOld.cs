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
	class DeviceNOld : CachedColorSpaceOld, IMulticomponentColorSpace, IColorSpace
	{
		public DeviceNOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.attributes = new DeviceNAttributes(contentManager);
		}

		public ColorSpaceOld AlternateColorSpace { get; set; }

		public FunctionOld TintTransform { get; set; }

		public override Brush DefaultBrush
		{
			get
			{
				return this.AlternateColorSpace.DefaultBrush;
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
				return this.names.Count;
			}
		}

		public void Init(PdfArrayOld array)
		{
			this.names = array.GetElement<PdfArrayOld>(1);
			this.AlternateColorSpace = array.GetElement<ColorSpaceOld>(2, Converters.ColorSpaceConverter);
			this.TintTransform = array.GetElement<FunctionOld>(3, Converters.FunctionConverter);
			if (array.Count > 4)
			{
				PdfDictionaryOld element = array.GetElement<PdfDictionaryOld>(4);
				this.attributes.Load(element);
			}
			base.IsLoaded = true;
		}

		protected override Color GetColorOverride(object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			return this.AlternateColorSpace.GetColor(this.TintTransform.Execute(DeviceNOld.ToParams(pars)).ToParams<double>());
		}

		public override Color GetColor(byte[] bytes, int index)
		{
			Guard.ThrowExceptionIfNull<byte[]>(bytes, "bytes");
			object[] array = new object[this.ComponentCount];
			for (int i = 0; i < this.ComponentCount; i++)
			{
				array[i] = bytes[index + i];
			}
			return this.GetColor(array);
		}

		public override Brush GetBrush(PdfResourceOld resources, object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			return this.AlternateColorSpace.GetBrush(resources, this.TintTransform.Execute(DeviceNOld.ToParams(pars)).ToParams<double>());
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return DeviceN.GetDefaultDecodeArray(this);
		}

		protected override PixelContainer GetPixelsOverride(IImageDescriptor image, bool applyMask)
		{
			PixelContainer deviceNPixels = DeviceN.GetDeviceNPixels(image, this, applyMask);
			if (this.AlternateColorSpace != null)
			{
				this.AlternateColorSpace.Clear();
			}
			return deviceNPixels;
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

		readonly DeviceNAttributes attributes;

		PdfArrayOld names;
	}
}
