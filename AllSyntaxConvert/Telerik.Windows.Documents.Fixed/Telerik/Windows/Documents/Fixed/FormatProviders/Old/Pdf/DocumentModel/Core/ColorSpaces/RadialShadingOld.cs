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
	[PdfClass(TypeName = "Shading", SubtypeProperty = "ShadingType", SubtypeValue = "3")]
	class RadialShadingOld : GradientShadingOld
	{
		public RadialShadingOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public override GradientBrush CreateBrush(Matrix transform, object[] pars)
		{
			double x;
			base.Coords.TryGetReal(0, out x);
			double y;
			base.Coords.TryGetReal(1, out y);
			double r;
			base.Coords.TryGetReal(2, out r);
			double x2;
			base.Coords.TryGetReal(3, out x2);
			double y2;
			base.Coords.TryGetReal(4, out y2);
			double r2;
			base.Coords.TryGetReal(5, out r2);
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
			RadialGradientBrush radialGradientBrush = new RadialGradientBrush(base.GetColor(num), base.GetColor(num2), new Point(x, y), new Point(x2, y2), r, r2, extendBefore, extendAfter, base.GetBackgroundColor(), transform, boundingBox);
			double step = GradientStopsCalculator.CalculateRadialGradientStopsMaximalStep(radialGradientBrush);
			radialGradientBrush.GradientStops = this.GetGradientStops(num, num2, step);
			return radialGradientBrush;
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
