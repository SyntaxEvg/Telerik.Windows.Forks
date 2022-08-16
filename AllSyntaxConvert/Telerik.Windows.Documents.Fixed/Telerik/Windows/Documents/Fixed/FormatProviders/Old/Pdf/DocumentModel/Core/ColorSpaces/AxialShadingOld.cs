using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns;
using Telerik.Windows.Documents.Fixed.Model.Internal;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	[PdfClass(TypeName = "Shading", SubtypeProperty = "ShadingType", SubtypeValue = "2")]
	class AxialShadingOld : GradientShadingOld
	{
		public AxialShadingOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public override GradientBrush CreateBrush(Matrix transform, object[] pars)
		{
			double x;
			base.Coords.TryGetReal(0, out x);
			double y;
			base.Coords.TryGetReal(1, out y);
			double x2;
			base.Coords.TryGetReal(2, out x2);
			double y2;
			base.Coords.TryGetReal(3, out y2);
			double num;
			base.Domain.TryGetReal(0, out num);
			double num2;
			base.Domain.TryGetReal(1, out num2);
			bool extendBefore;
			base.Extend.TryGetBool(0, out extendBefore);
			bool extendAfter;
			base.Extend.TryGetBool(1, out extendAfter);
			Rect? boundingBox = null;
			if (base.BoundingBox != null)
			{
				boundingBox = new Rect?(base.BoundingBox.ToRect());
			}
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.GetColor(num), base.GetColor(num2), new Point(x, y), new Point(x2, y2), extendBefore, extendAfter, base.GetBackgroundColor(), transform, boundingBox);
			double step = GradientStopsCalculator.CalculateLinearGradientStopsMaximalStep(linearGradientBrush);
			linearGradientBrush.GradientStops = this.GetGradientStops(num, num2, step);
			return linearGradientBrush;
		}

		GradientStop[] GetGradientStops(double t0, double t1, double step)
		{
			List<GradientStop> list = new List<GradientStop>();
			for (double num = t0 + step; num < t1; num += step)
			{
				list.Add(new GradientStop(base.GetColor(num), num));
			}
			return list.ToArray();
		}
	}
}
