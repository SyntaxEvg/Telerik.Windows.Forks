using System;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths.Number;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class EngineeringFunctions
	{
		public static double BESSELI(double x, double n)
		{
			return Functions.BesselI(x, (double)n.Truncate());
		}

		public static double BESSELJ(double x, double n)
		{
			return Functions.BesselJ(x, (double)n.Truncate());
		}

		public static double BESSELK(double x, double n)
		{
			return Functions.BesselK(x, (double)n.Truncate());
		}

		public static double BESSELY(double x, double n)
		{
			return Functions.BesselY(x, (double)n.Truncate());
		}

		public static long BIN2DEC(string number)
		{
			return TwoComplementNumber.FromBin(number, 10).ToDec();
		}

		public static string BIN2HEX(string number, int places)
		{
			return EngineeringFunctions.BIN2HEX(number).PadLeft(places, '0');
		}

		public static string BIN2HEX(string number)
		{
			return TwoComplementNumber.FromDec(TwoComplementNumber.FromBin(number, 10).ToDec(), 40).ToHex();
		}

		public static string BIN2OCT(string number, int places)
		{
			return EngineeringFunctions.BIN2OCT(number).PadLeft(places, '0');
		}

		public static string BIN2OCT(string number)
		{
			return TwoComplementNumber.FromDec(TwoComplementNumber.FromBin(number, 10).ToDec(), 30).ToOct();
		}

		public static long BITAND(long number1, long number2)
		{
			return number1 & number2;
		}

		public static long BITLSHIFT(long number, int amount)
		{
			if (amount >= 0)
			{
				return number << amount;
			}
			return EngineeringFunctions.BITRSHIFT(number, -amount);
		}

		public static long BITOR(long number1, long number2)
		{
			return number1 | number2;
		}

		public static long BITRSHIFT(long number, int amount)
		{
			if (amount >= 0)
			{
				return number >> amount;
			}
			return EngineeringFunctions.BITLSHIFT(number, -amount);
		}

		public static long BITXOR(long number1, long number2)
		{
			return number1 ^ number2;
		}

		public static string COMPLEX(double a, double b, string complexSymbol = "i")
		{
			if (complexSymbol != "i" && complexSymbol != "j")
			{
				throw new ArgumentException("Only 'i' or 'j' is allowed for the imaginary number.", "complexSymbol");
			}
			if (double.IsNaN(a) || double.IsInfinity(a))
			{
				throw new ArgumentException("Cannot pass NaN or Infinity as an complex coeficient!", "a");
			}
			if (double.IsNaN(b) || double.IsInfinity(b))
			{
				throw new ArgumentException("Cannot pass NaN or Infinity as an complex coeficient!", "b");
			}
			if (a == 0.0 && b == 0.0)
			{
				return "0";
			}
			string text = ((b > 0.0 && a != 0.0) ? "+" : string.Empty);
			string text2 = string.Empty;
			if (b == 1.0)
			{
				text2 = "-";
			}
			else if (b != 0.0 && b != 1.0)
			{
				text2 = b.ToString();
			}
			return string.Format("{0}{1}{2}{3}", new object[]
			{
				(a != 0.0) ? a.ToString() : string.Empty,
				text,
				text2,
				(b != 0.0) ? complexSymbol : string.Empty
			});
		}

		public static double CONVERT(double number, string from_unit, string to_unit)
		{
			return Converter.Convert(number, from_unit, to_unit);
		}

		public static string DEC2BIN(double number)
		{
			return TwoComplementNumber.FromDec(number.Truncate(), 10).ToBin();
		}

		public static string DEC2BIN(double number, int places)
		{
			return EngineeringFunctions.DEC2BIN(number).PadLeft(places, '0');
		}

		public static string DEC2HEX(double number, int places)
		{
			return EngineeringFunctions.DEC2HEX(number).PadLeft(places, '0');
		}

		public static string DEC2HEX(double number)
		{
			return TwoComplementNumber.FromDec(number.Truncate(), 40).ToHex();
		}

		public static string DEC2OCT(double number, int places)
		{
			return EngineeringFunctions.DEC2OCT(number).PadLeft(places, '0');
		}

		public static string DEC2OCT(double number)
		{
			return TwoComplementNumber.FromDec(number.Truncate(), 30).ToOct();
		}

		public static int DELTA(double number)
		{
			if (!number.IsZero(1E-08))
			{
				return 0;
			}
			return 1;
		}

		public static int DELTA(double number1, double number2)
		{
			return EngineeringFunctions.DELTA(number1 - number2);
		}

		public static double ERF(double lower_limit, double upper_limit)
		{
			return Functions.Erf(upper_limit) - Functions.Erf(lower_limit);
		}

		public static double ERF(double upper_limit)
		{
			return Functions.Erf(upper_limit);
		}

		public static int GESTEP(double number)
		{
			if (number < 0.0)
			{
				return 0;
			}
			return 1;
		}

		public static int GESTEP(double number, double step)
		{
			return EngineeringFunctions.GESTEP(number - step);
		}

		public static string HEX2BIN(string number, int places)
		{
			return EngineeringFunctions.HEX2BIN(number).PadLeft(places, '0');
		}

		public static string HEX2BIN(string number)
		{
			return TwoComplementNumber.FromDec(TwoComplementNumber.FromHex(number, 40).ToDec(), 10).ToBin();
		}

		public static long HEX2DEC(string number)
		{
			return TwoComplementNumber.FromHex(number, 40).ToDec();
		}

		public static string HEX2OCT(string number, int places)
		{
			return EngineeringFunctions.HEX2OCT(number).PadLeft(places, '0');
		}

		public static string HEX2OCT(string number)
		{
			return TwoComplementNumber.FromDec(TwoComplementNumber.FromHex(number, 40).ToDec(), 30).ToOct();
		}

		public static double IMABS(string complexNumber)
		{
			return System.Numerics.Complex.Abs(complexNumber.ToComplex());
		}

		public static double IMAGINARY(string complexNumber)
		{
			return complexNumber.ToComplex().Imaginary;
		}

		public static double IMARGUMENT(string complexNumber)
		{
			Complex complex = complexNumber.ToComplex();
			return Math.Atan2(complex.Imaginary, complex.Real);
		}

		public static Complex IMCONJUGATE(string complexNumber)
		{
			Complex value = complexNumber.ToComplex();
			return Complex.Conjugate(value);
		}

		public static Complex IMCOS(string complexNumber)
		{
			return Complex.Cos(complexNumber.ToComplex());
		}

		public static Complex IMCOSH(string complexNumber)
		{
			return Complex.Cosh(complexNumber.ToComplex());
		}

		public static Complex IMCOT(string complexNumber)
		{
			return Complex.Divide(1, EngineeringFunctions.IMTAN(complexNumber));
		}

		public static Complex IMCSC(string complexNumber)
		{
			return Complex.Divide(1, EngineeringFunctions.IMSIN(complexNumber));
		}

		public static Complex IMCSCH(string complexNumber)
		{
			return Complex.Divide(1, EngineeringFunctions.IMSINH(complexNumber));
		}

		public static Complex IMDIV(string complexNumber1, string complexNumber2)
		{
			return Complex.Divide(complexNumber1.ToComplex(), complexNumber2.ToComplex());
		}

		public static Complex IMEXP(string complexNumber)
		{
			return Complex.Exp(complexNumber.ToComplex());
		}

		public static Complex IMLN(string complexNumber)
		{
			return Complex.Log(complexNumber.ToComplex());
		}

		public static Complex IMLOG10(string complexNumber)
		{
			return Complex.Log10(complexNumber.ToComplex());
		}

		public static Complex IMLOG2(string complexNumber)
		{
			return Complex.Divide(EngineeringFunctions.IMLN(complexNumber), 0.6931471805599453);
		}

		public static Complex IMPOWER(string complexNumber, double power)
		{
			return Complex.Pow(complexNumber.ToComplex(), power);
		}

		public static Complex IMPRODUCT(string complexNumber1, string complexNumber2)
		{
			return Complex.Multiply(complexNumber1.ToComplex(), complexNumber2.ToComplex());
		}

		public static double IMREAL(string complexNumber)
		{
			return complexNumber.ToComplex().Real;
		}

		public static Complex IMSEC(string complexNumber)
		{
			return Complex.Divide(1, EngineeringFunctions.IMCOS(complexNumber));
		}

		public static Complex IMSECH(string complexNumber)
		{
			return Complex.Divide(1, EngineeringFunctions.IMCOSH(complexNumber));
		}

		public static Complex IMSIN(string complexNumber)
		{
			return Complex.Sin(complexNumber.ToComplex());
		}

		public static Complex IMSINH(string complexNumber)
		{
			return Complex.Sinh(complexNumber.ToComplex());
		}

		public static Complex IMSQRT(string complexNumber)
		{
			return Complex.Sqrt(complexNumber.ToComplex());
		}

		public static Complex IMSUB(string complexNumber1, string complexNumber2)
		{
			return Complex.Subtract(complexNumber1.ToComplex(), complexNumber2.ToComplex());
		}

		public static Complex IMSUM(string complexNumber1, string complexNumber2)
		{
			return Complex.Add(complexNumber1.ToComplex(), complexNumber2.ToComplex());
		}

		public static Complex IMTAN(string complexNumber)
		{
			return Complex.Tan(complexNumber.ToComplex());
		}

		public static string OCT2BIN(string number, int places)
		{
			return EngineeringFunctions.OCT2BIN(number).PadLeft(places, '0');
		}

		public static string OCT2BIN(string number)
		{
			return TwoComplementNumber.FromDec(TwoComplementNumber.FromOct(number, 30).ToDec(), 10).ToBin();
		}

		public static long OCT2DEC(string number)
		{
			return TwoComplementNumber.FromOct(number, 30).ToDec();
		}

		public static string OCT2HEX(string number, int places)
		{
			return EngineeringFunctions.OCT2HEX(number).PadLeft(places, '0');
		}

		public static string OCT2HEX(string number)
		{
			return TwoComplementNumber.FromDec(TwoComplementNumber.FromOct(number, 30).ToDec(), 40).ToHex();
		}

		const int hexBitsCount = 40;

		const int octBitsCount = 30;

		const int binBitsCount = 10;
	}
}
