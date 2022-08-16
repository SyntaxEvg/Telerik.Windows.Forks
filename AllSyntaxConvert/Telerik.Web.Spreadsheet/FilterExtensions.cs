using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Web.Spreadsheet
{
	public static class FilterExtensions
	{
		public static FilterColumn ToFilterColumn(this IFilter filter)
		{
			if (filter is TopFilter)
			{
				return FilterExtensions.ToFilterColumn((TopFilter)filter);
			}
			if (filter is ValuesCollectionFilter)
			{
				return FilterExtensions.ToFilterColumn((ValuesCollectionFilter)filter);
			}
			if (filter is DynamicFilter)
			{
				return FilterExtensions.ToFilterColumn((DynamicFilter)filter);
			}
			if (filter is CustomFilter)
			{
				return FilterExtensions.ToFilterColumn((CustomFilter)filter);
			}
			return null;
		}

		static FilterColumn ToFilterColumn(TopFilter filter)
		{
			return new FilterColumn
			{
				Index = new double?((double)filter.RelativeColumnIndex),
				Filter = "top",
				Type = filter.TopFilterType.ToString().ToCamelCase(),
				Value = new double?(filter.Value)
			};
		}

		static FilterColumn ToFilterColumn(DynamicFilter filter)
		{
			return new FilterColumn
			{
				Filter = "dynamic",
				Index = new double?((double)filter.RelativeColumnIndex),
				Type = filter.DynamicFilterType.ToString().ToCamelCase()
			};
		}

		static FilterColumn ToFilterColumn(ValuesCollectionFilter filter)
		{
			FilterColumn filterColumn = new FilterColumn();
			filterColumn.Index = new double?((double)filter.RelativeColumnIndex);
			filterColumn.Filter = "value";
			filterColumn.Blanks = new bool?(filter.Blank);
			filterColumn.Dates = (from item in filter.DateItems
				select new ValueFilterDate
				{
					Year = item.Year,
					Month = ((item.Month == 0) ? item.Month : (item.Month - 1)),
					Day = item.Day,
					Hours = item.Hour,
					Minutes = item.Minute,
					Seconds = item.Second
				}).ToList<ValueFilterDate>();
			filterColumn.Values = (from value in filter.StringValues
				select FilterExtensions.ParseValue(value)).ToList<object>();
			return filterColumn;
		}

		static FilterColumn ToFilterColumn(CustomFilter filter)
		{
			return new FilterColumn
			{
				Filter = "custom",
				Index = new double?((double)filter.RelativeColumnIndex),
				Logic = filter.LogicalOperator.ToString().ToLowerInvariant(),
				Criteria = FilterExtensions.CreateFilterCriteria(filter)
			};
		}

		static List<Criteria> CreateFilterCriteria(CustomFilter filter)
		{
			List<Criteria> list = new List<Criteria>();
			if (filter.Criteria1 != null)
			{
				list.Add(FilterExtensions.ToCriteria(filter.Criteria1));
			}
			if (filter.Criteria2 != null)
			{
				list.Add(FilterExtensions.ToCriteria(filter.Criteria2));
			}
			return list;
		}

		static Criteria ToCriteria(CustomFilterCriteria source)
		{
			string @operator = FilterExtensions.ComparisonOperators[source.ComparisonOperator.ToString().ToLowerInvariant()];
			object obj = FilterExtensions.ParseValue(source.FilterValue);
			if (obj is string && (source.ComparisonOperator == ComparisonOperator.EqualsTo || source.ComparisonOperator == ComparisonOperator.NotEqualsTo))
			{
				StringFilterOpperators stringFilterOpperators = (obj.ToString().EndsWith("*") ? StringFilterOpperators.StartsWith : StringFilterOpperators.EqualsTo);
				StringFilterOpperators stringFilterOpperators2 = (obj.ToString().StartsWith("*") ? StringFilterOpperators.EndsWith : StringFilterOpperators.EqualsTo);
				int num = (int)(source.ComparisonOperator | (ComparisonOperator)stringFilterOpperators | (ComparisonOperator)stringFilterOpperators2);
				@operator = FilterExtensions.ComparisonOperators[Enum.GetName(typeof(StringFilterOpperators), num).ToLowerInvariant()];
				obj = obj.ToString().TrimStart(new char[] { '*' }).TrimEnd(new char[] { '*' });
			}
			return new Criteria
			{
				Operator = @operator,
				Value = obj
			};
		}

		static object ParseValue(string value)
		{
			double num;
			if (double.TryParse(value, out num))
			{
				return num;
			}
			bool flag;
			if (bool.TryParse(value, out flag))
			{
				return flag;
			}
			return value;
		}

		static readonly Dictionary<string, string> ComparisonOperators = new Dictionary<string, string>
		{
			{ "contains", "contains" },
			{ "doesnotcontain", "doesnotcontain" },
			{ "startswith", "startswith" },
			{ "endswith", "endswith" },
			{ "equalsto", "eq" },
			{ "notequalsto", "neq" },
			{ "lessthan", "lt" },
			{ "greaterthan", "gt" },
			{ "greaterthanorequalsto", "gte" },
			{ "lessthanorequalsto", "lte" }
		};
	}
}
