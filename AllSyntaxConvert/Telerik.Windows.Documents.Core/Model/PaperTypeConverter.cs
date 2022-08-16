using System;
using System.Collections.Generic;
using System.Windows;

namespace Telerik.Windows.Documents.Model
{
	public static class PaperTypeConverter
	{
		static PaperTypeConverter()
		{
			PaperTypeConverter.paperTypeSizes[PaperTypes.A0] = new Size(3173.0, 4491.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.A1] = new Size(2245.0, 3173.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.A2] = new Size(1587.0, 2245.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.A3] = new Size(1123.0, 1587.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.A4] = new Size(793.0, 1123.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.A5] = new Size(560.0, 793.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.RA0] = new Size(3251.0, 4611.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.RA1] = new Size(2305.0, 3251.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.RA2] = new Size(1625.0, 2305.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.RA3] = new Size(1153.0, 1625.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.RA4] = new Size(812.0, 1153.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.RA5] = new Size(457.0, 812.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.B0] = new Size(3780.0, 5344.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.B1] = new Size(2672.0, 3780.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.B2] = new Size(5669.0, 1889.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.B3] = new Size(1335.0, 1889.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.B4] = new Size(972.0, 1376.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.B5] = new Size(688.0, 972.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Quarto] = new Size(768.0, 960.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Foolscap] = new Size(768.0, 1248.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Executive] = new Size(720.0, 960.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.GovernmentLetter] = new Size(1008.0, 768.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Letter] = new Size(816.0, 1056.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Legal] = new Size(816.0, 1344.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Ledger] = new Size(1632.0, 1056.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Tabloid] = new Size(1056.0, 1632.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Post] = new Size(1501.0, 1848.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Crown] = new Size(1920.0, 1440.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.LargePost] = new Size(1584.0, 2016.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Demy] = new Size(1680.0, 2112.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Medium] = new Size(1728.0, 2208.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Royal] = new Size(1920.0, 2400.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Elephant] = new Size(2087.0, 2688.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.DoubleDemy] = new Size(2256.0, 3360.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.QuadDemy] = new Size(3360.0, 4320.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.STMT] = new Size(528.0, 816.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Folio] = new Size(816.0, 1248.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Statement] = new Size(528.0, 816.0);
			PaperTypeConverter.paperTypeSizes[PaperTypes.Size10x14] = new Size(960.0, 1344.0);
		}

		public static Size ToSize(PaperTypes type)
		{
			Size result;
			if (PaperTypeConverter.paperTypeSizes.TryGetValue(type, out result))
			{
				return result;
			}
			throw new ArgumentException("Invalid PaperType.");
		}

		static readonly Dictionary<PaperTypes, Size> paperTypeSizes = new Dictionary<PaperTypes, Size>();
	}
}
