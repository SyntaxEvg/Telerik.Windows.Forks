using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ArgumentConversionRules
	{
		public ArgumentInterpretation EmptyDirectArgument
		{
			get
			{
				return this.emptyDirectArgument;
			}
		}

		public ArgumentInterpretation NumberDirectArgument
		{
			get
			{
				return this.numberDirectArgument;
			}
		}

		public ArgumentInterpretation BoolDirectArgument
		{
			get
			{
				return this.boolDirectArgument;
			}
		}

		public ArgumentInterpretation TextNumberDirectArgument
		{
			get
			{
				return this.textNumberDirectArgument;
			}
		}

		public ArgumentInterpretation NonTextNumberDirectArgument
		{
			get
			{
				return this.nonTextNumberDirectArgument;
			}
		}

		public ArgumentInterpretation EmptyIndirectArgument
		{
			get
			{
				return this.emptyIndirectArgument;
			}
		}

		public ArgumentInterpretation NumberIndirectArgument
		{
			get
			{
				return this.numberIndirectArgument;
			}
		}

		public ArgumentInterpretation BoolIndirectArgument
		{
			get
			{
				return this.boolIndirectArgument;
			}
		}

		public ArgumentInterpretation TextNumberIndirectArgument
		{
			get
			{
				return this.textNumberIndirectArgument;
			}
		}

		public ArgumentInterpretation NonTextNumberIndirectArgument
		{
			get
			{
				return this.nonTextNumberIndirectArgument;
			}
		}

		public ArrayArgumentInterpretation ArrayArgument
		{
			get
			{
				return this.arrayArgument;
			}
		}

		internal bool ConvertDateTextToNumber
		{
			get
			{
				return this.convertDateTextToNumber;
			}
		}

		public ArgumentConversionRules(ArgumentConversionRules conversionRules, ArrayArgumentInterpretation arrayArgument)
			: this(conversionRules.EmptyDirectArgument, conversionRules.NumberDirectArgument, conversionRules.BoolDirectArgument, conversionRules.TextNumberDirectArgument, conversionRules.NonTextNumberDirectArgument, conversionRules.EmptyIndirectArgument, conversionRules.NumberIndirectArgument, conversionRules.BoolIndirectArgument, conversionRules.TextNumberIndirectArgument, conversionRules.NonTextNumberIndirectArgument, arrayArgument)
		{
		}

		public ArgumentConversionRules(ArgumentInterpretation emptyDirectArgument = ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation numberDirectArgument = ArgumentInterpretation.UseAsIs, ArgumentInterpretation boolDirectArgument = ArgumentInterpretation.UseAsIs, ArgumentInterpretation textNumberDirectArgument = ArgumentInterpretation.UseAsIs, ArgumentInterpretation nonTextNumberDirectArgument = ArgumentInterpretation.UseAsIs, ArgumentInterpretation emptyIndirectArgument = ArgumentInterpretation.UseAsIs, ArgumentInterpretation numberIndirectArgument = ArgumentInterpretation.UseAsIs, ArgumentInterpretation boolIndirectArgument = ArgumentInterpretation.UseAsIs, ArgumentInterpretation textNumberIndirectArgument = ArgumentInterpretation.UseAsIs, ArgumentInterpretation nonTextNumberIndirectArgument = ArgumentInterpretation.UseAsIs, ArrayArgumentInterpretation arrayArgument = ArrayArgumentInterpretation.UseFirstElement)
		{
			this.emptyDirectArgument = emptyDirectArgument;
			this.numberDirectArgument = numberDirectArgument;
			this.boolDirectArgument = boolDirectArgument;
			this.textNumberDirectArgument = textNumberDirectArgument;
			this.nonTextNumberDirectArgument = nonTextNumberDirectArgument;
			this.emptyIndirectArgument = emptyIndirectArgument;
			this.numberIndirectArgument = numberIndirectArgument;
			this.boolIndirectArgument = boolIndirectArgument;
			this.textNumberIndirectArgument = textNumberIndirectArgument;
			this.nonTextNumberIndirectArgument = nonTextNumberIndirectArgument;
			this.arrayArgument = arrayArgument;
		}

		internal ArgumentConversionRules(ArgumentConversionRules conversionRules, bool convertDateTextToNumber)
			: this(conversionRules.EmptyDirectArgument, conversionRules.NumberDirectArgument, conversionRules.BoolDirectArgument, conversionRules.TextNumberDirectArgument, conversionRules.NonTextNumberDirectArgument, conversionRules.EmptyIndirectArgument, conversionRules.NumberIndirectArgument, conversionRules.BoolIndirectArgument, conversionRules.TextNumberIndirectArgument, conversionRules.NonTextNumberIndirectArgument, conversionRules.ArrayArgument)
		{
			this.convertDateTextToNumber = convertDateTextToNumber;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ArgumentConversionRules()
		{
			ArgumentInterpretation argumentInterpretation = ArgumentInterpretation.ConvertToDefault;
			ArgumentInterpretation argumentInterpretation2 = ArgumentInterpretation.TreatAsError;
			ArgumentConversionRules.NonBoolNumberFunctionConversion = new ArgumentConversionRules(argumentInterpretation, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, argumentInterpretation2, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArrayArgumentInterpretation.UseFirstElement);
			ArgumentConversionRules.NaryNumberFunctionConversion = new ArgumentConversionRules(ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArrayArgumentInterpretation.UseAllElements);
			ArgumentInterpretation argumentInterpretation3 = ArgumentInterpretation.Ignore;
			ArgumentInterpretation argumentInterpretation4 = ArgumentInterpretation.ConvertToDefault;
			ArgumentInterpretation argumentInterpretation5 = ArgumentInterpretation.UseAsIs;
			ArgumentInterpretation argumentInterpretation6 = ArgumentInterpretation.TreatAsError;
			ArgumentConversionRules.NonBoolNaryFunctionConversion = new ArgumentConversionRules(argumentInterpretation4, argumentInterpretation5, ArgumentInterpretation.TreatAsError, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, argumentInterpretation3, ArgumentInterpretation.UseAsIs, argumentInterpretation6, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArrayArgumentInterpretation.UseAllElements);
			ArgumentInterpretation argumentInterpretation7 = ArgumentInterpretation.ConvertToDefault;
			ArgumentConversionRules.DefaultValueNumberFunctionConversion = new ArgumentConversionRules(ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, argumentInterpretation7, ArgumentInterpretation.ConvertToDefault, ArrayArgumentInterpretation.UseAllElements);
			ArgumentInterpretation argumentInterpretation8 = ArgumentInterpretation.Ignore;
			ArgumentInterpretation argumentInterpretation9 = ArgumentInterpretation.Ignore;
			ArgumentInterpretation argumentInterpretation10 = ArgumentInterpretation.Ignore;
			ArgumentConversionRules.NaryIgnoreIndirectNumberFunctionConversion = new ArgumentConversionRules(ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, argumentInterpretation8, ArgumentInterpretation.UseAsIs, argumentInterpretation9, argumentInterpretation10, ArgumentInterpretation.Ignore, ArrayArgumentInterpretation.UseAllElements);
			ArgumentInterpretation argumentInterpretation11 = ArgumentInterpretation.Ignore;
			ArgumentInterpretation argumentInterpretation12 = ArgumentInterpretation.ConvertToDefault;
			ArgumentInterpretation argumentInterpretation13 = ArgumentInterpretation.UseAsIs;
			ArgumentInterpretation argumentInterpretation14 = ArgumentInterpretation.Ignore;
			ArgumentConversionRules.BoolFunctionConversion = new ArgumentConversionRules(argumentInterpretation12, argumentInterpretation13, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArgumentInterpretation.TreatAsError, argumentInterpretation11, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, argumentInterpretation14, ArgumentInterpretation.Ignore, ArrayArgumentInterpretation.UseAllElements);
		}

		public static readonly ArgumentConversionRules NumberFunctionConversion = new ArgumentConversionRules(ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArrayArgumentInterpretation.UseFirstElement);

		public static readonly ArgumentConversionRules NonBoolNumberFunctionConversion;

		public static readonly ArgumentConversionRules NaryNumberFunctionConversion;

		public static readonly ArgumentConversionRules NonBoolNaryFunctionConversion;

		public static readonly ArgumentConversionRules DefaultValueNumberFunctionConversion;

		public static readonly ArgumentConversionRules NaryIgnoreIndirectNumberFunctionConversion;

		public static readonly ArgumentConversionRules BoolFunctionConversion;

		readonly ArgumentInterpretation emptyDirectArgument;

		readonly ArgumentInterpretation numberDirectArgument;

		readonly ArgumentInterpretation boolDirectArgument;

		readonly ArgumentInterpretation textNumberDirectArgument;

		readonly ArgumentInterpretation nonTextNumberDirectArgument;

		readonly ArgumentInterpretation emptyIndirectArgument;

		readonly ArgumentInterpretation numberIndirectArgument;

		readonly ArgumentInterpretation boolIndirectArgument;

		readonly ArgumentInterpretation textNumberIndirectArgument;

		readonly ArgumentInterpretation nonTextNumberIndirectArgument;

		readonly ArrayArgumentInterpretation arrayArgument;

		readonly bool convertDateTextToNumber;
	}
}
