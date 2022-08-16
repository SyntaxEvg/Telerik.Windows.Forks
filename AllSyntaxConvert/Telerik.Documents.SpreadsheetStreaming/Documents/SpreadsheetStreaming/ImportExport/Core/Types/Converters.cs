using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	static class Converters
	{
		public static IntConverter IntValueConverter
		{
			get
			{
				if (Converters.intValueConverter == null)
				{
					Converters.intValueConverter = new IntConverter();
				}
				return Converters.intValueConverter;
			}
		}

		public static BoolConverter BoolValueConverter
		{
			get
			{
				if (Converters.boolValueConverter == null)
				{
					Converters.boolValueConverter = new BoolConverter();
				}
				return Converters.boolValueConverter;
			}
		}

		public static UnsignedIntHexConverter UnsignedIntHexConverter
		{
			get
			{
				if (Converters.unsignedIntHexConverter == null)
				{
					Converters.unsignedIntHexConverter = new UnsignedIntHexConverter();
				}
				return Converters.unsignedIntHexConverter;
			}
		}

		public static NullableIntConverter NullableIntValueConverter
		{
			get
			{
				if (Converters.nullableIntValueConverter == null)
				{
					Converters.nullableIntValueConverter = new NullableIntConverter();
				}
				return Converters.nullableIntValueConverter;
			}
		}

		public static CellRefConverter CellRefConverter
		{
			get
			{
				if (Converters.cellRefConverter == null)
				{
					Converters.cellRefConverter = new CellRefConverter();
				}
				return Converters.cellRefConverter;
			}
		}

		public static RefConverter RefConverter
		{
			get
			{
				if (Converters.refConverter == null)
				{
					Converters.refConverter = new RefConverter();
				}
				return Converters.refConverter;
			}
		}

		public static CellRefRangeSequenceConverter CellRefRangeSequenceConverter
		{
			get
			{
				if (Converters.cellRefRangeSequenceConverter == null)
				{
					Converters.cellRefRangeSequenceConverter = new CellRefRangeSequenceConverter();
				}
				return Converters.cellRefRangeSequenceConverter;
			}
		}

		static IntConverter intValueConverter;

		static BoolConverter boolValueConverter;

		static UnsignedIntHexConverter unsignedIntHexConverter;

		static NullableIntConverter nullableIntValueConverter;

		static CellRefConverter cellRefConverter;

		static RefConverter refConverter;

		static CellRefRangeSequenceConverter cellRefRangeSequenceConverter;
	}
}
