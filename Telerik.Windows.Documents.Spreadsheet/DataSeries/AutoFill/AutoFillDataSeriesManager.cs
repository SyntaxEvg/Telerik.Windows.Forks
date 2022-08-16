using System;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class AutoFillDataSeriesManager
	{
		public NamedObjectList<IAutoFillGenerator> AutoFillGenerators
		{
			get
			{
				return this.autoFillGenerators;
			}
		}

		public AutoFillDataSeriesManager(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
			this.autoFillGenerators = new NamedObjectList<IAutoFillGenerator>();
			this.InitBuiltInGenerators();
		}

		void InitBuiltInGenerators()
		{
			this.autoFillGenerators.AddLast(new RepeatAutoFillGenerator());
			this.autoFillGenerators.AddLast(new NumberAutoFillGenerator());
			this.autoFillGenerators.AddLast(new NumberedTextAutoFillGenerator());
			this.autoFillGenerators.AddLast(new QuarterAutoFillGenerator());
			this.autoFillGenerators.AddLast(new ShortLowerCaseWeekDaysAutoFillGenerator());
			this.autoFillGenerators.AddLast(new ShortUpperCaseWeekDaysAutoFillGenerator());
			this.autoFillGenerators.AddLast(new LongLowerCaseWeekDaysAutoFillGenerator());
			this.autoFillGenerators.AddLast(new LongUpperCaseWeekDaysAutoFillGenerator());
			this.autoFillGenerators.AddLast(new ShortLowerCaseMonthsAutoFillGenerator());
			this.autoFillGenerators.AddLast(new ShortUpperCaseMonthsAutoFillGenerator());
			this.autoFillGenerators.AddLast(new LongLowerCaseMonthsAutoFillGenerator());
			this.autoFillGenerators.AddLast(new LongUpperCaseMonthsAutoFillGenerator());
			this.autoFillGenerators.AddLast(new OrdinalNumbersFillGenerator());
		}

		public void FillAuto(CellIndex[] indexes, int initialIndexesCount)
		{
			foreach (IAutoFillGenerator autoFillGenerator in this.autoFillGenerators)
			{
				autoFillGenerator.FillSeries(this.worksheet, indexes, initialIndexesCount);
			}
		}

		readonly NamedObjectList<IAutoFillGenerator> autoFillGenerators;

		readonly Worksheet worksheet;
	}
}
