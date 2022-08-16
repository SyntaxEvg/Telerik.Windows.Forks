using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class PressureConverter
	{
		static PressureConverter()
		{
			Dictionary<Tuple<PressureUnit, PressureUnit>, Func<double, double>> dictionary = new Dictionary<Tuple<PressureUnit, PressureUnit>, Func<double, double>>();
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Pascal, PressureUnit.Pascal), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Pascal, PressureUnit.Atmosphere), (double v) => v * 9.86923266716013E-06);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Pascal, PressureUnit.Mercury), (double v) => v * 0.00750063755419211);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Pascal, PressureUnit.PSI), (double v) => v * 0.000145037737730209);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Pascal, PressureUnit.Torr), (double v) => v * 0.0075006168270417);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Atmosphere, PressureUnit.Pascal), (double v) => v * 101325.0);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Atmosphere, PressureUnit.Atmosphere), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Atmosphere, PressureUnit.Mercury), (double v) => v * 760.002100178515);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Atmosphere, PressureUnit.PSI), (double v) => v * 14.6959487755134);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Atmosphere, PressureUnit.Torr), (double v) => v * 760.0);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Mercury, PressureUnit.Pascal), (double v) => v * 133.322);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Mercury, PressureUnit.Atmosphere), (double v) => v * 0.00131578583765112);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Mercury, PressureUnit.Mercury), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Mercury, PressureUnit.PSI), (double v) => v * 0.019336721269667);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Mercury, PressureUnit.Torr), (double v) => v * 0.999997236614853);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.PSI, PressureUnit.Pascal), (double v) => v * 6894.75729316836);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.PSI, PressureUnit.Atmosphere), (double v) => v * 0.0680459639098777);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.PSI, PressureUnit.Mercury), (double v) => v * 51.7150754801785);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.PSI, PressureUnit.PSI), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.PSI, PressureUnit.Torr), (double v) => v * 51.7149325715071);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Torr, PressureUnit.Pascal), (double v) => v * 133.322368421053);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Torr, PressureUnit.Atmosphere), (double v) => v * 0.00131578947368421);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Torr, PressureUnit.Mercury), (double v) => v * 1.00000276339278);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Torr, PressureUnit.PSI), (double v) => v * 0.019336774704623);
			dictionary.Add(Tuple.Create<PressureUnit, PressureUnit>(PressureUnit.Torr, PressureUnit.Torr), (double v) => v * 1.0);
			PressureConverter.Lookup = dictionary;
		}

		public static double Convert(double number, PressureUnit from_unit, PressureUnit to_unit)
		{
			if (from_unit != to_unit)
			{
				return PressureConverter.Lookup[Tuple.Create<PressureUnit, PressureUnit>(from_unit, to_unit)](number);
			}
			return number;
		}

		static readonly Dictionary<Tuple<PressureUnit, PressureUnit>, Func<double, double>> Lookup;
	}
}
