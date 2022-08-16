using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Web.Spreadsheet
{
	public static class CommonExtensions
	{
		public static string ToPixels(this double value)
		{
			return Math.Round(value).ToString() + "px";
		}

		public static CellIndex ToCellIndex(this string cellRef)
		{
			CellRange cellRange;
			if (NameConverter.TryConvertCellNameToCellRange(cellRef, out cellRange))
			{
				return cellRange.FromIndex;
			}
			return new CellIndex(0, 0);
		}

		public static List<NamedRange> GetNamedRanges(this IEnumerable<ISpreadsheetName> spreadsheetNames)
		{
			List<NamedRange> list = new List<NamedRange>();
			foreach (ISpreadsheetName spreadsheetName in spreadsheetNames)
			{
				list.Add(new NamedRange
				{
					Name = spreadsheetName.Name,
					Value = spreadsheetName.RefersTo.Substring(1),
					Hidden = new bool?(!spreadsheetName.IsVisible)
				});
			}
			return list;
		}

		public static IEnumerable<CellRange> ToCellRange(this string refs)
		{
			List<CellRange> list = new List<CellRange>();
			CellRange item = null;
			string[] array = refs.Split(new char[] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				if (NameConverter.TryConvertCellNameToCellRange(array[i].Trim(), out item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static List<T> GetOrDefault<T>(this List<T> collection)
		{
			return collection ?? new List<T>();
		}

		public static string ToCamelCase(this string instance)
		{
			return instance[0].ToString().ToLowerInvariant() + instance.Substring(1);
		}

		public static bool HasValue(this string value)
		{
			return !string.IsNullOrEmpty(value);
		}

		public static T ToEnum<T>(this string value, T defaultValue)
		{
			if (!value.HasValue())
			{
				return defaultValue;
			}
			T result;
			try
			{
				result = (T)((object)Enum.Parse(typeof(T), value, true));
			}
			catch (ArgumentException)
			{
				result = defaultValue;
			}
			return result;
		}
	}
}
