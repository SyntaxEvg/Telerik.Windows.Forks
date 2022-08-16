using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
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
			Guard.ThrowExceptionIfNull<IEnumerable<GradientStop>>(stops, "stops");
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
				Guard.ThrowExceptionIfNull<List<GradientStop>>(value, "value");
				this.stops = value;
			}
		}

		public double? Degree { get; set; }

		public double? Left { get; set; }

		public double? Top { get; set; }

		public double? Right { get; set; }

		public double? Bottom { get; set; }

		public GradientInfoType? InfoType { get; set; }

		public static GradientFillInfo Create(GradientFill gradientFill)
		{
			return GradientFillInfo.gradientTypeToInfoFactory[gradientFill.GradientType](gradientFill);
		}

		public GradientFill ToFill()
		{
			ThemableColor themableColor = null;
			ThemableColor themableColor2 = null;
			GradientType gradientType = GradientType.Horizontal;
			foreach (GradientStop gradientStop in this.Stops)
			{
				if (themableColor == null)
				{
					themableColor = gradientStop.ThemableColor;
				}
				else if (themableColor2 == null)
				{
					themableColor2 = gradientStop.ThemableColor;
				}
			}
			Guard.ThrowExceptionIfNull<ThemableColor>(themableColor, "color1");
			Guard.ThrowExceptionIfNull<ThemableColor>(themableColor2, "color2");
			GradientFill arg = new GradientFill(GradientType.Horizontal, themableColor, themableColor2);
			foreach (KeyValuePair<GradientType, Func<GradientFill, GradientFillInfo>> keyValuePair in GradientFillInfo.gradientTypeToInfoFactory)
			{
				Func<GradientFill, GradientFillInfo> value = keyValuePair.Value;
				GradientFillInfo gradientFillInfo = value(arg);
				if (this.Stops.Count == gradientFillInfo.Stops.Count && TelerikHelper.EqualsOfT<double?>(this.Degree, gradientFillInfo.Degree) && TelerikHelper.EqualsOfT<double?>(this.Left, gradientFillInfo.Left) && TelerikHelper.EqualsOfT<double?>(this.Right, gradientFillInfo.Right) && TelerikHelper.EqualsOfT<double?>(this.Top, gradientFillInfo.Top) && TelerikHelper.EqualsOfT<double?>(this.Bottom, gradientFillInfo.Bottom) && TelerikHelper.EqualsOfT<GradientInfoType?>(this.InfoType, gradientFillInfo.InfoType))
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
			return new GradientFill(gradientType, themableColor, themableColor2);
		}

		static void InitGradientInfoFactories()
		{
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.Horizontal, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(90.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.HorizontalReversed, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(270.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.HorizontalRepeated, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(0.5, gradientFill.Color2),
					new GradientStop(1.0, gradientFill.Color1)
				};
				return new GradientFillInfo(list, new double?(90.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.Vertical, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.VerticalReversed, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(180.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.VerticalRepeated, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(0.5, gradientFill.Color2),
					new GradientStop(1.0, gradientFill.Color1)
				};
				return new GradientFillInfo(list, null, null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.DiagonalUp, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(45.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.DiagonalUpReversed, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(225.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.DiagonalUpRepeated, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(0.5, gradientFill.Color2),
					new GradientStop(1.0, gradientFill.Color1)
				};
				return new GradientFillInfo(list, new double?(45.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.DiagonalDown, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(135.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.DiagonalDownReversed, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, new double?(315.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.DiagonalDownRepeated, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(0.5, gradientFill.Color2),
					new GradientStop(1.0, gradientFill.Color1)
				};
				return new GradientFillInfo(list, new double?(135.0), null, null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.FromTopLeftCorner, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, new GradientInfoType?(GradientInfoType.Path), null, null, null, null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.FromTopRightCorner, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, new GradientInfoType?(GradientInfoType.Path), new double?(1.0), null, new double?(1.0), null);
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.FromBottomLeftCorner, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, new GradientInfoType?(GradientInfoType.Path), null, new double?(1.0), null, new double?(1.0));
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.FromBottomRightCorner, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, new GradientInfoType?(GradientInfoType.Path), new double?(1.0), new double?(1.0), new double?(1.0), new double?(1.0));
			});
			GradientFillInfo.RegisterGradientCreationMethod(GradientType.FromCenter, delegate(GradientFill gradientFill)
			{
				List<GradientStop> list = new List<GradientStop>
				{
					new GradientStop(0.0, gradientFill.Color1),
					new GradientStop(1.0, gradientFill.Color2)
				};
				return new GradientFillInfo(list, null, new GradientInfoType?(GradientInfoType.Path), new double?(0.5), new double?(0.5), new double?(0.5), new double?(0.5));
			});
		}

		static void RegisterGradientCreationMethod(GradientType gradientType, Func<GradientFill, GradientFillInfo> factory)
		{
			GradientFillInfo.gradientTypeToInfoFactory.Add(gradientType, factory);
		}

		static readonly Dictionary<GradientType, Func<GradientFill, GradientFillInfo>> gradientTypeToInfoFactory = new Dictionary<GradientType, Func<GradientFill, GradientFillInfo>>();

		List<GradientStop> stops;
	}
}
