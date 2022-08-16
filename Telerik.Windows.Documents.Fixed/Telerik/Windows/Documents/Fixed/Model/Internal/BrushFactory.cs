using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	static class BrushFactory
	{
		internal static SolidColorBrush GetSolidColorBrush(Color color)
		{
			Color color2 = new Color(color.A, color.R, color.G, color.B);
			return BrushFactory.GetSolidColorBrush(color2);
		}

		internal static Brush GetBrushWithDifferentAlpha(Brush brush, byte alpha)
		{
			Brush brush2 = brush.Clone();
			brush2.AlphaConstant = alpha;
			SolidColorBrush solidColorBrush = brush2 as SolidColorBrush;
			if (solidColorBrush != null)
			{
				Color color = Color.FromColor(alpha, solidColorBrush.Color);
				brush2 = BrushFactory.GetSolidColorBrush(color);
			}
			return brush2;
		}

		internal static SolidColorBrush GetSolidColorBrush(Color color)
		{
			SolidColorBrush solidColorBrush;
			lock (BrushFactory.lockObject)
			{
				if (!BrushFactory.solidColorBrushesStore.TryGetValue(color, out solidColorBrush))
				{
					solidColorBrush = new SolidColorBrush(color);
					solidColorBrush.AlphaConstant = color.A;
					BrushFactory.solidColorBrushesStore.Add(solidColorBrush.Color, solidColorBrush);
				}
			}
			return solidColorBrush;
		}

		static readonly object lockObject = new object();

		static readonly Dictionary<Color, SolidColorBrush> solidColorBrushesStore = new Dictionary<Color, SolidColorBrush>();
	}
}
