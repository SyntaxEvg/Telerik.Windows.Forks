using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class StringChartData : IChartData
	{
		public StringChartData(IEnumerable<string> stringLiterals)
		{
			this.stringLiterals = stringLiterals;
		}

		public ChartDataType ChartDataType
		{
			get
			{
				return ChartDataType.StringLiteral;
			}
		}

		public IEnumerable<string> StringLiterals
		{
			get
			{
				return this.stringLiterals;
			}
		}

		public IChartData Clone()
		{
			return new StringChartData(new List<string>(this.StringLiterals));
		}

		readonly IEnumerable<string> stringLiterals;
	}
}
