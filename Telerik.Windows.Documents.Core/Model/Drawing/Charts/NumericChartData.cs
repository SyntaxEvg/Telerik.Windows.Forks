using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class NumericChartData : IChartData
	{
		public NumericChartData(IEnumerable<double> numericLiterals)
		{
			this.numericLiterals = numericLiterals;
		}

		public ChartDataType ChartDataType
		{
			get
			{
				return ChartDataType.NumericLiteral;
			}
		}

		public IEnumerable<double> NumericLiterals
		{
			get
			{
				return this.numericLiterals;
			}
		}

		public IChartData Clone()
		{
			return new NumericChartData(new List<double>(this.NumericLiterals));
		}

		readonly IEnumerable<double> numericLiterals;
	}
}
