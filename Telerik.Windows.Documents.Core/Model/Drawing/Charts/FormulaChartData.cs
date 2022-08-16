using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public abstract class FormulaChartData : IChartData
	{
		protected FormulaChartData(string formula)
		{
			this.formula = formula;
		}

		public ChartDataType ChartDataType
		{
			get
			{
				return ChartDataType.Formula;
			}
		}

		public string Formula
		{
			get
			{
				return this.formula;
			}
		}

		public abstract IChartData Clone();

		readonly string formula;
	}
}
