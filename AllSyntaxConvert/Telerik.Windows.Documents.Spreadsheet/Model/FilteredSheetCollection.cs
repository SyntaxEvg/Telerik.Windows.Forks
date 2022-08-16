using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class FilteredSheetCollection<T> : IEnumerable<T>, IEnumerable where T : Sheet
	{
		public Workbook Workbook
		{
			get
			{
				return this.sheetCollection.Workbook;
			}
		}

		public int Count
		{
			get
			{
				return this.GetCount();
			}
		}

		public T this[int index]
		{
			get
			{
				return this.GetAllOfT()[index];
			}
		}

		public T this[string sheetName]
		{
			get
			{
				return this.GetByName(sheetName);
			}
		}

		internal FilteredSheetCollection(SheetCollection sheetCollection)
		{
			Guard.ThrowExceptionIfNull<SheetCollection>(sheetCollection, "sheetCollection");
			this.Invalidate();
			this.sheetCollection = sheetCollection;
			this.sheetCollection.Changed += this.SheetCollection_Changed;
			this.sheetType = SpreadsheetHelper.GetSheetType<T>();
		}

		void SheetCollection_Changed(object sender, SheetCollectionChangedEventArgs e)
		{
			this.Invalidate();
			this.OnSheetCollectionChanged(e);
		}

		internal event EventHandler<SheetCollectionChangedEventArgs> Changed;

		void OnSheetCollectionChanged(SheetCollectionChangedEventArgs e)
		{
			T t = e.Sheet as T;
			if (e.Sheet != null && t == null)
			{
				return;
			}
			if (this.Changed != null)
			{
				this.Changed(this, e);
			}
		}

		internal void Invalidate()
		{
			this.allSheetsInvalidated = true;
		}

		List<T> GetAllOfT()
		{
			if (this.allSheetsInvalidated)
			{
				this.allSheets = new List<T>();
				this.allSheets.AddRange(this);
				this.allSheetsInvalidated = false;
			}
			return this.allSheets;
		}

		int GetCount()
		{
			return this.GetAllOfT().Count;
		}

		public T Add()
		{
			return this.Insert(this.Count);
		}

		public T Insert(int index)
		{
			List<T> allOfT = this.GetAllOfT();
			Guard.ThrowExceptionIfOutOfRange<int>(0, allOfT.Count, index, "index");
			int index2;
			if (index == allOfT.Count)
			{
				if (index == 0)
				{
					index2 = this.sheetCollection.Count;
				}
				else
				{
					index2 = this.sheetCollection.IndexOf(allOfT[index - 1]) + 1;
				}
			}
			else
			{
				index2 = this.sheetCollection.IndexOf(allOfT[index]);
			}
			return (T)((object)this.sheetCollection.Insert(index2, this.sheetType));
		}

		public int IndexOf(T item)
		{
			List<T> allOfT = this.GetAllOfT();
			return allOfT.IndexOf(item);
		}

		public int IndexOf(string sheetName)
		{
			List<T> allOfT = this.GetAllOfT();
			for (int i = 0; i < allOfT.Count; i++)
			{
				T t = allOfT[i];
				if (SheetCollection.AreEqualIgnoreCaseSheetNames(t.Name, sheetName))
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

		public bool Contains(T item)
		{
			return this.IndexOf(item) >= 0;
		}

		public T GetByName(string sheetName)
		{
			foreach (T result in this)
			{
				if (SheetCollection.AreEqualIgnoreCaseSheetNames(result.Name, sheetName))
				{
					return result;
				}
			}
			return default(T);
		}

		public void RemoveAt(int index)
		{
			List<T> allOfT = this.GetAllOfT();
			T t = allOfT[index];
			this.sheetCollection.Remove(t);
		}

		public bool Remove(string sheetName)
		{
			T byName = this.GetByName(sheetName);
			return this.Remove(byName);
		}

		public bool Remove(T item)
		{
			return this.sheetCollection.Remove(item);
		}

		public void Clear()
		{
			List<T> allOfT = this.GetAllOfT();
			for (int i = allOfT.Count - 1; i >= 0; i--)
			{
				this.sheetCollection.Remove(allOfT[i]);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (Sheet sheet in this.sheetCollection)
			{
				T t = sheet as T;
				if (t != null)
				{
					yield return t;
				}
			}
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		readonly SheetCollection sheetCollection;

		readonly SheetType sheetType;

		List<T> allSheets;

		bool allSheetsInvalidated;
	}
}
