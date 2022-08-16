using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	static class ChartTypeToAxesTypeMapper
	{
		static ChartTypeToAxesTypeMapper()
		{
			ChartTypeToAxesTypeMapper.valuePairs.Add(ChartType.Area, AxesType.CategoriesValues);
			ChartTypeToAxesTypeMapper.valuePairs.Add(ChartType.Bar, AxesType.CategoriesValues);
			ChartTypeToAxesTypeMapper.valuePairs.Add(ChartType.Column, AxesType.CategoriesValues);
			ChartTypeToAxesTypeMapper.valuePairs.Add(ChartType.Doughnut, AxesType.CategoriesValues);
			ChartTypeToAxesTypeMapper.valuePairs.Add(ChartType.Line, AxesType.CategoriesValues);
			ChartTypeToAxesTypeMapper.valuePairs.Add(ChartType.Pie, AxesType.CategoriesValues);
			ChartTypeToAxesTypeMapper.valuePairs.Add(ChartType.Scatter, AxesType.XValuesYValues);
			ChartTypeToAxesTypeMapper.valuePairs.Add(ChartType.Bubble, AxesType.XValuesYValues);
		}

		internal static AxesType GetAxesType(ChartType seriesType)
		{
			return ChartTypeToAxesTypeMapper.valuePairs[seriesType];
		}

		static readonly Dictionary<ChartType, AxesType> valuePairs = new Dictionary<ChartType, AxesType>();
	}
}
