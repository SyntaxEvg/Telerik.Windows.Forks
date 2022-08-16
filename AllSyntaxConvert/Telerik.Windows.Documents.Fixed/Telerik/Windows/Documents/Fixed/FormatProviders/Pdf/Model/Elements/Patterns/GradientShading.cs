using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Functions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns
{
	abstract class GradientShading : Shading
	{
		public GradientShading()
		{
			this.coordinates = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Coords"));
			this.domain = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Domain"), new PdfArray(new PdfPrimitive[]
			{
				new PdfReal(0.0),
				new PdfReal(1.0)
			}));
			this.function = base.RegisterReferenceProperty<FunctionObject>(new PdfPropertyDescriptor("Function"));
			this.extend = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Extend"), new PdfArray(new PdfPrimitive[]
			{
				new PdfBool(false),
				new PdfBool(false)
			}));
		}

		public PdfArray Coordinates
		{
			get
			{
				return this.coordinates.GetValue();
			}
			set
			{
				this.coordinates.SetValue(value);
			}
		}

		public PdfArray Domain
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

		public FunctionObject Function
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

		public PdfArray Extend
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

		public override ColorBase ToColor(PostScriptReader reader, IPdfContentImportContext context, Matrix matrix)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			Gradient gradient = this.ToColorOverride(reader, context, matrix);
			gradient.ExtendBefore = ((PdfBool)this.Extend[0]).Value;
			gradient.ExtendAfter = ((PdfBool)this.Extend[1]).Value;
			gradient.Background = base.GetBackgroundColor(reader, context);
			return gradient;
		}

		protected void InitializeGradientStops(PostScriptReader reader, IPdfContentImportContext context, Gradient gradient, Matrix matrix, double distance)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Gradient>(gradient, "gradient");
			FunctionBase functionBase = this.Function.ToFunction(reader, context.Owner);
			double t = this.Domain[0].ToReal();
			double t2 = this.Domain[1].ToReal();
			IEnumerable<double> gradientStopPositionCoeficients = GradientStopsCalculator.GetGradientStopPositionCoeficients(functionBase, matrix, t, t2, distance, base.ColorSpace.Name);
			foreach (double num in gradientStopPositionCoeficients)
			{
				double num2 = num;
				double[] array = functionBase.Execute(new double[] { num2 });
				PdfArray components = array.ToPdfArray();
				ColorObjectBase color = base.ColorSpace.GetColor(context, reader, components);
				ColorBase colorBase = color.ToColor(reader, context);
				SimpleColor color2 = (SimpleColor)colorBase;
				gradient.GradientStops.Add(new Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop(color2, num2));
			}
		}

		protected override void CopyPropertiesFromOverride(IPdfContentExportContext context, Gradient gradient)
		{
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Gradient>(gradient, "gradient");
			this.Extend = new PdfArray(new PdfPrimitive[0]);
			this.Extend.Add(gradient.ExtendBefore.ToPdfBool());
			this.Extend.Add(gradient.ExtendAfter.ToPdfBool());
			this.Function = GradientShading.CreateFunctionObject(context.Owner, gradient);
		}

		protected abstract Gradient ToColorOverride(PostScriptReader reader, IPdfContentImportContext context, Matrix matrix);

		static FunctionObject CreateFunctionObject(IPdfExportContext exportContext, Gradient gradient)
		{
			FunctionBase functionBase = GradientShading.CreateFunction(gradient);
			FunctionObject functionObject = FunctionObject.CreateFunction(new PdfInt((int)functionBase.FunctionType));
			functionObject.CopyPropertiesFrom(exportContext, functionBase);
			return functionObject;
		}

		static FunctionBase CreateFunction(Gradient gradient)
		{
			if (gradient.GradientStops.Count > 1)
			{
				return GradientShading.CreateStitchingFunction(gradient);
			}
			if (gradient.GradientStops.Count == 1)
			{
				Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop gradientStop = gradient.GradientStops.First<Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop>();
				return GradientShading.CreateGradientFunction(0.0, 1.0, gradientStop.Color, gradientStop.Color);
			}
			return GradientShading.CreateGradientFunction(0.0, 1.0, RgbColors.Transparent, RgbColors.Transparent);
		}

		static FunctionBase CreateStitchingFunction(Gradient gradient)
		{
			Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop[] array = (from s in gradient.GradientStops
				orderby s.Offset
				select s).ToArray<Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop>();
			double[] array2 = new double[] { 0.0, 1.0 };
			List<double> list = new List<double>();
			List<double> list2 = new List<double>();
			List<FunctionBase> list3 = new List<FunctionBase>();
			int num = array.Length - 1;
			int num2 = num - 1;
			if (array[0].Offset > 0.0)
			{
				list3.Add(GradientShading.CreateGradientFunction(0.0, array[0].Offset, array[0].Color, array[0].Color));
				list2.AddRange(new double[]
				{
					0.0,
					array[0].Offset
				});
				list.Add(array[0].Offset);
			}
			for (int i = 0; i < num; i++)
			{
				list3.Add(GradientShading.CreateGradientFunction(array[i].Offset, array[i + 1].Offset, array[i].Color, array[i + 1].Color));
				list2.AddRange(new double[]
				{
					array[i].Offset,
					array[i + 1].Offset
				});
				if (i < num2)
				{
					list.Add(array[i + 1].Offset);
				}
			}
			if (array[num].Offset < 1.0)
			{
				list3.Add(GradientShading.CreateGradientFunction(array[num].Offset, 1.0, array[num].Color, array[num].Color));
				list2.AddRange(new double[]
				{
					array[num].Offset,
					1.0
				});
				list.Add(array[num].Offset);
			}
			return new StitchingFunction(list3.ToArray(), list.ToArray(), list2.ToArray(), array2, null);
		}

		static FunctionBase CreateGradientFunction(double startOffset, double endOffset, SimpleColor startColor, SimpleColor endColor)
		{
			Guard.ThrowExceptionIfNull<SimpleColor>(startColor, "startColor");
			Guard.ThrowExceptionIfNull<SimpleColor>(endColor, "endColor");
			double[] array = new double[] { startOffset, endOffset };
			double[] range = new double[] { 0.0, 1.0, 0.0, 1.0, 0.0, 1.0 };
			int[] size = new int[] { 2 };
			int bitsPerSample = 8;
			return new SampledFunction(new int[][]
			{
				startColor.GetColorComponents(),
				endColor.GetColorComponents()
			}, size, bitsPerSample, array, range, null, null);
		}

		readonly DirectProperty<PdfArray> coordinates;

		readonly DirectProperty<PdfArray> domain;

		readonly ReferenceProperty<FunctionObject> function;

		readonly DirectProperty<PdfArray> extend;
	}
}
