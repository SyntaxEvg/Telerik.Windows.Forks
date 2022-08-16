using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class XlsxConverters
	{
		public static ErrorsPrintStyleConverter ErrorsPrintStyleConverter
		{
			get
			{
				return XlsxConverters.errorsPrintStyleConverter;
			}
		}

		public static PageOrderConverter PageOrderConverter
		{
			get
			{
				return XlsxConverters.pageOrderConverter;
			}
		}

		public static PaperTypesConverter PaperTypesConverter
		{
			get
			{
				return XlsxConverters.paperTypesConverter;
			}
		}

		public static ComparisonOperatorConverter ComparisonOperatorConverter
		{
			get
			{
				return XlsxConverters.comparisonOperatorConverter;
			}
		}

		public static DynamicFilterTypeConverter DynamicFilterTypeConverter
		{
			get
			{
				return XlsxConverters.dynamicFilterTypeConverter;
			}
		}

		public static DateTimeGroupingTypeConverter DateTimeGroupingTypeConverter
		{
			get
			{
				return XlsxConverters.dateTimeGroupingTypeConverter;
			}
		}

		public static CellRefConverter CellRefConverter
		{
			get
			{
				return XlsxConverters.cellRefConverter;
			}
		}

		public static CellRefRangeConverter CellRefRangeConverter
		{
			get
			{
				return XlsxConverters.cellRefRangeConverter;
			}
		}

		public static RefConverter RefConverter
		{
			get
			{
				return XlsxConverters.refConverter;
			}
		}

		public static CellRefRangeSequenceConverter CellRefRangeSequenceConverter
		{
			get
			{
				return XlsxConverters.cellRefRangeSequenceConverter;
			}
		}

		public static ErrorStyleConverter ErrorStyleConverter
		{
			get
			{
				return XlsxConverters.errorStyleConverter;
			}
		}

		public static DataValidationRuleTypeConverter DataValidationRuleTypeConverter
		{
			get
			{
				return XlsxConverters.dataValidationRuleTypeConverter;
			}
		}

		static readonly ErrorsPrintStyleConverter errorsPrintStyleConverter = new ErrorsPrintStyleConverter();

		static readonly PageOrderConverter pageOrderConverter = new PageOrderConverter();

		static readonly PaperTypesConverter paperTypesConverter = new PaperTypesConverter();

		static readonly ComparisonOperatorConverter comparisonOperatorConverter = new ComparisonOperatorConverter();

		static readonly DynamicFilterTypeConverter dynamicFilterTypeConverter = new DynamicFilterTypeConverter();

		static readonly DateTimeGroupingTypeConverter dateTimeGroupingTypeConverter = new DateTimeGroupingTypeConverter();

		static readonly CellRefConverter cellRefConverter = new CellRefConverter();

		static readonly CellRefRangeConverter cellRefRangeConverter = new CellRefRangeConverter();

		static readonly RefConverter refConverter = new RefConverter();

		static readonly CellRefRangeSequenceConverter cellRefRangeSequenceConverter = new CellRefRangeSequenceConverter();

		static readonly ErrorStyleConverter errorStyleConverter = new ErrorStyleConverter();

		static readonly DataValidationRuleTypeConverter dataValidationRuleTypeConverter = new DataValidationRuleTypeConverter();
	}
}
