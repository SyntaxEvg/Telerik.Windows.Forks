using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class GradientFillInfo
	{
		static GradientFillInfo()
		{
			GradientFillInfo.InitGradientInfoFactories();
		}

		public GradientFillInfo()
		{
			this.Stops = new List<GradientStop>();
		}

		public GradientFillInfo(IEnumerable<GradientStop> stops, double? degree = null, GradientInfoType? infoType = null, double? left = null, double? top = null, double? right = null, double? bottom = null)
		{
			this.Stops = stops.ToList<GradientStop>();
			this.Degree = degree;
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
			this.InfoType = infoType;
		}

		public List<GradientStop> Stops
		{
			get
			{
				return this.stops;
			}
			set
			{
				this.stops = value;
			}
		}

		public double? Degree { get; set; }

		public double? Left { get; set; }

		public double? Top { get; set; }

		public double? Right { get; set; }

		public double? Bottom { get; set; }

		public GradientInfoType? InfoType { get; set; }

		public static GradientFillInfo Create(SpreadGradientFill gradientFill)
		{
			return GradientFillInfo.gradientTypeToInfoFactory[gradientFill.GradientType](gradientFill);
		}

		public SpreadGradientFill ToFill()
		{
			SpreadThemableColor spreadThemableColor = null;
			SpreadThemableColor spreadThemableColor2 = null;
			SpreadGradientType gradientType = SpreadGradientType.Horizontal;
			foreach (GradientStop gradientStop in this.Stops)
			{
				if (spreadThemableColor == null)
				{
					spreadThemableColor = gradientStop.ThemableColor;
				}
				else if (spreadThemableColor2 == null)
				{
					spreadThemableColor2 = gradientStop.ThemableColor;
				}
			}
			Guard.ThrowExceptionIfNull<SpreadThemableColor>(spreadThemableColor, "color1");
			Guard.ThrowExceptionIfNull<SpreadThemableColor>(spreadThemableColor2, "color2");
			SpreadGradientFill arg = new SpreadGradientFill(SpreadGradientType.Horizontal, spreadThemableColor, spreadThemableColor2);
			foreach (KeyValuePair<SpreadGradientType, Func<SpreadGradientFill, GradientFillInfo>> keyValuePair in GradientFillInfo.gradientTypeToInfoFactory)
			{
				Func<SpreadGradientFill, GradientFillInfo> value = keyValuePair.Value;
				GradientFillInfo gradientFillInfo = value(arg);
				if (this.Stops.Count == gradientFillInfo.Stops.Count && ObjectExtensions.EqualsOfT<double?>(this.Degree, gradientFillInfo.Degree) && ObjectExtensions.EqualsOfT<double?>(this.Left, gradientFillInfo.Left) && ObjectExtensions.EqualsOfT<double?>(this.Right, gradientFillInfo.Right) && ObjectExtensions.EqualsOfT<double?>(this.Top, gradientFillInfo.Top) && ObjectExtensions.EqualsOfT<double?>(this.Bottom, gradientFillInfo.Bottom) && ObjectExtensions.EqualsOfT<GradientInfoType?>(this.InfoType, gradientFillInfo.InfoType))
				{
					bool flag = true;
					for (int i = 0; i < this.Stops.Count; i++)
					{
						if (this.stops[i].Position != gradientFillInfo.stops[i].Position)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						gradientType = keyValuePair.Key;
						break;
					}
				}
			}
			return new SpreadGradientFill(gradientType, spreadThemableColor, spreadThemableColor2);
		}

		static void InitGradientInfoFactories()
		{
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.Horizontal, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(90.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.HorizontalReversed, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(270.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.HorizontalRepeated, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(0.5, gradientFill.Color2),
					new GradientStop(1.0, gradientFill.Color1)
				};
				return new GradientFillInfo(list, new double?(90.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.Vertical, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.VerticalReversed, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(180.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.VerticalRepeated, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(0.5, gradientFill.Color2),
					new GradientStop(1.0, gradientFill.Color1)
				};
				return new GradientFillInfo(list, null, null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.DiagonalUp, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(45.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.DiagonalUpReversed, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(225.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.DiagonalUpRepeated, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(0.5, gradientFill.Color2),
					new GradientStop(1.0, gradientFill.Color1)
				};
				return new GradientFillInfo(list, new double?(45.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.DiagonalDown, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(135.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.DiagonalDownReversed, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(315.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.DiagonalDownRepeated, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(0.5, gradientFill.Color2),
					new GradientStop(1.0, gradientFill.Color1)
				};
				return new GradientFillInfo(list, new double?(135.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.FromTopLeftCorner, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, new GradientInfoType?(GradientInfoType.Path), null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.FromTopRightCorner, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, new GradientInfoType?(GradientInfoType.Path), new double?(1.0), null, new double?(1.0), null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.FromBottomLeftCorner, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, new GradientInfoType?(GradientInfoType.Path), null, new double?(1.0), null, new double?(1.0));
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.FromBottomRightCorner, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, new GradientInfoType?(GradientInfoType.Path), new double?(1.0), new double?(1.0), new double?(1.0), new double?(1.0));
			});
			GradientFillInfo.RegisterGradientCreationMethod(SpreadGradientType.FromCenter, delegate(SpreadGradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, new GradientInfoType?(GradientInfoType.Path), new double?(0.5), new double?(0.5), new double?(0.5), new double?(0.5));
			});
		}

		static void RegisterGradientCreationMethod(SpreadGradientType gradientType, Func<SpreadGradientFill, GradientFillInfo> factory)
		{
			GradientFillInfo.gradientTypeToInfoFactory.Add(gradientType, factory);
		}

		static readonly Dictionary<SpreadGradientType, Func<SpreadGradientFill, GradientFillInfo>> gradientTypeToInfoFactory = new Dictionary<SpreadGradientType, Func<SpreadGradientFill, GradientFillInfo>>();

		List<GradientStop> stops;
	}
}
