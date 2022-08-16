using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class SheetCollection : IEnumerable<Sheet>, IEnumerable
	{
		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		public Sheet ActiveSheet
		{
			get
			{
				return this.activeSheet;
			}
			set
			{
				int num = this.IndexOf(value);
				if (num == -1)
				{
					throw new InvalidOperationException("The value must belong to the same workbook.");
				}
				this.UpdateActiveSheetAndIndex(value, num);
			}
		}

		public int ActiveSheetIndex
		{
			get
			{
				return this.activeSheetIndex;
			}
			set
			{
				Guard.ThrowExceptionIfOutOfRange<int>(-1, this.Count - 1, value, "value");
				Sheet sheet = ((value != -1) ? this.innerList[value] : null);
				this.UpdateActiveSheetAndIndex(sheet, value);
			}
		}

		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		public Sheet this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		public Sheet this[string sheetName]
		{
			get
			{
				return this.GetByName(sheetName);
			}
		}

		internal SheetCollection(Workbook workbook)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			this.activeSheetIndex = -1;
			this.workbook = workbook;
			this.innerList = new List<Sheet>();
		}

		string GetNextSheetName(SheetType type)
		{
			int num = 0;
			foreach (Sheet sheet in this)
			{
				if (sheet.Type == type)
				{
					num++;
				}
			}
			num++;
			string sheetNamePrefixBySheetType = SpreadsheetHelper.GetSheetNamePrefixBySheetType(type);
			string text = sheetNamePrefixBySheetType + num;
			while (this.Contains(text))
			{
				num++;
				text = sheetNamePrefixBySheetType + num;
			}
			return text;
		}

		Sheet CreateSheet(SheetType type)
		{
			string nextSheetName = this.GetNextSheetName(type);
			return SheetFactory.Create(type, nextSheetName, this.workbook);
		}

		void UpdateActiveSheetAndIndex(Sheet sheet, int index)
		{
			if (this.activeSheetIndex != index)
			{
				this.activeSheetIndex = index;
				this.OnActiveSheetIndexChanged();
			}
			if (this.activeSheet != sheet)
			{
				this.activeSheet = sheet;
				this.OnActiveSheetChanged();
			}
		}

		void UpdateActiveSheetIndex()
		{
			this.UpdateActiveSheetAndIndex(this.ActiveSheet, this.IndexOf(this.ActiveSheet));
		}

		void UpdateActiveSheet()
		{
			this.UpdateActiveSheetAndIndex(this[this.ActiveSheetIndex], this.ActiveSheetIndex);
		}

		public Sheet Add(SheetType type)
		{
			return this.Insert(this.Count, type);
		}

		public Sheet Insert(SheetType type)
		{
			return this.Insert(this.ActiveSheetIndex, type);
		}

		public Sheet Insert(int index, SheetType type)
		{
			Sheet sheet = this.CreateSheet(type);
			if (this.Workbook.IsLayoutUpdateSuspended)
			{
				ISheet sheet2 = sheet;
				sheet2.SuspendLayoutUpdate();
			}
			if (type == SheetType.Worksheet && this.Workbook.IsPropertyChangeSuspended)
			{
				Worksheet worksheet = (Worksheet)sheet;
				worksheet.Cells.PropertyBag.SuspendPropertyChanged();
			}
			this.OnItemAdding(sheet);
			this.innerList.Insert(index, sheet);
			this.OnItemAdded(sheet);
			if (this.ActiveSheetIndex == -1)
			{
				this.ActiveSheetIndex = 0;
			}
			else if (this.workbook.ActiveTabIndex == index)
			{
				this.UpdateActiveSheetAndIndex(sheet, index);
			}
			return sheet;
		}

		public int IndexOf(Sheet sheet)
		{
			return this.innerList.IndexOf(sheet);
		}

		public int IndexOf(string sheetName)
		{
			for (int i = 0; i < this.innerList.Count; i++)
			{
				if (SheetCollection.AreEqualIgnoreCaseSheetNames(this.innerList[i].Name, sheetName))
				{
					return i;
				}
			}
			return -1;
		}

		public bool Contains(string sheetName)
		{
			return this.IndexOf(sheetName) >= 0;
		}

		public bool Contains(Sheet sheet)
		{
			return this.IndexOf(sheet) >= 0;
		}

		public Sheet GetByName(string sheetName)
		{
			foreach (Sheet sheet in this)
			{
				if (SheetCollection.AreEqualIgnoreCaseSheetNames(sheet.Name, sheetName))
				{
					return sheet;
				}
			}
			return null;
		}

		public void Remove()
		{
			this.RemoveAt(this.ActiveSheetIndex);
		}

		public bool Remove(string sheetName)
		{
			Sheet byName = this.GetByName(sheetName);
			return this.Remove(byName);
		}

		public bool Remove(Sheet sheet)
		{
			int num = this.innerList.IndexOf(sheet);
			if (num == -1)
			{
				return false;
			}
			this.RemoveAt(num);
			return true;
		}

		public void RemoveAt(int index)
		{
			Sheet sheet = this.innerList[index];
			this.OnItemRemoving(sheet);
			this.innerList.RemoveAt(index);
			if (this.ActiveSheetIndex == index)
			{
				if (this.ActiveSheetIndex == this.Count)
				{
					this.ActiveSheetIndex--;
				}
				else
				{
					this.UpdateActiveSheet();
				}
			}
			else
			{
				this.UpdateActiveSheetIndex();
			}
			this.OnItemRemoved(sheet);
		}

		public void Move(int fromIndex, int itemCount, int toIndex)
		{
			if (fromIndex == toIndex)
			{
				return;
			}
			Stack<Sheet> stack = new Stack<Sheet>();
			for (int i = itemCount - 1; i >= 0; i--)
			{
				stack.Push(this.innerList[fromIndex + i]);
				this.innerList.RemoveAt(fromIndex + i);
			}
			if (toIndex > fromIndex)
			{
				toIndex -= itemCount;
			}
			this.innerList.InsertRange(toIndex, stack);
			this.OnChanged(SheetCollectionChangeType.Move, null);
			this.UpdateActiveSheetIndex();
		}

		public IEnumerator<Sheet> GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.innerList).GetEnumerator();
		}

		internal static bool IsValidSheetName(string sheetName)
		{
			bool flag = !string.IsNullOrEmpty(sheetName);
			if (flag)
			{
				flag &= sheetName[0] != '\'';
				flag &= sheetName[sheetName.Length - 1] != '\'';
				flag &= sheetName.Length <= 31;
				foreach (char item in sheetName)
				{
					if (!flag)
					{
						break;
					}
					flag &= !SheetCollection.forbiddenSheetNameSymbols.Contains(item);
				}
			}
			return flag;
		}

		internal bool IsUniqueSheetName(string sheetName)
		{
			return !this.Contains(sheetName);
		}

		internal bool IsUniqueSheetNameInputed(string inputedName, string oldName)
		{
			return SheetCollection.AreEqualIgnoreCaseSheetNames(inputedName, oldName) || this.IsUniqueSheetName(inputedName);
		}

		internal static bool AreEqualIgnoreCaseSheetNames(string firstName, string secondName)
		{
			return firstName.Equals(secondName, StringComparison.CurrentCultureIgnoreCase);
		}

		void OnItemAdding(Sheet sheet)
		{
			this.OnChanging(SheetCollectionChangeType.Add, sheet);
		}

		void OnItemAdded(Sheet sheet)
		{
			this.OnChanged(SheetCollectionChangeType.Add, sheet);
		}

		void OnItemRemoving(Sheet sheet)
		{
			this.OnChanging(SheetCollectionChangeType.Remove, sheet);
		}

		void OnItemRemoved(Sheet sheet)
		{
			sheet.Dispose();
			this.OnChanged(SheetCollectionChangeType.Remove, null);
		}

		public event EventHandler ActiveSheetIndexChanged;

		protected virtual void OnActiveSheetIndexChanged()
		{
			if (this.ActiveSheetIndexChanged != null)
			{
				this.ActiveSheetIndexChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler ActiveSheetChanged;

		protected virtual void OnActiveSheetChanged()
		{
			if (this.ActiveSheetChanged != null)
			{
				this.ActiveSheetChanged(this, EventArgs.Empty);
			}
		}

		void OnChanging(SheetCollectionChangeType change, Sheet sheet)
		{
			this.OnChanging(new SheetCollectionChangedEventArgs(change, sheet));
		}

		internal event EventHandler<SheetCollectionChangedEventArgs> Changing;

		void OnChanging(SheetCollectionChangedEventArgs args)
		{
			if (this.Changing != null)
			{
				this.Changing(this, args);
			}
		}

		void OnChanged(SheetCollectionChangeType change, Sheet sheet = null)
		{
			this.OnChanged(new SheetCollectionChangedEventArgs(change, sheet));
		}

		public event EventHandler<SheetCollectionChangedEventArgs> Changed;

		protected virtual void OnChanged(SheetCollectionChangedEventArgs args)
		{
			if (this.Changed != null)
			{
				this.Changed(this, args);
			}
		}

		const int MaxSheetNameLength = 31;

		readonly Workbook workbook;

		readonly List<Sheet> innerList;

		Sheet activeSheet;

		int activeSheetIndex;

		static readonly HashSet<char> forbiddenSheetNameSymbols = new HashSet<char>(new char[] { '\\', '/', '?', '*', '[', ']', ':' });
	}
}
