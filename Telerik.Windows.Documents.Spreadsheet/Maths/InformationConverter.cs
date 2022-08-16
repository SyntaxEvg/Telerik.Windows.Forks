using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class InformationConverter
	{
		static InformationConverter()
		{
			Dictionary<Tuple<InformationUnit, InformationUnit>, Func<double, double>> dictionary = new Dictionary<Tuple<InformationUnit, InformationUnit>, Func<double, double>>();
			dictionary.Add(Tuple.Create<InformationUnit, InformationUnit>(InformationUnit.Bit, InformationUnit.Byte), (double v) => v * 0.125);
			dictionary.Add(Tuple.Create<InformationUnit, InformationUnit>(InformationUnit.Byte, InformationUnit.Bit), (double v) => v * 8.0);
			InformationConverter.Lookup = dictionary;
		}

		public static double Convert(double number, InformationUnit from_unit, InformationUnit to_unit)
		{
			if (from_unit != to_unit)
			{
				return InformationConverter.Lookup[Tuple.Create<InformationUnit, InformationUnit>(from_unit, to_unit)](number);
			}
			return number;
		}

		static readonly Dictionary<Tuple<InformationUnit, InformationUnit>, Func<double, double>> Lookup;
	}
}
