using System;

namespace Telerik.Windows.Documents.Media
{
	public static class Unit
	{
		public static double DipToPoint(double value)
		{
			return value * 72.0 / 96.0;
		}

		public static int DipToPointI(double pixels)
		{
			return (int)Math.Round(Unit.DipToPoint(pixels));
		}

		public static double DipToPica(double value)
		{
			return value * 6.0 / 96.0;
		}

		public static double DipToCm(double value)
		{
			return value * 2.54 / 96.0;
		}

		public static double DipToMm(double value)
		{
			return Unit.DipToCm(value * 10.0);
		}

		public static double DipToInch(double value)
		{
			return value / 96.0;
		}

		public static double DipToTwip(double value)
		{
			return value * 1440.0 / 96.0;
		}

		public static double DipToEmu(double value)
		{
			return value * 914400.0 / 96.0;
		}

		public static int DipToEmuI(double value)
		{
			return (int)Math.Round(value * 914400.0 / 96.0);
		}

		public static int DipToTwipI(double value)
		{
			return (int)Math.Round(Unit.DipToTwip(value));
		}

		public static float DipToTwipF(double value)
		{
			return (float)Unit.DipToTwip(value);
		}

		public static float TwipToDipF(double value)
		{
			return (float)Unit.TwipToDip(value);
		}

		public static int TwipToDipI(double value)
		{
			return (int)Unit.TwipToDip(value);
		}

		public static double DipToUnit(double value, UnitType type)
		{
			if (type == UnitType.Cm)
			{
				return Unit.DipToCm(value);
			}
			if (type == UnitType.Inch)
			{
				return Unit.DipToInch(value);
			}
			if (type == UnitType.Mm)
			{
				return Unit.DipToMm(value);
			}
			if (type == UnitType.Pica)
			{
				return Unit.DipToPica(value);
			}
			if (type == UnitType.Point)
			{
				return Unit.DipToPoint(value);
			}
			if (type == UnitType.Twip)
			{
				return Unit.DipToTwip(value);
			}
			if (type == UnitType.Emu)
			{
				return Unit.DipToEmu(value);
			}
			if (type == UnitType.Dip)
			{
				return value;
			}
			throw new ArgumentException("Unknown UnitType");
		}

		public static double PointToDip(double value)
		{
			return value * 96.0 / 72.0;
		}

		public static double PicaToDip(double value)
		{
			return value * 96.0 / 6.0;
		}

		public static double EmuToDip(double value)
		{
			return Math.Round(value * 96.0 / 914400.0, 2);
		}

		public static double CmToDip(double value)
		{
			return value * 96.0 / 2.54;
		}

		public static double MmToDip(double value)
		{
			return Unit.CmToDip(value / 10.0);
		}

		public static double InchToDip(double value)
		{
			return value * 96.0;
		}

		public static double TwipToDip(double value)
		{
			return value * 96.0 / 1440.0;
		}

		public static double UnitToDip(double value, UnitType type)
		{
			if (type == UnitType.Cm)
			{
				return Unit.CmToDip(value);
			}
			if (type == UnitType.Inch)
			{
				return Unit.InchToDip(value);
			}
			if (type == UnitType.Mm)
			{
				return Unit.MmToDip(value);
			}
			if (type == UnitType.Pica)
			{
				return Unit.PicaToDip(value);
			}
			if (type == UnitType.Point)
			{
				return Unit.PointToDip(value);
			}
			if (type == UnitType.Twip)
			{
				return Unit.TwipToDip(value);
			}
			if (type == UnitType.Emu)
			{
				return Unit.EmuToDip(value);
			}
			if (type == UnitType.Dip)
			{
				return value;
			}
			throw new ArgumentException("Unknown UnitType");
		}

		public static double PixelToEm(double basePixelSize, double value)
		{
			return value / basePixelSize;
		}

		public static double EmToPixel(double basePixelSize, double value)
		{
			return basePixelSize * value;
		}

		public static double PixelToPercent(double basePixelSize, double value)
		{
			return value / basePixelSize * 100.0;
		}

		public static double PercentToPixel(double basePixelSize, double value)
		{
			return basePixelSize * (value / 100.0);
		}

		public static double UnitToPixel(double basePixelSize, double value, UnitType type)
		{
			if (type == UnitType.Em)
			{
				return Unit.EmToPixel(basePixelSize, value);
			}
			if (type == UnitType.Percent)
			{
				return Unit.PercentToPixel(basePixelSize, value);
			}
			if (type == UnitType.Dip)
			{
				return value;
			}
			return Unit.UnitToDip(value, type);
		}

		public static double PixelToUnit(double basePixelSize, double value, UnitType type)
		{
			if (type == UnitType.Em)
			{
				return Unit.PixelToEm(basePixelSize, value);
			}
			if (type == UnitType.Percent)
			{
				return Unit.PixelToPercent(basePixelSize, value);
			}
			if (type == UnitType.Dip)
			{
				return value;
			}
			return Unit.DipToUnit(value, type);
		}

		public static bool IsRelativeUnitType(UnitType type)
		{
			return type == UnitType.Em || type == UnitType.Percent;
		}

		const double DefaultDpi = 96.0;

		const double CmToInch = 2.54;

		const double PointToInch = 72.0;

		const double PicaToInch = 6.0;

		const double TwipToInch = 1440.0;

		const double EmuToInch = 914400.0;
	}
}
