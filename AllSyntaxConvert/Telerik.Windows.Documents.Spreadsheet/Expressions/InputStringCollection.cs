using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class InputStringCollection : IEnumerable<UserInputStringBase>, IEnumerable
	{
		public bool ContainsSpreadsheetName
		{
			get
			{
				return this.innerList.Any((UserInputStringBase item) => item is DefinedNameInputString);
			}
		}

		public bool IsSheetNameDependent
		{
			get
			{
				return this.innerList.Any((UserInputStringBase item) => item.IsSheetNameDependent);
			}
		}

		public bool IsWorkbookNameDependent
		{
			get
			{
				return this.innerList.Any((UserInputStringBase item) => item.IsWorkbookNameDependent);
			}
		}

		public bool IsCellsNameDependent
		{
			get
			{
				return this.innerList.Any((UserInputStringBase item) => item is CellReferenceInputString);
			}
		}

		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		public UserInputStringBase this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		public InputStringCollection()
		{
			this.innerList = new List<UserInputStringBase>();
		}

		public void Add(UserInputStringBase stringExpression)
		{
			this.innerList.Add(stringExpression);
		}

		public string BuildStringInCulture(SpreadsheetCultureHelper cultureInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.innerList.Count; i++)
			{
				stringBuilder.Append(this.innerList[i].ToString(cultureInfo));
			}
			return stringBuilder.ToString();
		}

		public string BuildString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.innerList.Count; i++)
			{
				stringBuilder.Append(this.innerList[i].ToString(FormatHelper.DefaultSpreadsheetCulture));
			}
			return stringBuilder.ToString();
		}

		public InputStringCollection Clone()
		{
			InputStringCollection inputStringCollection = new InputStringCollection();
			for (int i = 0; i < this.innerList.Count; i++)
			{
				inputStringCollection.Add(this.innerList[i].Clone());
			}
			return inputStringCollection;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<UserInputStringBase>)this).GetEnumerator();
		}

		public IEnumerator<UserInputStringBase> GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		readonly List<UserInputStringBase> innerList;
	}
}
