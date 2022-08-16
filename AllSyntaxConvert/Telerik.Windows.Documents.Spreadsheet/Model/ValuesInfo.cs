using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	class ValuesInfo
	{
		public List<CellIndex[]> CellIndexLists
		{
			get
			{
				if (this.cellIndexLists == null)
				{
					this.cellIndexLists = new List<CellIndex[]>();
				}
				return this.cellIndexLists;
			}
			set
			{
				if (this.cellIndexLists != value)
				{
					this.cellIndexLists = value;
				}
			}
		}

		public List<int> InitialValuesCounts
		{
			get
			{
				if (this.initialValuesCounts == null)
				{
					this.initialValuesCounts = new List<int>();
				}
				return this.initialValuesCounts;
			}
			set
			{
				if (this.initialValuesCounts != value)
				{
					this.initialValuesCounts = value;
				}
			}
		}

		public List<ICellValue[]> InitialValuesList
		{
			get
			{
				if (this.initialValuesList == null)
				{
					this.initialValuesList = new List<ICellValue[]>();
				}
				return this.initialValuesList;
			}
			set
			{
				if (this.initialValuesList != value)
				{
					this.initialValuesList = value;
				}
			}
		}

		public int MaxInitialValuesCount
		{
			get
			{
				int maxIndexesCount = (from p in this.CellIndexLists
					select p.Length).Max();
				int num = (from p in this.InitialValuesCounts
					where p < maxIndexesCount
					select p).FirstOrDefault<int>();
				if (num >= maxIndexesCount || num <= 0)
				{
					return maxIndexesCount;
				}
				return num;
			}
		}

		public ValuesInfo()
		{
		}

		public ValuesInfo(List<CellIndex[]> cellIndexLists, List<int> initialValuesCounts, List<ICellValue[]> initialValuesList)
		{
			this.cellIndexLists = cellIndexLists;
			this.initialValuesCounts = initialValuesCounts;
			this.initialValuesList = initialValuesList;
		}

		List<CellIndex[]> cellIndexLists;

		List<int> initialValuesCounts;

		List<ICellValue[]> initialValuesList;
	}
}
