using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class EnergyConverter
	{
		static EnergyConverter()
		{
			Dictionary<Tuple<EnergyUnit, EnergyUnit>, Func<double, double>> dictionary = new Dictionary<Tuple<EnergyUnit, EnergyUnit>, Func<double, double>>();
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Joule, EnergyUnit.Joule), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Joule, EnergyUnit.Erg), (double v) => v * 10000000.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Joule, EnergyUnit.ThermodynamicCalorie), (double v) => v * 0.239005736137667);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Joule, EnergyUnit.ITCalorie), (double v) => v * 0.238845896627496);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Joule, EnergyUnit.ElectronVolt), (double v) => v * 6.24150964712042E+18);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Joule, EnergyUnit.HorsePowerPerHour), (double v) => v * 3.72506135998619E-07);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Joule, EnergyUnit.WattPerHour), (double v) => v * 0.000277777777777778);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Joule, EnergyUnit.FootPound), (double v) => v * 0.737562149277265);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Joule, EnergyUnit.BTU), (double v) => v * 0.000947817120313317);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Erg, EnergyUnit.Joule), (double v) => v * 1E-07);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Erg, EnergyUnit.Erg), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Erg, EnergyUnit.ThermodynamicCalorie), (double v) => v * 2.39005736137667E-08);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Erg, EnergyUnit.ITCalorie), (double v) => v * 2.38845896627496E-08);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Erg, EnergyUnit.ElectronVolt), (double v) => v * 624150964712.042);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Erg, EnergyUnit.HorsePowerPerHour), (double v) => v * 3.72506135998619E-14);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Erg, EnergyUnit.WattPerHour), (double v) => v * 2.77777777777778E-11);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Erg, EnergyUnit.FootPound), (double v) => v * 7.37562149277265E-08);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.Erg, EnergyUnit.BTU), (double v) => v * 9.47817120313317E-11);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ThermodynamicCalorie, EnergyUnit.Joule), (double v) => v * 4.184);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ThermodynamicCalorie, EnergyUnit.Erg), (double v) => v * 41840000.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ThermodynamicCalorie, EnergyUnit.ThermodynamicCalorie), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ThermodynamicCalorie, EnergyUnit.ITCalorie), (double v) => v * 0.999331231489443);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ThermodynamicCalorie, EnergyUnit.ElectronVolt), (double v) => v * 2.61144763635518E+19);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ThermodynamicCalorie, EnergyUnit.HorsePowerPerHour), (double v) => v * 1.55856567301822E-06);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ThermodynamicCalorie, EnergyUnit.WattPerHour), (double v) => v * 0.00116222222222222);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ThermodynamicCalorie, EnergyUnit.FootPound), (double v) => v * 3.08596003257608);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ThermodynamicCalorie, EnergyUnit.BTU), (double v) => v * 0.00396566683139092);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ITCalorie, EnergyUnit.Joule), (double v) => v * 4.1868);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ITCalorie, EnergyUnit.Erg), (double v) => v * 41868000.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ITCalorie, EnergyUnit.ThermodynamicCalorie), (double v) => v * 1.00066921606119);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ITCalorie, EnergyUnit.ITCalorie), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ITCalorie, EnergyUnit.ElectronVolt), (double v) => v * 2.61319525905638E+19);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ITCalorie, EnergyUnit.HorsePowerPerHour), (double v) => v * 1.55960869019902E-06);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ITCalorie, EnergyUnit.WattPerHour), (double v) => v * 0.001163);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ITCalorie, EnergyUnit.FootPound), (double v) => v * 3.08802520659405);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ITCalorie, EnergyUnit.BTU), (double v) => v * 0.0039683207193278);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ElectronVolt, EnergyUnit.Joule), (double v) => v * 1.602176487E-19);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ElectronVolt, EnergyUnit.Erg), (double v) => v * 1.602176487E-12);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ElectronVolt, EnergyUnit.ThermodynamicCalorie), (double v) => v * 3.82929370697897E-20);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ElectronVolt, EnergyUnit.ITCalorie), (double v) => v * 3.82673279593007E-20);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ElectronVolt, EnergyUnit.ElectronVolt), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ElectronVolt, EnergyUnit.HorsePowerPerHour), (double v) => v * 5.96820572360211E-26);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ElectronVolt, EnergyUnit.WattPerHour), (double v) => v * 4.45049024166667E-23);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ElectronVolt, EnergyUnit.FootPound), (double v) => v * 1.18170473327322E-19);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.ElectronVolt, EnergyUnit.BTU), (double v) => v * 1.51857030414205E-22);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.HorsePowerPerHour, EnergyUnit.Joule), (double v) => v * 2684519.53769617);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.HorsePowerPerHour, EnergyUnit.Erg), (double v) => v * 26845195376961.7);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.HorsePowerPerHour, EnergyUnit.ThermodynamicCalorie), (double v) => v * 641615.568283024);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.HorsePowerPerHour, EnergyUnit.ITCalorie), (double v) => v * 641186.475995073);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.HorsePowerPerHour, EnergyUnit.ElectronVolt), (double v) => v * 1.67554545924139E+25);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.HorsePowerPerHour, EnergyUnit.HorsePowerPerHour), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.HorsePowerPerHour, EnergyUnit.WattPerHour), (double v) => v * 745.69987158227);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.HorsePowerPerHour, EnergyUnit.FootPound), (double v) => v * 1980000.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.HorsePowerPerHour, EnergyUnit.BTU), (double v) => v * 2544.43357764402);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.WattPerHour, EnergyUnit.Joule), (double v) => v * 3600.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.WattPerHour, EnergyUnit.Erg), (double v) => v * 36000000000.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.WattPerHour, EnergyUnit.ThermodynamicCalorie), (double v) => v * 860.420650095602);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.WattPerHour, EnergyUnit.ITCalorie), (double v) => v * 859.845227858985);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.WattPerHour, EnergyUnit.ElectronVolt), (double v) => v * 2.24694347296335E+22);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.WattPerHour, EnergyUnit.HorsePowerPerHour), (double v) => v * 0.00134102208959503);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.WattPerHour, EnergyUnit.WattPerHour), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.WattPerHour, EnergyUnit.FootPound), (double v) => v * 2655.22373739816);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.WattPerHour, EnergyUnit.BTU), (double v) => v * 3.41214163312794);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.FootPound, EnergyUnit.Joule), (double v) => v * 1.3558179483314);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.FootPound, EnergyUnit.Erg), (double v) => v * 13558179.483314);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.FootPound, EnergyUnit.ThermodynamicCalorie), (double v) => v * 0.324048266809608);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.FootPound, EnergyUnit.ITCalorie), (double v) => v * 0.323831553532865);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.FootPound, EnergyUnit.ElectronVolt), (double v) => v * 8.46235080424945E+18);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.FootPound, EnergyUnit.HorsePowerPerHour), (double v) => v * 5.05050505050505E-07);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.FootPound, EnergyUnit.WattPerHour), (double v) => v * 0.000376616096758722);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.FootPound, EnergyUnit.FootPound), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.FootPound, EnergyUnit.BTU), (double v) => v * 0.00128506746345658);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.BTU, EnergyUnit.Joule), (double v) => v * 1055.05585262);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.BTU, EnergyUnit.Erg), (double v) => v * 10550558526.2);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.BTU, EnergyUnit.ThermodynamicCalorie), (double v) => v * 252.164400721797);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.BTU, EnergyUnit.ITCalorie), (double v) => v * 251.995761111111);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.BTU, EnergyUnit.ElectronVolt), (double v) => v * 6.58514128237859E+21);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.BTU, EnergyUnit.HorsePowerPerHour), (double v) => v * 0.000393014778922204);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.BTU, EnergyUnit.WattPerHour), (double v) => v * 0.293071070172222);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.BTU, EnergyUnit.FootPound), (double v) => v * 778.169262265965);
			dictionary.Add(Tuple.Create<EnergyUnit, EnergyUnit>(EnergyUnit.BTU, EnergyUnit.BTU), (double v) => v * 1.0);
			EnergyConverter.Lookup = dictionary;
		}

		public static double Convert(double number, EnergyUnit from_unit, EnergyUnit to_unit)
		{
			if (from_unit != to_unit)
			{
				return EnergyConverter.Lookup[Tuple.Create<EnergyUnit, EnergyUnit>(from_unit, to_unit)](number);
			}
			return number;
		}

		static readonly Dictionary<Tuple<EnergyUnit, EnergyUnit>, Func<double, double>> Lookup;
	}
}
