using System;
using Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	static class Converters
	{
		public static HexBinary3ColorConverter HexBinary3Converter = new HexBinary3ColorConverter();

		public static NameOrHashedHexBinary3ColorConverter NameOrHashedHexBinary3Converter = new NameOrHashedHexBinary3ColorConverter();

		public static HexToTintConverter HexToTintConverter = new HexToTintConverter();

		public static BoolConverter BoolValueConverter = new BoolConverter();

		public static EmuToDipConverter EmuToDipConverter = new EmuToDipConverter();

		public static UniversalMeasureToDipConverter UniversalMeasureToDipConverter = new UniversalMeasureToDipConverter();

		public static EmuOrUniversalMeasureToDipConverter EmuOrUniversalMeasureToDipConverter = new EmuOrUniversalMeasureToDipConverter();

		public static TwipToDipConverter TwipToDipConverter = new TwipToDipConverter();

		public static SignedTwipsMeasureToDipConverter SignedTwipsMeasureToDipConverter = new SignedTwipsMeasureToDipConverter();

		public static PageOrientationConverter PageOrientationConverter = new PageOrientationConverter();

		public static UnsignedIntHexConverter UnsignedIntHexConverter = new UnsignedIntHexConverter();

		public static IntConverter IntValueConverter = new IntConverter();

		public static NullableIntConverter NullableIntValueConverter = new NullableIntConverter();

		public static ThemeColorTypeToStringConverter ThemeColorTypeToStringConverter = new ThemeColorTypeToStringConverter();

		public static BarDirectionToStringConverter BarDirectionToValueConverter = new BarDirectionToStringConverter();

		public static AxisPositionToStringConverter AxisPositionToStringConverter = new AxisPositionToStringConverter();

		public static BarGroupingToStringConverter BarGroupingToStringConverter = new BarGroupingToStringConverter();

		public static SeriesGroupingToStringConverter SeriesGroupingToStringConverter = new SeriesGroupingToStringConverter();

		public static LegendPositionToStringConverter LegendPositionToStringConverter = new LegendPositionToStringConverter();

		public static MarkerStyleToStringConverter MarkerStyleToStringConverter = new MarkerStyleToStringConverter();

		public static ScatterStyleToStringConverter ScatterStyleToStringConverter = new ScatterStyleToStringConverter();
	}
}
