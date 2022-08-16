using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class MassConverter
	{
		static MassConverter()
		{
			Dictionary<Tuple<MassUnit, MassUnit>, Func<double, double>> dictionary = new Dictionary<Tuple<MassUnit, MassUnit>, Func<double, double>>();
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.AtomicMassUnit), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.Grain), (double v) => v * 2.56260295427809E-23);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.Gram), (double v) => v * 1.660538782E-24);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.ImperialTon), (double v) => v * 1.63431310859572E-30);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.ImperialWeight), (double v) => v * 3.26862621719144E-29);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.Ounce), (double v) => v * 5.85737818120706E-26);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.Pound), (double v) => v * 3.66086136325441E-27);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.ShortWeight), (double v) => v * 3.66086136325441E-29);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.Slug), (double v) => v * 1.13783049616326E-28);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.Stone), (double v) => v * 2.61490097375315E-28);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.AtomicMassUnit, MassUnit.Ton), (double v) => v * 1.83043068162721E-30);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.AtomicMassUnit), (double v) => v * 3.90228224130691E+22);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.Grain), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.Gram), (double v) => v * 0.06479891);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.ImperialTon), (double v) => v * 6.37755102040816E-08);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.ImperialWeight), (double v) => v * 1.27551020408163E-06);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.Ounce), (double v) => v * 0.00228571428571429);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.Pound), (double v) => v * 0.000142857142857143);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.ShortWeight), (double v) => v * 1.42857142857143E-06);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.Slug), (double v) => v * 4.44013573879532E-06);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.Stone), (double v) => v * 1.02040816326531E-05);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Grain, MassUnit.Ton), (double v) => v * 7.14285714285714E-08);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.AtomicMassUnit), (double v) => v * 6.02214179421676E+23);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.Grain), (double v) => v * 15.4323583529414);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.Gram), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.ImperialTon), (double v) => v * 9.84206527611061E-07);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.ImperialWeight), (double v) => v * 1.96841305522212E-05);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.Ounce), (double v) => v * 0.0352739619495804);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.Pound), (double v) => v * 0.00220462262184878);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.ShortWeight), (double v) => v * 2.20462262184878E-05);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.Slug), (double v) => v * 6.85217658567918E-05);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.Stone), (double v) => v * 0.00015747304441777);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Gram, MassUnit.Ton), (double v) => v * 1.10231131092439E-06);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.AtomicMassUnit), (double v) => v * 6.11877855436923E+29);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.Grain), (double v) => v * 15680000.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.Gram), (double v) => v * 1016046.9088);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.ImperialTon), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.ImperialWeight), (double v) => v * 20.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.Ounce), (double v) => v * 35840.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.Pound), (double v) => v * 2240.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.ShortWeight), (double v) => v * 22.4);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.Slug), (double v) => v * 69.6213283843107);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.Stone), (double v) => v * 160.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialTon, MassUnit.Ton), (double v) => v * 1.12);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.AtomicMassUnit), (double v) => v * 3.05938927718461E+28);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.Grain), (double v) => v * 784000.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.Gram), (double v) => v * 50802.34544);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.ImperialTon), (double v) => v * 0.05);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.ImperialWeight), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.Ounce), (double v) => v * 1792.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.Pound), (double v) => v * 112.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.ShortWeight), (double v) => v * 1.12);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.Slug), (double v) => v * 3.48106641921553);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.Stone), (double v) => v * 8.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ImperialWeight, MassUnit.Ton), (double v) => v * 0.056);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.AtomicMassUnit), (double v) => v * 1.70724848057177E+25);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.Grain), (double v) => v * 437.5);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.Gram), (double v) => v * 28.349523125);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.ImperialTon), (double v) => v * 2.79017857142857E-05);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.ImperialWeight), (double v) => v * 0.000558035714285714);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.Ounce), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.Pound), (double v) => v * 0.0625);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.ShortWeight), (double v) => v * 0.000625);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.Slug), (double v) => v * 0.00194255938572295);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.Stone), (double v) => v * 0.00446428571428571);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ounce, MassUnit.Ton), (double v) => v * 3.125E-05);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.AtomicMassUnit), (double v) => v * 2.73159756891483E+26);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.Grain), (double v) => v * 7000.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.Gram), (double v) => v * 453.59237);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.ImperialTon), (double v) => v * 0.000446428571428571);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.ImperialWeight), (double v) => v * 0.00892857142857143);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.Ounce), (double v) => v * 16.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.Pound), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.ShortWeight), (double v) => v * 0.01);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.Slug), (double v) => v * 0.0310809501715673);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.Stone), (double v) => v * 0.0714285714285714);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Pound, MassUnit.Ton), (double v) => v * 0.0005);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.AtomicMassUnit), (double v) => v * 2.73159756891483E+28);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.Grain), (double v) => v * 700000.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.Gram), (double v) => v * 45359.237);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.ImperialTon), (double v) => v * 0.0446428571428571);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.ImperialWeight), (double v) => v * 0.892857142857143);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.Ounce), (double v) => v * 1600.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.Pound), (double v) => v * 100.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.ShortWeight), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.Slug), (double v) => v * 3.10809501715673);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.Stone), (double v) => v * 7.14285714285714);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.ShortWeight, MassUnit.Ton), (double v) => v * 0.05);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.AtomicMassUnit), (double v) => v * 8.78865528188932E+27);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.Grain), (double v) => v * 225218.339895013);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.Gram), (double v) => v * 14593.9029372064);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.ImperialTon), (double v) => v * 0.0143634145341207);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.ImperialWeight), (double v) => v * 0.287268290682415);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.Ounce), (double v) => v * 514.784776902887);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.Pound), (double v) => v * 32.1740485564304);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.ShortWeight), (double v) => v * 0.321740485564304);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.Slug), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.Stone), (double v) => v * 2.29814632545932);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Slug, MassUnit.Ton), (double v) => v * 0.0160870242782152);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.AtomicMassUnit), (double v) => v * 3.82423659648077E+27);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.Grain), (double v) => v * 98000.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.Gram), (double v) => v * 6350.29318);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.ImperialTon), (double v) => v * 0.00625);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.ImperialWeight), (double v) => v * 0.125);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.Ounce), (double v) => v * 224.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.Pound), (double v) => v * 14.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.ShortWeight), (double v) => v * 0.14);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.Slug), (double v) => v * 0.435133302401942);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.Stone), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Stone, MassUnit.Ton), (double v) => v * 0.007);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.AtomicMassUnit), (double v) => v * 5.46319513782967E+29);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.Grain), (double v) => v * 14000000.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.Gram), (double v) => v * 907184.74);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.ImperialTon), (double v) => v * 0.892857142857143);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.ImperialWeight), (double v) => v * 17.8571428571429);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.Ounce), (double v) => v * 32000.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.Pound), (double v) => v * 2000.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.ShortWeight), (double v) => v * 20.0);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.Slug), (double v) => v * 62.1619003431345);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.Stone), (double v) => v * 142.857142857143);
			dictionary.Add(Tuple.Create<MassUnit, MassUnit>(MassUnit.Ton, MassUnit.Ton), (double v) => v * 1.0);
			MassConverter.Lookup = dictionary;
		}

		public static double Convert(double number, MassUnit from_unit, MassUnit to_unit)
		{
			if (from_unit != to_unit)
			{
				return MassConverter.Lookup[Tuple.Create<MassUnit, MassUnit>(from_unit, to_unit)](number);
			}
			return number;
		}

		static readonly Dictionary<Tuple<MassUnit, MassUnit>, Func<double, double>> Lookup;
	}
}
