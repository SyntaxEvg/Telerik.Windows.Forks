using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class FormulaTitle : Title
	{
		public FormulaTitle(FormulaChartData formulaChartData)
		{
			if (formulaChartData == null)
			{
				throw new ArgumentNullException("formulaChartData", "The title data cannot be null.");
			}
			this.formula = formulaChartData;
		}

		public override TitleType TitleType
		{
			get
			{
				return TitleType.Formula;
			}
		}

		public FormulaChartData Formula
		{
			get
			{
				return this.formula;
			}
		}

		public override Title Clone()
		{
			return new FormulaTitle(this.Formula.Clone() as FormulaChartData);
		}

		readonly FormulaChartData formula;
	}
}
