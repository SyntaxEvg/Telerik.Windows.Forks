using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Functions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	abstract class GradientShadingOld : ShadingOld
	{
		public GradientShadingOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.coords = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Coords", true));
			this.domain = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Domain"), new PdfArrayOld(base.ContentManager, new object[] { 0.0, 1.0 }));
			this.function = base.CreateLoadOnDemandProperty<FunctionOld>(new PdfPropertyDescriptor("Function", true), Converters.FunctionConverter);
			this.extend = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Extend"), new PdfArrayOld(base.ContentManager, new object[] { false, false }));
		}

		public PdfArrayOld Coords
		{
			get
			{
				return this.coords.GetValue();
			}
			set
			{
				this.coords.SetValue(value);
			}
		}

		public PdfArrayOld Domain
		{
			get
			{
				return this.domain.GetValue();
			}
			set
			{
				this.domain.SetValue(value);
			}
		}

		public FunctionOld Function
		{
			get
			{
				return this.function.GetValue();
			}
			set
			{
				this.function.SetValue(value);
			}
		}

		public PdfArrayOld Extend
		{
			get
			{
				return this.extend.GetValue();
			}
			set
			{
				this.extend.SetValue(value);
			}
		}

		protected Color GetColor(double d)
		{
			double[] array = this.Function.Execute(new double[] { d });
			return base.ColorSpace.GetColor(array.ToParams<double>());
		}

		readonly InstantLoadProperty<PdfArrayOld> coords;

		readonly InstantLoadProperty<PdfArrayOld> domain;

		readonly LoadOnDemandProperty<FunctionOld> function;

		readonly InstantLoadProperty<PdfArrayOld> extend;
	}
}
